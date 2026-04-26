using Data;

namespace DataTest
{
    [TestClass]
    public sealed class BallTest
    {
        [TestMethod]
        public void BallNotifiesOnPropertyChange()
        {
            IBall ball = new Ball(5);
            string? changedPropertyName = null;
            ball.PropertyChanged += (sender, e) => changedPropertyName = e.PropertyName;

            ball.Position = new Position(10, 10);

            Assert.AreEqual("Position", changedPropertyName);
        }

        [TestMethod]
        public void BallNotifiesOnSubscribedNotifications()
        {
            IBall ball = new Ball(5);
            string? changedPropertyName = null;
            ball.PropertyChanged += (sender, e) => changedPropertyName = e.PropertyName;

            ball.Position.X = 10;

            Assert.AreEqual("Position", changedPropertyName);
        }

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
