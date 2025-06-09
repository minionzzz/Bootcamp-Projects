namespace CheckersGame
{
    public class Piece
    {
        public PieceColor Color { get; }
        public PieceType Type { get; set; }

        public Piece(PieceColor color, PieceType type = PieceType.Normal)
        {
            Color = color;
            Type = type;
        }
    }
}


