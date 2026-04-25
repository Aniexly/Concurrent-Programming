using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data
{
    public interface IPosition : INotifyPropertyChanged
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
