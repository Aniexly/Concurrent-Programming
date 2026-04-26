using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public interface ILogicApi
    {
        public void Start(int ballsCount, Action<IBoard, List<IBall>> callback);

        public void MoveBalls(IBoard board);
    }
}
