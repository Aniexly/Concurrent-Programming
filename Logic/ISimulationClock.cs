using System;
using System.ComponentModel;

namespace Logic
{
    public interface ISimulationClock : INotifyPropertyChanged
    {
        TimeSpan Elapsed { get; }

        public void Start();
    }
}
