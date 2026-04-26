using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;

namespace Model
{
    public class BoardModel : IBoardModel
    {
        private IBoard board;
        private double scale = 2.0;
        public double Width => board.Width * scale;
        public double Height => board.Height * scale;

        public BoardModel(IBoard board)
        {
            this.board = board;
        }
    }
}
