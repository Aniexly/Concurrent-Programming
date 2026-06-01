using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    public class SimulationClock : ISimulationClock
    {
        private readonly TimeSpan _updateInterval;
        private PeriodicTimer? _timer;
        private readonly CancellationToken _cancellationToken;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Barrier _ballsMovementBarrier;

        public TimeSpan Elapsed
        {
            get;
            private set
            {
                field = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action? OnTick;

        public SimulationClock(TimeSpan updateInterval, CancellationToken cancellationToken, Barrier ballsMovementBarrier)
        {
            _updateInterval = updateInterval;
            _cancellationToken = cancellationToken;
            _ballsMovementBarrier = ballsMovementBarrier;
        }

        public void Start()
        {
            _timer = new PeriodicTimer(_updateInterval);
            StartStopwatch();
            UpdateOnInterval();
        }

        private void StartStopwatch()
        {
            Elapsed = TimeSpan.Zero;
            _stopwatch.Restart();
        }

        private async Task UpdateOnInterval()
        {
            try
            {
                while (_timer != null && await _timer.WaitForNextTickAsync(_cancellationToken))
                {
                    Tick();

                    if (_ballsMovementBarrier != null)
                    {
                        try
                        {
                            if (_ballsMovementBarrier.ParticipantsRemaining == 1)
                            {
                                _ballsMovementBarrier.SignalAndWait(_cancellationToken);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _timer?.Dispose();
                _timer = null;
                _stopwatch.Stop();
            }
        }

        private void Tick()
        {
            Elapsed = _stopwatch.Elapsed;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
