using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DataApi : IDataApi
    {
        private readonly Random _random = new Random();

        private const int BoardWidth = 300;
        private const int BoardHeight = 200;
        private readonly IPosition _minPosition = new Position(0, 0);
        private const int BallRadius = 4;
        private const double BallSpeed = 2;

        public IBoard CreateBoard()
        {
            return new Board(BoardWidth, BoardHeight);
        }

        public IBall CreateBall(IBoard board)
        {
            IPosition maxPosition = new Position(board.Width, board.Height);
            IPosition position = CreateRandomPosition(maxPosition);
            IVelocity velocity = CreateRandomVelocity();
            IBall ball = new Ball(BallRadius, position, velocity);
            board.Balls.Add(ball);
            return ball;
        }

        private IPosition CreateRandomPosition(IPosition maxPosition)
        {
            double x = _random.NextDouble() * (maxPosition.X - _minPosition.X) + _minPosition.X;
            double y = _random.NextDouble() * (maxPosition.Y - _minPosition.Y) + _minPosition.Y;
            return new Position(x, y);
        }

        private IVelocity CreateRandomVelocity()
        {
            double angle = _random.NextDouble() * 2 * Math.PI;
            double velocityX = Math.Cos(angle) * BallSpeed;
            double velocityY = Math.Sin(angle) * BallSpeed;
            return new Velocity(velocityX, velocityY);
        }
    }
}
