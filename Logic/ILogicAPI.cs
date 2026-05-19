using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public interface ILogicApi
    {
        public Task Start(int ballsCount, Action<IBoard, List<IBall>> callback);

        public void MoveBallsOnce(IBoard board);

        public void Stop();
    }
}
