namespace CheckersGame
{
    public class Display
    {
        private IGameController _controller;
        public Display(IGameController controller)
        {
            _controller = controller;
        }

        public void DrawBoard()
        {
            var board = _controller.GetBoard();
            Console.Write("   ");
            for (int col = 0; col < board.Size; col++)
            {
                Console.Write($"{col} ");
            }
            Console.WriteLine();

            for (int row = 0; row < board.Size; row++)
            {
                Console.Write($"{row}  ");
                for (int col = 0; col < board.Size; col++)
                {
                    var piece = board.Grid[row, col];
                    char symbol = '.';

                    if (piece != null)
                    {
                        symbol = piece.Type == PieceType.King
                                ? (piece.Color == PieceColor.Red ? 'R' : 'B')
                                : (piece.Color == PieceColor.Red ? 'r' : 'b');
                    }

                    Console.Write($"{symbol} ");
                }
                Console.WriteLine();
            }
        }

        public void ShowPlayerTurn(IPlayer player)
        {
            Console.WriteLine($"It's {player.Name}'s turn.");
        }

        public void ShowWinner(IPlayer player)
        {
            Console.WriteLine($"{player.Name} wins the game!");
        }

        public void ShowPromotion(Position pos)
        {
            Console.WriteLine($"Piece at ({pos.Row}, {pos.Col}) has been promoted to King!");
        }
    }
}


