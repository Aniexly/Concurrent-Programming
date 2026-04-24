using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;
using Model;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IDataAPI dataAPI = new DataAPI();
        public IBoardModel Board { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            Board = new BoardModel(dataAPI.CreateBoard());
        }
    }
}
