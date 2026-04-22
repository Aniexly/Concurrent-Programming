namespace Data
{
    public class Board : IBoard
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<IBall> Balls { get; } = new List<IBall>();

        public Board(int width, int height) => (Width, Height) = (width, height);
    }
}
