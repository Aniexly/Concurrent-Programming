using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IDataAPI
    {
        public IBoard CreateBoard();

        public IBall CreateBall(IBoard board);
    }
}
