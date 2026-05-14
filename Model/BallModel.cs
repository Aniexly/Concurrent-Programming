using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;

namespace Model
{
    public class BallModel : IBallModel
    {
        private readonly IBall _ball;
        private const double Scale = 2.0;
        private static readonly Dictionary<double, string> WeightColorDict = new Dictionary<double, string>
        {
            {1, "Red"},
            {2, "Green"},
            {3, "Blue"},
            {4, "Yellow"},
            {5, "Purple"}
        };
        public double PositionX => (_ball.Position.X - _ball.Radius) * Scale;
        public double PositionY => (_ball.Position.Y - _ball.Radius) * Scale;
        public double Diameter => _ball.Radius * 2 * Scale;
        public string Color => WeightColorDict.GetValueOrDefault(_ball.Weight, "Black");

        public event PropertyChangedEventHandler? PropertyChanged;

        public BallModel(IBall ball)
        {
            _ball = ball;
            SubscribeToBallEvents();
        }

        private void SubscribeToBallEvents()
        {
            _ball.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(_ball.Position))
                {
                    OnPropertyChanged(nameof(PositionX));
                    OnPropertyChanged(nameof(PositionY));
                }
            };
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
