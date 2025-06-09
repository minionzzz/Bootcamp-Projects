namespace CheckersGame
{
    public class Board : IBoard
    {
        public int Size { get; }
        public Piece[,] Grid { get; }
        public Board(int size)
        {
            Size = size;
            Grid = new Piece[size, size];
        }
    }
    
}

