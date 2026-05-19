using Data;
using Logic;

namespace LogicTest
{
    [TestClass]
    public sealed class LogicApiTest
    {
        private IDataApi _dataApi = new DataApi();
        private ILogicApi _logicApi = new LogicApi();

        private const int BoardWidth = 300;
        private const int BoardHeight = 200;

        [TestInitialize]
        public void InitializeTests()
        {
            _dataApi = new DataApi();
            _logicApi = new LogicApi();
        }

        [TestMethod]
        public void StartCallbackReturnsCorrectBallsAndBoard()
        {
            const int ballsCount = 5;
            IBoard? callbackBoard = null;
            List<IBall>? callbackBalls = null;

            _logicApi.Start(ballsCount, (board, balls) =>
            {
                callbackBoard = board;
                callbackBalls = balls;
            });

            Assert.IsNotNull(callbackBoard);
            Assert.IsNotNull(callbackBalls);
            Assert.HasCount(ballsCount, callbackBalls);
            ThenBallsAreAssignedToBoard(callbackBoard, callbackBalls);

        }

        private void ThenBallsAreAssignedToBoard(IBoard board, List<IBall> balls)
        {
            foreach (IBall ball in balls)
            {
                Assert.Contains(ball, board.Balls);
            }
        }

        [TestMethod]
        public void MoveBallsAddsVelocityVectorToPosition()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ball = _dataApi.CreateBall(board);
            ball.Position = new Position(10, 10);
            ball.Velocity = new Velocity(5, 7);
            IPosition expectedPosition = new Position(15, 17);

            _logicApi.MoveBallsOnce(board);

            Assert.AreEqual(expectedPosition, ball.Position);
        }

        [TestMethod]
        public void MoveBallsChangesVelocityOnCollisionsWithWalls()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ballCollidingWithLeftWall = _dataApi.CreateBall(board);
            ballCollidingWithLeftWall.Position = new Position(3, 10);
            ballCollidingWithLeftWall.Velocity = new Velocity(-5, 0);
            IBall ballCollidingWithRightWall = _dataApi.CreateBall(board);
            ballCollidingWithRightWall.Position = new Position(board.Width - 3, 10);
            ballCollidingWithRightWall.Velocity = new Velocity(5, 0);
            IBall ballCollidingWithTopWall = _dataApi.CreateBall(board);
            ballCollidingWithTopWall.Position = new Position(10, 3);
            ballCollidingWithTopWall.Velocity = new Velocity(0, -5);
            IBall ballCollidingWithBottomWall = _dataApi.CreateBall(board);
            ballCollidingWithBottomWall.Position = new Position(10, board.Height - 3);
            ballCollidingWithBottomWall.Velocity = new Velocity(0, 5);

            _logicApi.MoveBallsOnce(board);

