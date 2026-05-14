using Data;

namespace DataTest
{
    [TestClass]
    public sealed class BallTest
    {
        [TestMethod]
        public void BallNotifiesOnPropertyChange()
        {
            const double radius = 5;
            const double weight = 5;
            IBall ball = new Ball(radius, weight);
            string? changedPropertyName = null;
            ball.PropertyChanged += (_, e) => changedPropertyName = e.PropertyName;

            ball.Position = new Position(10, 10);

            Assert.AreEqual("Position", changedPropertyName);
        }

        [TestMethod]
        public void BallNotifiesOnSubscribedNotifications()
        {
            const double radius = 5;
            const double weight = 5;
            IBall ball = new Ball(radius, weight);
            string? changedPropertyName = null;
            ball.PropertyChanged += (_, e) => changedPropertyName = e.PropertyName;

            ball.Position.X = 10;

            Assert.AreEqual("Position", changedPropertyName);
        }

        [TestMethod]
        public void BallsPositionIsZeroAfterInitialization()
        {
            const int radius = 5;
            const double weight = 5;

            IBall ball = new Ball(radius, weight);

            Assert.AreEqual(0, ball.Position.X);
            Assert.AreEqual(0, ball.Position.Y);
        }

        [TestMethod]
        public void BallHasNoVelocityAfterInitialization()
        {
            const int radius = 5;
            const double weight = 5;

            IBall ball = new Ball(radius, weight);

            Assert.AreEqual(0, ball.Velocity.X);
            Assert.AreEqual(0, ball.Velocity.Y);
        }
    }
}
