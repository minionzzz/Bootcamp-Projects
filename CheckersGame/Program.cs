namespace CheckersGame;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Inisialisasi pemain dan papan
        var players = new List<IPlayer> 
        { 
            new Player("ADIT", PieceColor.Red),
            new Player("DENIS", PieceColor.Black) 
        };
        IBoard board = new Board(8);
        var controller = new GameController(players, board);
        var display = new Display(controller);

        //Event handler
        controller.playerTurnChanged += display.ShowPlayerTurn;
        controller.gameEnded += display.ShowWinner;
        controller.piecePromoted += display.ShowPromotion;

        // Mulai permainan
        controller.StartGame();

        // Game loop
        while (true)
        {
            display.DrawBoard();

            if (controller.IsGameOver)
            {
                Console.WriteLine("Game over. Press any key to exit.");
                break;
            }

            var currentPlayer = controller.GetCurrentPlayer();
            Console.WriteLine($"{currentPlayer.Name} ({currentPlayer.Color}) - Make your move!");

            // Loop input jika harus lanjut capture
            bool moveSucceeded = false;
            do
            {
                Position from = ReadPosition("From");
                Position to = ReadPosition("To");

                moveSucceeded = controller.MakeMove(from, to);

                if (!moveSucceeded)
                {
                    Console.WriteLine("Invalid move. Try again.");
                }
            } while (!moveSucceeded);
        }
        Console.ReadKey();
    }
    static Position ReadPosition(string label)
    {
        while (true)
        {
            Console.Write($"{label} (row col): ");
            string input = Console.ReadLine();
            var parts = input?.Split();

            if (parts?.Length == 2 &&
                int.TryParse(parts[0], out int row) &&
                int.TryParse(parts[1], out int col))
            {
                return new Position(row, col);
            }

            Console.WriteLine("Invalid input. Please enter two numbers separated by space.");
        }
    }
}