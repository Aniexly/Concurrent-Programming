using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic;

namespace Model
{
    public class SimulationClockModel : ISimulationClockModel
    {
        private readonly ISimulationClock _simulationClock;
        public string Elapsed => _simulationClock.Elapsed.ToString(@"hh\:mm\:ss");

        public SimulationClockModel(ISimulationClock simulationClock)
        {
            _simulationClock = simulationClock;
            SubscribeToSimulationClockEvents();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void SubscribeToSimulationClockEvents()
        {
            _simulationClock.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(_simulationClock.Elapsed))
                {
                    OnPropertyChanged(nameof(Elapsed));
                }
            };
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
