using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IBall
    {
        public float Radius { get; }
        public IPosition Position { get; set; }
        public IVelocity Velocity { get; set; }
    }
}
