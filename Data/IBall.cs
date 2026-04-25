using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        public double Radius { get; }
        public IPosition Position { get; set; }
        public IVelocity Velocity { get; set; }
    }
}
