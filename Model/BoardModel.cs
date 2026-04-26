using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;

namespace Model
{
    public class BoardModel : IBoardModel
    {
        private readonly IBoard _board;
        private const double Scale = 2.0;
        public double Width => _board.Width * Scale;
        public double Height => _board.Height * Scale;

        public BoardModel(IBoard board)
        {
            _board = board;
        }
    }
}
