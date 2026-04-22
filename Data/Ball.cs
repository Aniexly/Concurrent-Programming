namespace Data
{
    public class Ball : IBall
    {
        public float Radius { get; }
        public IPosition Position { get; set; } = new Position();
        public IVelocity Velocity { get; set; } = new Velocity();

        public Ball(int radius) => (Radius) = (radius);

        public Ball(int radius, IPosition position) => (Radius, Position) = (radius, position);

        public Ball(int radius, IVelocity velocity) => (Radius, Velocity) = (radius, velocity);

        public Ball(int radius, IPosition position, IVelocity velocity) => (Radius, Position, Velocity) = (radius, position, velocity);
    }
}
