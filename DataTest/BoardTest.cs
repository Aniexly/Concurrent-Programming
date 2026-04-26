using Data;

namespace DataTest
{
    [TestClass]
    public sealed class BoardTest
    {
        [TestMethod]
        public void BoardHasNoBallsAfterInitialization()
        {
            const int width = 10;
            const int height = 10;

            IBoard board = new Board(width, height);

            Assert.IsEmpty(board.Balls);
        }
    }
}
