using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DataAPI : IDataAPI
    {
        private readonly Random random = new Random();

        private readonly int boardWidth = 800;
        private readonly int boardHeight = 600;
        private readonly IPosition minPosition = new Position(0, 0);
        private IPosition maxPosition = new Position(0, 0);

        private readonly int ballRadius = 5;
        private readonly float ballSpeed = 5f;

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
            float x = random.NextSingle() * ((int)maxPosition.X - (int)minPosition.X) + (int)minPosition.X;
            float y = random.NextSingle() * ((int)maxPosition.Y - (int)minPosition.Y) + (int)minPosition.Y;
            return new Position(x, y);
        }

        private IVelocity CreateRandomVelocity()
        {
            float angle = (float)(random.NextSingle() * 2 * Math.PI);
            float velocityX = (float)(Math.Cos(angle) * ballSpeed);
            float velocityY = (float)(Math.Sin(angle) * ballSpeed);
            return new Velocity(velocityX, velocityY);
        }
    }
}
