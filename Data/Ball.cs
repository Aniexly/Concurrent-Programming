using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : IBall
    {
        public Guid Id { get; }
        public double Radius { get; }
        public double Weight { get; set; }
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

        public Ball(double radius, double weight) : this(radius, weight, new Position(), new Velocity()) { }

        public Ball(double radius, double weight, IPosition position) : this(radius, weight, position, new Velocity()) { }

        public Ball(double radius, double weight, IVelocity velocity) : this(radius, weight, new Position(), velocity) { }

        public Ball(double radius, double weight, IPosition position, IVelocity velocity)
        {
            Id = Guid.NewGuid();
            Radius = radius;
            Weight = weight;
            Position = position;
            Velocity = velocity;
            SubscribeToPositionEvents();
        }

        public override string ToString()
        {
            return $"Id: {Id}, Radius: {Radius}, Weight: {Weight}, Position: {Position}, Velocity: {Velocity}";
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
