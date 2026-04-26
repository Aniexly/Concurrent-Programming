using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Model
{
    public interface IBallModel : INotifyPropertyChanged
    {
        public double PositionX { get; }
        public double PositionY { get; }
        public double Diameter { get; }
    }
}
