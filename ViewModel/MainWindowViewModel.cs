using Data;
using Logic;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ILogicAPI logicAPI = new LogicAPI();
        public IBoardModel BoardModel { get; set; }
        public ObservableCollection<IBallModel> BallModels { get; } = new ObservableCollection<IBallModel>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            int ballCount = 5;
            logicAPI.Start(ballCount, StartCallback);
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
    }
}
