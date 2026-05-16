using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IDataApi
    {
        public IBoard CreateBoard(int width, int height);

        public IBall CreateBall(IBoard board);
    }
}
