using Data;

namespace DataTest
{
    [TestClass]
    public sealed class BallTest
    {
        [TestMethod]
        public void BallsPositionIsZeroAfterInitialization()
        {
            int radius = 1;

            IBall ball = new Ball(radius);

            Assert.AreEqual(0, ball.Position.X);
            Assert.AreEqual(0, ball.Position.Y);
        }

        [TestMethod]
        public void BallHasNoVelocityAfterInitialization()
        {
            int radius = 1;

            IBall ball = new Ball(radius);

            Assert.AreEqual(0, ball.Velocity.X);
            Assert.AreEqual(0, ball.Velocity.Y);
        }
    }
}
