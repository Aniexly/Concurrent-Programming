using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public interface ILogicAPI
    {
        public void Start(int ballCount, Action<IBoard, List<IBall>> callback);

        public void MoveBalls(IBoard board);
    }
}
