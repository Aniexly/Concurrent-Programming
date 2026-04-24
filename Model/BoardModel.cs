using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;

namespace Model
{
    public class BoardModel : IBoardModel
    {
        private IBoard board;
        public int Width => board.Width;
        public int Height => board.Height;

        public event PropertyChangedEventHandler? PropertyChanged;

        public BoardModel(IBoard board)
        {
            this.board = board;
            OnBoardChanged();
        }

        private void OnBoardChanged()
        {
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Height));
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
