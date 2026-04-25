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

        public BoardModel(IBoard board)
        {
            this.board = board;
        }
    }
}
