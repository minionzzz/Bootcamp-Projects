using System;
using System.Collections.Generic;

public enum PieceColor
{
    Black,
    White
}

public interface IPlayer
{
    string Name { get; }
    PieceColor Color { get; }
}

public interface IBoard
{
    int Size { get; }
    IPiece?[,] Grid { get; }
    void Initialize();
    bool IsInBounds(Position pos);
}

public interface IPiece
{
    PieceColor Color { get; }
}

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

public class Piece : IPiece
{
    public PieceColor Color { get; private set; }

    public Piece(PieceColor color)
    {
        Color = color;
    }
}

public class Player : IPlayer
{
    public string Name { get; private set; }
    public PieceColor Color { get; private set; }

    public Player(string name, PieceColor color)
    {
        Name = name;
        Color = color;
    }
}

public class Board : IBoard
{
    public int Size { get; private set; }
    public IPiece?[,] Grid { get; private set; }

    public Board(int size)
    {
        Size = size;
        Grid = new IPiece?[size, size];
    }

    public void Initialize()
    {
        Grid = new IPiece?[Size, Size];
        int mid = Size / 2;
        Grid[mid - 1, mid - 1] = new Piece(PieceColor.White);
        Grid[mid, mid] = new Piece(PieceColor.White);
        Grid[mid - 1, mid] = new Piece(PieceColor.Black);
        Grid[mid, mid - 1] = new Piece(PieceColor.Black);
    }

    public bool IsInBounds(Position pos) =>
        pos.Row >= 0 && pos.Row < Size && pos.Col >= 0 && pos.Col < Size;
}

public class GameController
{
    public IBoard Board { get; private set; }
    public List<IPlayer> Players { get; private set; }
    public IPlayer CurrentPlayer => Players[currentPlayerIndex];

    private int currentPlayerIndex = 0;

    public Action<string>? OnMoveMade;

    private List<Position> directions = new()
    {
        new Position(-1, -1), new Position(-1, 0), new Position(-1, 1),
        new Position(0, -1),                    new Position(0, 1),
        new Position(1, -1),  new Position(1, 0), new Position(1, 1)
    };

    public GameController(List<IPlayer> players, IBoard board)
    {
        Players = players;
        Board = board;
    }

    public void StartGame()
    {
        currentPlayerIndex = 0;
        Board.Initialize();
    }

    public bool GameOver()
    {
        foreach (var player in Players)
        {
            if (GetValidMoves(player.Color).Count > 0)
                return false;
        }
        return true;
    }

    public void MakeMove(Position pos)
    {
        if (!IsValidMove(pos, CurrentPlayer.Color))
        {
            OnMoveMade?.Invoke("Invalid move.");
            return;
        }

        PlacePiece(pos, CurrentPlayer.Color);
        FlipPieces(pos, CurrentPlayer.Color);
        OnMoveMade?.Invoke($"{CurrentPlayer.Name} placed at ({pos.Row},{pos.Col})");
        SwitchPlayer();
    }

    public bool IsValidMove(Position pos, PieceColor color)
    {
        if (!Board.IsInBounds(pos)) return false;
        if (Board.Grid[pos.Row, pos.Col] != null) return false;

        foreach (var dir in directions)
        {
            int r = pos.Row + dir.Row;
            int c = pos.Col + dir.Col;
            bool hasOpponent = false;

            while (Board.IsInBounds(new Position(r, c)) &&
                   Board.Grid[r, c] != null &&
                   Board.Grid[r, c].Color != color)
            {
                hasOpponent = true;
                r += dir.Row;
                c += dir.Col;
            }

            if (hasOpponent &&
                Board.IsInBounds(new Position(r, c)) &&
                Board.Grid[r, c]?.Color == color)
            {
                return true;
            }
        }

        return false;
    }

    public List<Position> GetValidMoves(PieceColor color)
    {
        var moves = new List<Position>();
        for (int r = 0; r < Board.Size; r++)
        {
            for (int c = 0; c < Board.Size; c++)
            {
                var pos = new Position(r, c);
                if (IsValidMove(pos, color))
                    moves.Add(pos);
            }
        }
        return moves;
    }

    public void SkipTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
    }

    private void PlacePiece(Position pos, PieceColor color)
    {
        Board.Grid[pos.Row, pos.Col] = new Piece(color);
    }

    private void FlipPieces(Position pos, PieceColor color)
    {
        foreach (var dir in directions)
        {
            var toFlip = new List<Position>();
            int r = pos.Row + dir.Row;
            int c = pos.Col + dir.Col;

            while (Board.IsInBounds(new Position(r, c)) &&
                   Board.Grid[r, c] != null &&
                   Board.Grid[r, c].Color != color)
            {
                toFlip.Add(new Position(r, c));
                r += dir.Row;
                c += dir.Col;
            }

            if (Board.IsInBounds(new Position(r, c)) &&
                Board.Grid[r, c]?.Color == color)
            {
                foreach (var p in toFlip)
                    Board.Grid[p.Row, p.Col] = new Piece(color);
            }
        }
    }

    private void SwitchPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
    }

    public void CountPieces(out int black, out int white)
    {
        black = 0;
        white = 0;
        foreach (var piece in Board.Grid)
        {
            if (piece?.Color == PieceColor.Black) black++;
            if (piece?.Color == PieceColor.White) white++;
        }
    }
}
class Program
{
    static void Main()
    {
        IPlayer player1 = new Player("Player 1", PieceColor.Black);
        IPlayer player2 = new Player("Player 2", PieceColor.White);
        IBoard board = new Board(8);

        var gameController = new GameController(new List<IPlayer> { player1, player2 }, board);
        gameController.OnMoveMade += Console.WriteLine;

        gameController.StartGame();

        while (!gameController.GameOver())
        {
            PrintBoard(board);
            var current = gameController.CurrentPlayer;
            Console.WriteLine($"{current.Name}'s turn ({current.Color}).");

            var validMoves = gameController.GetValidMoves(current.Color);
            if (validMoves.Count == 0)
            {
                Console.WriteLine("No valid moves. Turn skipped.");
                gameController.SkipTurn();
                continue;
            }

            Console.Write("Enter move (row col): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            var parts = input.Split();
            if (parts.Length != 2 || 
                !int.TryParse(parts[0], out int row) || 
                !int.TryParse(parts[1], out int col))
            {
                Console.WriteLine("Invalid input. Try again.");
                continue;
            }

            var pos = new Position(row, col);
            if (!gameController.IsValidMove(pos, current.Color))
            {
                Console.WriteLine("Invalid move. Try again.");
                continue;
            }

            gameController.MakeMove(pos);
        }

        Console.WriteLine("Game over!");
        var (black, white) = gameController.CountPieces();
        Console.WriteLine($"Final Score - Black: {black}, White: {white}");

        if (black > white) Console.WriteLine("Black wins!");
        else if (white > black) Console.WriteLine("White wins!");
        else Console.WriteLine("It's a tie!");
    }

    static void PrintBoard(IBoard board)
    {
        Console.Write("   ");
        for (int c = 0; c < board.Size; c++)
            Console.Write($"{c} ");
        Console.WriteLine();

        for (int r = 0; r < board.Size; r++)
        {
            Console.Write($"{r}  ");
            for (int c = 0; c < board.Size; c++)
            {
                var piece = board.Grid[r, c];
                if (piece == null)
                    Console.Write(". ");
                else if (piece.Color == PieceColor.Black)
                    Console.Write("B ");
                else
                    Console.Write("W ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

