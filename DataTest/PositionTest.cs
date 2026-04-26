using Data;

namespace DataTest
{
    [TestClass]
    public sealed class PositionTest
    {
        [TestMethod]
        public void PositionNotifiesOnPropertyChange()
        {
            IPosition position = new Position();
            string? changedPropertyName = null;
            position.PropertyChanged += (_, e) => changedPropertyName = e.PropertyName;

            position.X = 10;

            Assert.AreEqual("X", changedPropertyName);

            changedPropertyName = null;
            position.Y = 10;

            Assert.AreEqual("Y", changedPropertyName);
        }
    }
}
