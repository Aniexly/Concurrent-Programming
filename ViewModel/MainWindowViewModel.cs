using Data;
using Logic;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ILogicApi _logicApi = new LogicApi();
        private readonly object _startSync = new object();

        public IBoardModel BoardModel
        {
            get;
            private set
            {
                field = value;
                OnPropertyChanged();
            }
        }
        public int BallsCount
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged();
                StartCommand.OnCanExecuteChanged();
            }
        }
        public ObservableCollection<IBallModel> BallModels { get; } = new ObservableCollection<IBallModel>();
        public Command StartCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            StartCommand = new Command(_ => Start(), _ => IsBallsCountValid());
        }

        private bool IsBallsCountValid()
        {
            return BallsCount > 0;
        }

        private async Task Start()
        {
            if (Monitor.TryEnter(_startSync))
            {
                try
                {
                    CleanSetup();
                    _logicApi.Stop();
                    await _logicApi.Start(BallsCount, StartCallback);
                }
                finally
                {
                    Monitor.Exit(_startSync);
                }
            }
        }

        private void CleanSetup()
        {
            BallModels.Clear();
        }

        private void StartCallback(IBoard board, List<IBall> balls)
        {
            SetupBoardModel(board);
            foreach (IBall ball in balls)
            {
                AddBallModel(ball);
            }
        }

        private void SetupBoardModel(IBoard board)
        {
            BoardModel = new BoardModel(board);
        }

        private void AddBallModel(IBall ball)
        {
            BallModels.Add(new BallModel(ball));
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
