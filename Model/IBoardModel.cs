using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Model
{
    public interface IBoardModel
    {
        public double Width { get; }
        public double Height { get; }
    }
}
