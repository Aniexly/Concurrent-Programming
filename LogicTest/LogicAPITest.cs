using Data;
using Logic;

namespace LogicTest
{
    [TestClass]
    public sealed class LogicAPITest
    {
        private IDataAPI dataAPI = new DataAPI();
        private ILogicAPI logicAPI = new LogicAPI();

        [TestInitialize]
        public void InitializeTests()
        {
            dataAPI = new DataAPI();
            logicAPI = new LogicAPI();
        }

        [TestMethod]
        public void MoveBallsAddsVelocityVectorToPosition()
        {
            IBoard board = dataAPI.CreateBoard();
            Assert.IsGreaterThan(20, board.Width);
            Assert.IsGreaterThan(20, board.Height);
            IBall ball = dataAPI.CreateBall(board);
            ball.Position = new Position(10, 10);
            ball.Velocity = new Velocity(5, 7);
            IPosition expectedPosition = new Position(15, 17);

            logicAPI.MoveBalls(board);

            Assert.AreEqual(expectedPosition, ball.Position);
        }

        [TestMethod]
        public void MoveBallsChangesVelocityOnCollisionsWithWalls()
        {
            IBoard board = dataAPI.CreateBoard();
            Assert.IsGreaterThan(20, board.Width);
            Assert.IsGreaterThan(20, board.Height);
            IBall ballCollidingWithLeftWall = dataAPI.CreateBall(board);
            ballCollidingWithLeftWall.Position = new Position(3, 10);
            ballCollidingWithLeftWall.Velocity = new Velocity(-5, 0);
            IBall ballCollidingWithRightWall = dataAPI.CreateBall(board);
            ballCollidingWithRightWall.Position = new Position(board.Width - 3, 10);
            ballCollidingWithRightWall.Velocity = new Velocity(5, 0);
            IBall ballCollidingWithTopWall = dataAPI.CreateBall(board);
            ballCollidingWithTopWall.Position = new Position(10, 3);
            ballCollidingWithTopWall.Velocity = new Velocity(0, -5);
            IBall ballCollidingWithBottomWall = dataAPI.CreateBall(board);
            ballCollidingWithBottomWall.Position = new Position(10, board.Height - 3);
            ballCollidingWithBottomWall.Velocity = new Velocity(0, 5);

            logicAPI.MoveBalls(board);

            Assert.AreEqual(new Velocity(5, 0), ballCollidingWithLeftWall.Velocity);
            Assert.AreEqual(new Velocity(-5, 0), ballCollidingWithRightWall.Velocity);
            Assert.AreEqual(new Velocity(0, 5), ballCollidingWithTopWall.Velocity);
            Assert.AreEqual(new Velocity(0, -5), ballCollidingWithBottomWall.Velocity);
        }

        [TestMethod]
        public void MoveBallsChangesPositionBasedOnNewVelocityOnCollision()
        {
            IBoard board = dataAPI.CreateBoard();
            Assert.IsGreaterThan(20, board.Width);
            Assert.IsGreaterThan(20, board.Height);
            IBall ballCollidingWithLeftWall = dataAPI.CreateBall(board);
            ballCollidingWithLeftWall.Position = new Position(3, 10);
            ballCollidingWithLeftWall.Velocity = new Velocity(-5, 0);
            IBall ballCollidingWithRightWall = dataAPI.CreateBall(board);
            ballCollidingWithRightWall.Position = new Position(board.Width - 3, 10);
            ballCollidingWithRightWall.Velocity = new Velocity(5, 0);
            IBall ballCollidingWithTopWall = dataAPI.CreateBall(board);
            ballCollidingWithTopWall.Position = new Position(10, 3);
            ballCollidingWithTopWall.Velocity = new Velocity(0, -5);
            IBall ballCollidingWithBottomWall = dataAPI.CreateBall(board);
            ballCollidingWithBottomWall.Position = new Position(10, board.Height - 3);
            ballCollidingWithBottomWall.Velocity = new Velocity(0, 5);

            logicAPI.MoveBalls(board);

            double radius = ballCollidingWithLeftWall.Radius;
            Assert.AreEqual(new Position(2 + 2 * radius, 10), ballCollidingWithLeftWall.Position);
            Assert.AreEqual(new Position(board.Width - 2 - 2 * radius, 10), ballCollidingWithRightWall.Position);
            Assert.AreEqual(new Position(10, 2 + 2 * radius), ballCollidingWithTopWall.Position);
            Assert.AreEqual(new Position(10, board.Height - 2 - 2 * radius), ballCollidingWithBottomWall.Position);
        }
    }
}
