namespace CheckersGame
{
    public interface IBoard
    {
        int Size { get; }
        Piece?[,] Grid { get; }
    }
}


