using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Velocity : IVelocity, IEquatable<Velocity>
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Velocity() => (X, Y) = (0, 0);

        public Velocity(double x, double y) => (X, Y) = (x, y);
        public bool Equals(Velocity? other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Velocity)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
