using Data;

namespace DataTest
{
    [TestClass]
    public sealed class DataApiTest
    {
        [TestMethod]
        public void CreateBallAddsBallToBoard()
        {
            IDataApi data = new DataApi();
            IBoard board = data.CreateBoard();

            IBall ball = data.CreateBall(board);

            Assert.Contains(ball, board.Balls);
            Assert.AreEqual(1, board.Balls.Count);
        }
    }
}
