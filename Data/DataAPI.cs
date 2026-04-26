using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DataAPI : IDataAPI
    {
        private readonly Random random = new Random();

        private readonly int boardWidth = 300;
        private readonly int boardHeight = 200;
        private readonly IPosition minPosition = new Position(0, 0);
        private IPosition maxPosition = new Position(0, 0);

        private readonly int ballRadius = 4;
        private readonly double ballSpeed = 2;

        public IBoard CreateBoard()
        {
            return new Board(boardWidth, boardHeight);
        }

        public IBall CreateBall(IBoard board)
        {
            maxPosition = new Position(board.Width, board.Height);
            IPosition position = CreateRandomPosition();
            IVelocity velocity = CreateRandomVelocity();
            IBall ball = new Ball(ballRadius, position, velocity);
            board.Balls.Add(ball);
            return ball;
        }

        private IPosition CreateRandomPosition()
        {
            double x = random.NextDouble() * (maxPosition.X - minPosition.X) + minPosition.X;
            double y = random.NextDouble() * (maxPosition.Y - minPosition.Y) + minPosition.Y;
            return new Position(x, y);
        }

        private IVelocity CreateRandomVelocity()
        {
            double angle = random.NextDouble() * 2 * Math.PI;
            double velocityX = Math.Cos(angle) * ballSpeed;
            double velocityY = Math.Sin(angle) * ballSpeed;
            return new Velocity(velocityX, velocityY);
        }
    }
}
