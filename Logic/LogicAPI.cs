using Data;

namespace Logic
{
    public class LogicApi : ILogicApi
    {
        private readonly IDataApi _dataApi = new DataApi();

        private Timer? _timer;
        private const int Fps = 60;
        private object _moveBallsSync = new object();

        public async Task Start(int ballsCount, Action<IBoard, List<IBall>> callback)
        {
            CleanBeforeStart();
            IBoard board = _dataApi.CreateBoard();
            List<IBall> balls = new List<IBall>(ballsCount);
            for (int i = 0; i < ballsCount; i++)
            {
                IBall ball = _dataApi.CreateBall(board);
                balls.Add(ball);
            }
            callback(board, balls);
            StartMovingBalls(board);
        }

        private void CleanBeforeStart()
        {
            _timer?.Dispose();
            _moveBallsSync = new object();
        }

        public void StartMovingBalls(IBoard board)
        {
            const int intervalMs = 1000 / Fps;
            _timer = new Timer(callback: _ => MoveBalls(board), state: null, dueTime: 0, period: intervalMs);
        }

        public void MoveBalls(IBoard board)
        {
            if (Monitor.TryEnter(_moveBallsSync))
            {
                try
                {
                    foreach (IBall ball in board.Balls)
                    {
                        lock (ball)
                        {
                            MoveBall(ball, board);
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(_moveBallsSync);
                }
            }
        }

        private async Task MoveBall(IBall ball, IBoard board)
        {
            await Task.Run(() =>
            {
                HandleCollisionWithBall(ball, board);
                HandleCollisionWithWall(ball, board);
            });
        }

        private void HandleCollisionWithBall(IBall ball, IBoard board)
        {
            List<IBall> possiblyCollidingBalls = FindPossiblyCollidingBalls(ball, board);
            foreach (IBall otherBall in possiblyCollidingBalls)
            {
                try
                {
                    if (DoesCollideWithBall(ball, otherBall))
                    {
                        BounceBalls(ball, otherBall);
                    }
                }
                finally
                {
                    Monitor.Exit(otherBall);
                }
            }
            ball.Position.X += ball.Velocity.X;
            ball.Position.Y += ball.Velocity.Y;
        }

        private List<IBall> FindPossiblyCollidingBalls(IBall ball, IBoard board)
        {
            return board.Balls
                .Where((otherBall) => otherBall != ball)
                .Where((otherBall) => CanBallsCollide(ball, otherBall)).ToList();
        }

        private bool CanBallsCollide(IBall ball, IBall otherBall)
        {
            Monitor.Enter(otherBall);
            double distance = CalculateDistance(ball.Position, otherBall.Position);
            bool doesCollide = distance < ball.Radius + otherBall.Radius + ball.Velocity.GetLength() + otherBall.Velocity.GetLength();
            if (!doesCollide)
            {
                Monitor.Exit(otherBall);
            }
            return doesCollide;
        }

        public double CalculateDistance(IPosition position1, IPosition position2)
        {
            double dx = position1.X - position2.X;
            double dy = position1.Y - position2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private bool DoesCollideWithBall(IBall ball, IBall otherBall)
        {
            return CalculateDistance(ball.Position, otherBall.Position) < ball.Radius + otherBall.Radius;
        }

        private void BounceBalls(IBall ball1, IBall ball2)
        {
            double sumOfWeights = ball1.Weight + ball2.Weight;
            IVelocity velocity1After = AddVelocities(
                MultiplyVelocity(ball1.Velocity, (ball1.Weight - ball2.Weight) / sumOfWeights),
                MultiplyVelocity(ball2.Velocity, (2 * ball2.Weight) / sumOfWeights)
                );
            IVelocity velocity2After = AddVelocities(
                MultiplyVelocity(ball1.Velocity, (2 * ball1.Weight) / sumOfWeights),
                MultiplyVelocity(ball2.Velocity, (ball2.Weight - ball1.Weight) / sumOfWeights)
                );

            ball1.Velocity = velocity1After;
            ball2.Velocity = velocity2After;
        }

        private IVelocity MultiplyVelocity(IVelocity velocity, double scalar)
        {
            return new Velocity(velocity.X * scalar, velocity.Y * scalar);
        }

        private IVelocity AddVelocities(IVelocity velocity1, IVelocity velocity2)
        {
            return new Velocity(velocity1.X + velocity2.X, velocity1.Y + velocity2.Y);
        }



        private void HandleCollisionWithWall(IBall ball, IBoard board)
        {
            if (DoesCollideWithHorizontalWalls(ball, board))
            {
                double distanceToWall = ball.Position.X + ball.Velocity.X - ball.Radius < 0
                    ? -ball.Position.X + ball.Radius
                    : board.Width - ball.Position.X - ball.Radius;
                double xOffset = distanceToWall;
                ball.Velocity.X = -ball.Velocity.X;
                xOffset += ball.Velocity.X + distanceToWall;
                ball.Position.X += xOffset;
            }
            else
            {
                ball.Position.X += ball.Velocity.X;
            }
            if (DoesCollideWithVerticalWalls(ball, board))
            {
                double distanceToWall = ball.Position.Y + ball.Velocity.Y - ball.Radius < 0
                    ? -ball.Position.Y + ball.Radius
                    : board.Height - ball.Position.Y - ball.Radius;
                double yOffset = distanceToWall;
                ball.Velocity.Y = -ball.Velocity.Y;
                yOffset += ball.Velocity.Y + distanceToWall;
                ball.Position.Y += yOffset;
            }
            else
            {
                ball.Position.Y += ball.Velocity.Y;
            }
        }

        private bool DoesCollideWithHorizontalWalls(IBall ball, IBoard board)
        {
            double newX = ball.Position.X + ball.Velocity.X;
            return newX - ball.Radius < 0 || newX + ball.Radius > board.Width;
        }
        private bool DoesCollideWithVerticalWalls(IBall ball, IBoard board)
        {
            double newY = ball.Position.Y + ball.Velocity.Y;
            return newY - ball.Radius < 0 || newY + ball.Radius > board.Height;
        }
    }
}