            Assert.AreEqual(new Velocity(5, 0), ballCollidingWithLeftWall.Velocity);
            Assert.AreEqual(new Velocity(-5, 0), ballCollidingWithRightWall.Velocity);
            Assert.AreEqual(new Velocity(0, 5), ballCollidingWithTopWall.Velocity);
            Assert.AreEqual(new Velocity(0, -5), ballCollidingWithBottomWall.Velocity);
        }

        [TestMethod]
        public void MoveBallsChangesPositionBasedOnNewVelocityOnCollisionWithWalls()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ballCollidingWithLeftWall = _dataApi.CreateBall(board);
            ballCollidingWithLeftWall.Position = new Position(3, 10);
            ballCollidingWithLeftWall.Velocity = new Velocity(-5, 0);
            IBall ballCollidingWithRightWall = _dataApi.CreateBall(board);
            ballCollidingWithRightWall.Position = new Position(board.Width - 3, 10);
            ballCollidingWithRightWall.Velocity = new Velocity(5, 0);
            IBall ballCollidingWithTopWall = _dataApi.CreateBall(board);
            ballCollidingWithTopWall.Position = new Position(10, 3);
            ballCollidingWithTopWall.Velocity = new Velocity(0, -5);
            IBall ballCollidingWithBottomWall = _dataApi.CreateBall(board);
            ballCollidingWithBottomWall.Position = new Position(10, board.Height - 3);
            ballCollidingWithBottomWall.Velocity = new Velocity(0, 5);

            _logicApi.MoveBallsOnce(board);

            double radius = ballCollidingWithLeftWall.Radius;
            Assert.AreEqual(new Position(2 + 2 * radius, 10), ballCollidingWithLeftWall.Position);
            Assert.AreEqual(new Position(board.Width - 2 - 2 * radius, 10), ballCollidingWithRightWall.Position);
            Assert.AreEqual(new Position(10, 2 + 2 * radius), ballCollidingWithTopWall.Position);
            Assert.AreEqual(new Position(10, board.Height - 2 - 2 * radius), ballCollidingWithBottomWall.Position);
        }

        [TestMethod]
        public void MoveBallsHandlesDirectBallsCollision()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ball1 = _dataApi.CreateBall(board);
            ball1.Position = new Position(10, 10);
            ball1.Velocity = new Velocity(4, 0);
            ball1.Weight = 1;
            IBall ball2 = _dataApi.CreateBall(board);
            ball2.Position = new Position(12, 10);
            ball2.Velocity = new Velocity(-4, 0);
            ball2.Weight = 1;

            _logicApi.MoveBallsOnce(board);

            Assert.IsLessThan(0, ball1.Velocity.X);
            Assert.IsGreaterThan(0, ball2.Velocity.X);
        }

        [TestMethod]
        public void MoveBallsTakesWeightIntoAccountWhenHandlingBallsCollision()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ball1 = _dataApi.CreateBall(board);
            ball1.Position = new Position(10, 10);
            ball1.Velocity = new Velocity(4, 0);
            ball1.Weight = 5;
            IBall ball2 = _dataApi.CreateBall(board);
            ball2.Position = new Position(12, 10);
            ball2.Velocity = new Velocity(-4, 0);
            ball2.Weight = 1;

            _logicApi.MoveBallsOnce(board);

            Assert.IsGreaterThan(0, ball1.Velocity.X);
            Assert.IsGreaterThan(0, ball2.Velocity.X);
        }

        [TestMethod]
        public void MoveBallsHandlesBallsCollisionAtAngle()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ball1 = _dataApi.CreateBall(board);
            ball1.Position = new Position(9, 16);
            ball1.Velocity = new Velocity(0, -3);
            ball1.Weight = 1;
            IBall ball2 = _dataApi.CreateBall(board);
            ball2.Position = new Position(10, 10);
            ball2.Velocity = new Velocity(0, 3);
            ball2.Weight = 1;

            _logicApi.MoveBallsOnce(board);

            Assert.IsLessThan(0, ball1.Velocity.X);
            Assert.IsGreaterThan(0, ball1.Velocity.Y);
            Assert.IsGreaterThan(0, ball2.Velocity.X);
            Assert.IsLessThan(0, ball2.Velocity.Y);
        }

        [TestMethod]
        public void MoveBallsShouldNotChangeBallsVelocitiesWhenTheyDoNotCollide()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ball1 = _dataApi.CreateBall(board);
            ball1.Position = new Position(10, 10);
            ball1.Velocity = new Velocity(5, 0);
            IBall ball2 = _dataApi.CreateBall(board);
            ball2.Position = new Position(50, 50);
            ball2.Velocity = new Velocity(-5, 0);

            _logicApi.MoveBallsOnce(board);

            Assert.AreEqual(new Velocity(5, 0), ball1.Velocity);
            Assert.AreEqual(new Velocity(-5, 0), ball2.Velocity);
        }

        [TestMethod]
        public void MoveBallsShouldNotHandleBallsCollisionWhenTheyAreInEachOtherAndMovingAway()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ball1 = _dataApi.CreateBall(board);
            ball1.Position = new Position(10, 10);
            ball1.Velocity = new Velocity(-5, 0);
            IBall ball2 = _dataApi.CreateBall(board);
            ball2.Position = new Position(13, 10);
            ball2.Velocity = new Velocity(5, 0);

            _logicApi.MoveBallsOnce(board);

            Assert.AreEqual(new Velocity(-5, 0), ball1.Velocity);
            Assert.AreEqual(new Velocity(5, 0), ball2.Velocity);
        }

        [TestMethod]
        public void MoveBallsPreservesEnergyAfterCollision()
        {
            IBoard board = _dataApi.CreateBoard(BoardWidth, BoardHeight);
            IBall ball1 = _dataApi.CreateBall(board);
            ball1.Position = new Position(10, 10);
            ball1.Velocity = new Velocity(4, 0);
            ball1.Weight = 1;
            IBall ball2 = _dataApi.CreateBall(board);
            ball2.Position = new Position(12, 10);
            ball2.Velocity = new Velocity(-4, 0);
            ball2.Weight = 1;
            double internalEnergyBefore = CalculateInternalEnergy(ball1, ball2);

            _logicApi.MoveBallsOnce(board);

            double internalEnergyAfter = CalculateInternalEnergy(ball1, ball2);
            Assert.AreEqual(internalEnergyBefore, internalEnergyAfter, 0.00001);
        }

        private double CalculateInternalEnergy(IBall ball1, IBall ball2)
        {
            return 0.5 * ball1.Weight * (Math.Pow(ball1.Velocity.X, 2) + Math.Pow(ball1.Velocity.Y, 2))
                   + 0.5 * ball2.Weight * (Math.Pow(ball2.Velocity.X, 2) + Math.Pow(ball2.Velocity.Y, 2));
        }
    }
}
