namespace CheckersGame
{
    public struct Position
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}

