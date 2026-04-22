using Data;

namespace DataTest
{
    [TestClass]
    public sealed class BoardTest
    {
        [TestMethod]
        public void BoardHasNoBallsAfterInitialization()
        {
            int width = 10;
            int height = 10;

            IBoard board = new Board(width, height);

            Assert.IsEmpty(board.Balls);
        }
    }
}
