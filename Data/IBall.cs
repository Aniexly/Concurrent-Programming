using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        public Guid Id { get; }
        public double Radius { get; }
        public double Weight { get; set; }
        public IPosition Position { get; set; }
        public IVelocity Velocity { get; set; }
    }
}
