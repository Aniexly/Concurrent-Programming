using Data;

namespace Logic
{
    public class LogicAPI : ILogicAPI
    {
        public void MoveBalls(IBoard board)
        {
            foreach (var ball in board.Balls)
            {
                MoveBall(ball, board);
            }
        }

        private void MoveBall(IBall ball, IBoard board)
        {
            HandleCollisionWithWalls(ball, board);
            float newX = ball.Position.X + ball.Velocity.X;
            float newY = ball.Position.Y + ball.Velocity.Y;
            ball.Position.X = newX;
            ball.Position.Y = newY;
        }

        private void HandleCollisionWithWalls(IBall ball, IBoard board)
        {
            float newX = ball.Position.X + ball.Velocity.X;
            float newY = ball.Position.Y + ball.Velocity.Y;
            if (newX - ball.Radius < 0 || newX + ball.Radius > board.Width)
            {
                ball.Velocity.X = -ball.Velocity.X;
            }
            if (newY - ball.Radius < 0 || newY + ball.Radius > board.Height)
            {
                ball.Velocity.Y = -ball.Velocity.Y;
            }
        }
    }
}
