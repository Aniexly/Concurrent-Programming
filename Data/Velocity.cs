using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Velocity : IVelocity
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Velocity() => (X, Y) = (0, 0);

        public Velocity(float x, float y) => (X, Y) = (x, y);
    }
}
