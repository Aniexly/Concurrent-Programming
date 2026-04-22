using Data;

namespace DataTest
{
    [TestClass]
    public sealed class DataAPITest
    {
        [TestMethod]
        public void CreateBallAddsBallToBoard()
        {
            IDataAPI data = new DataAPI();
            IBoard board = data.CreateBoard();

            IBall ball = data.CreateBall(board);

            Assert.Contains(ball, board.Balls);
            Assert.AreEqual(1, board.Balls.Count);
        }
    }
}
