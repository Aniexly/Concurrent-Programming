using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : IBall
    {
        public double Radius { get; }
        private IPosition position = new Position();
        public IPosition Position
        {
            get => position;
            set
            {
                position = value;
                SubscribeToPositionEvents();
                OnPropertyChanged();
            }
        }
        public IVelocity Velocity { get; set; } = new Velocity();

        public event PropertyChangedEventHandler? PropertyChanged;

        public Ball(int radius) => (Radius) = (radius);

        public Ball(int radius, IPosition position) => (Radius, Position) = (radius, position);

        public Ball(int radius, IVelocity velocity) => (Radius, Velocity) = (radius, velocity);

        public Ball(int radius, IPosition position, IVelocity velocity) => (Radius, Position, Velocity) = (radius, position, velocity);

        private void SubscribeToPositionEvents()
        {
            Position.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Position.X) || e.PropertyName == nameof(Position.Y))
                {
                    OnPropertyChanged(nameof(Position));
                }
            };
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
