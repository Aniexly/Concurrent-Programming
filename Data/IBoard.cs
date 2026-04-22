using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IBoard
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<IBall> Balls { get; }
    }
}
