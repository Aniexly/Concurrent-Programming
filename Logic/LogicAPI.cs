using Data;

namespace Logic
{
    public class LogicAPI : ILogicAPI
    {
        private IDataAPI IDataAPI = new DataAPI();
        private Timer timer;
        private int fps = 60;

        public void Start(int ballsCount, Action<IBoard, List<IBall>> callback)
        {
            IBoard board = IDataAPI.CreateBoard();
            List<IBall> balls = new List<IBall>(ballsCount);
            for (int i = 0; i < ballsCount; i++)
            {
                IBall ball = IDataAPI.CreateBall(board);
                balls.Add(ball);
            }
            callback(board, balls);
            StartMovingBalls(board);
        }
        public void StartMovingBalls(IBoard board)
        {
            int intervalMs = 1000 / fps;
            timer = new Timer(callback: _ => MoveBalls(board), state: null, dueTime: 0, period: intervalMs);
        }

        public void MoveBalls(IBoard board)
        {
            foreach (var ball in board.Balls)
            {
                MoveBall(ball, board);
            }
        }

        private void MoveBall(IBall ball, IBoard board)
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
