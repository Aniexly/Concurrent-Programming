using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Position : IPosition
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position() => (X, Y) = (0, 0);

        public Position(float x, float y) => (X, Y) = (x, y);
    }
}
