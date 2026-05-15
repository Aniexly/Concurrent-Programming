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
                HandleCollisionWithWallAndMove(ball, board);
                MakeSureThatBallIsInsideBoard(ball, board);
            });
        }

        private void HandleCollisionWithBall(IBall ball, IBoard board)
        {
            List<IBall> possiblyCollidingBalls = FindPossiblyCollidingBalls(ball, board);
            foreach (IBall otherBall in possiblyCollidingBalls)
            {
                try
                {
                    if (DoBallsCollide(ball, otherBall))
                    {
                        BounceBalls(ball, otherBall);
                    }
                }
                finally
                {
                    Monitor.Exit(otherBall);
                }
            }
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

        private double CalculateDistance(IPosition position1, IPosition position2)
        {
            double dx = position1.X - position2.X;
            double dy = position1.Y - position2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private double CalculateDistance(double dx, double dy)
        {
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private bool DoBallsCollide(IBall ball, IBall otherBall)
        {
            return CalculateDistance(ball.Position, otherBall.Position) < ball.Radius + otherBall.Radius;
        }

        private void BounceBalls(IBall ball1, IBall ball2)
        {
            double dx = ball1.Position.X - ball2.Position.X;
            double dy = ball1.Position.Y - ball2.Position.Y;
            double distance = CalculateDistance(dx, dy);

            if (distance == 0)
            {
                return;
            }

            double cosdx = dx / distance;
            double cosdy = dy / distance;

            if (!AreMovingTowardsEachOther(ball1, ball2, cosdx, cosdy))
            {
                return;
            }

            double v1Before = ball1.Velocity.X * cosdx + ball1.Velocity.Y * cosdy;
            double v2Before = ball2.Velocity.X * cosdx + ball2.Velocity.Y * cosdy;
            (double v1After, double v2After) = CalculateVelocitiesAfterCollision(v1Before, v2Before,
                ball1.Weight, ball2.Weight);

            ball1.Velocity.X += (v1After - v1Before) * cosdx;
            ball1.Velocity.Y += (v1After - v1Before) * cosdy;
            ball2.Velocity.X += (v2After - v2Before) * cosdx;
            ball2.Velocity.Y += (v2After - v2Before) * cosdy;
        }

        private bool AreMovingTowardsEachOther(IBall ball1, IBall ball2, double cosdx, double cosdy)
        {
            double dvx = ball1.Velocity.X - ball2.Velocity.X;
            double dvy = ball1.Velocity.Y - ball2.Velocity.Y;
            double dot = CalculateDotProduct(dvx, dvy, cosdx, cosdy);
            return dot < 0;
        }

        private double CalculateDotProduct(double dx, double dy, double cosx, double cosy)
        {
            return dx * cosx + dy * cosy;
        }

        private (double, double) CalculateVelocitiesAfterCollision(double v1Before, double v2Before, double m1, double m2)
        {
            double v1After = (v1Before * (m1 - m2) + 2 * m2 * v2Before) / (m1 + m2);
            double v2After = (v2Before * (m2 - m1) + 2 * m1 * v1Before) / (m1 + m2);
            return (v1After, v2After);
        }

        private void HandleCollisionWithWallAndMove(IBall ball, IBoard board)
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

        private void MakeSureThatBallIsInsideBoard(IBall ball, IBoard board)
        {
            if (ball.Position.X - ball.Radius < 0)
            {
                ball.Position.X = ball.Radius;
            }
            else if (ball.Position.X + ball.Radius > board.Width)
            {
                ball.Position.X = board.Width - ball.Radius;
            }

            if (ball.Position.Y - ball.Radius < 0)
            {
                ball.Position.Y = ball.Radius;
            }
            else if (ball.Position.Y + ball.Radius > board.Height)
            {
                ball.Position.Y = board.Height - ball.Radius;
            }
        }
    }
}
