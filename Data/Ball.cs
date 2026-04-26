using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : IBall
    {
        public double Radius { get; }

        public IPosition Position
        {
            get;
            set
            {
                field = value;
                SubscribeToPositionEvents();
                OnPropertyChanged();
            }
        }
        public IVelocity Velocity { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Ball(int radius) : this(radius, new Position(), new Velocity()) { }

        public Ball(int radius, IPosition position) : this(radius, position, new Velocity()) { }

        public Ball(int radius, IVelocity velocity) : this(radius, new Position(), velocity) { }

        public Ball(int radius, IPosition position, IVelocity velocity)
        {
            Radius = radius;
            Position = position;
            Velocity = velocity;
            SubscribeToPositionEvents();
        }

        private void SubscribeToPositionEvents()
        {
            Position.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName is nameof(Position.X) or nameof(Position.Y))
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
