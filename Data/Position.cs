using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Position : IPosition, IEquatable<Position>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position() => (X, Y) = (0, 0);

        public Position(float x, float y) => (X, Y) = (x, y);

        public bool Equals(Position? other)
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
            return Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
