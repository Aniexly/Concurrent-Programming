using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;

namespace Model
{
    public class BallModel : IBallModel
    {
        private IBall ball;
        public double PositionX => ball.Position.X - ball.Radius;
        public double PositionY => ball.Position.Y - ball.Radius;
        public double Diameter => ball.Radius * 2;

        public event PropertyChangedEventHandler? PropertyChanged;

        public BallModel(IBall ball)
        {
            this.ball = ball;
            SubscribeToBallEvents();
        }

        private void SubscribeToBallEvents()
        {
            ball.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ball.Position))
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
