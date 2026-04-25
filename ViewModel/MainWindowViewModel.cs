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
        private ILogicAPI logicAPI = new LogicAPI();

        private IBoardModel boardModel;
        public IBoardModel BoardModel
        {
            get => boardModel;
            private set
            {
                boardModel = value;
                OnPropertyChanged();
            }
        }
        private int ballsCount;
        public int BallsCount
        {
            get => ballsCount;
            set
            {
                ballsCount = value;
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
            if (BallsCount == 0)
            {
                return false;
            }
            return true;
        }

        private void Start()
        {
            CleanSetup();
            logicAPI.Start(BallsCount, StartCallback);
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
