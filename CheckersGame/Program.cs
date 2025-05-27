using System;
using System.Collections.Generic;

public enum PieceColor
{
    Red,
    Black
}

public enum PieceType
{
    Normal,
    King
}
public interface IBoard
{
    int Size { get; }
    Piece?[,] Grid { get; }
}

public interface IPlayer
{
    string Name { get; }
    PieceColor Color { get; }
}

public struct Position
{
    public int Row { get; set; }
    public int Col { get; set; }
    Position(int row, int col)
    {
        Row = row;
        Col = col;
    }
}

public class Piece 
{
    public PieceColor Color { get;  set; }
    public PieceType Type { get; set; }

    public Piece(PieceColor color, PieceType type)
    {
        Color = color;
        Type = type;
    }
}

public class Player : IPlayer
{
    public string Name { get; set; }
    public PieceColor Color { get; set; }
    Player(string name, PieceColor color)
    {
        Name = name;
        Color = color;
    }
}

public class Board : IBoard
{
    public int Size { get; set; }
    public Piece[,] Grid { get; set; }
    Board(int size)
    {
        Size = size;
        Grid = new Piece[size, size];
    }
}

public class Display
{
    public IBoard Board;
    Display(IBoard board)
    {
        Board = board;
    }

    public void DrawBoard()
    {
        for (int row = 0; row < _board.Size; row++)
        {
            for (int col = 0; col < _board.Size; col++)
            {
                var piece = _board.Grid[row, col];
                Console.Write(piece == null ? "." : piece.Color.ToString()[0]);
                Console.Write(" ");
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

public class GameController
{
    private  IBoard _board;
    private IPlayer _currentPlayer;
    private  List<IPlayer> _players;
    public bool IsGameOver { get; private set; }

    public Action<IPlayer> PlayerTurnChanged;
    public Action<IPlayer> GameEnded;
    public Action<Position> PiecePromoted;

    public GameController(IPlayer player1, IPlayer player2, IBoard board)
    {
        _board = board;
        _players = new List<IPlayer> { player1, player2 };
        _currentPlayer = player1;
    }

    public void StartGame()
    {
        InitializeBoard();
        PlayerTurnChanged?.Invoke(_currentPlayer);
    }

    public void InitializeBoard()
    {
        int size = _board.Size;
        for (int row = 0; row < size; row++)
        {
            for (int col = (row + 1) % 2; col < size; col += 2)
            {
                if (row < 3)
                    _board.Grid[row, col] = new Piece(PieceColor.Black, PieceType.Normal);
                else if (row >= size - 3)
                    _board.Grid[row, col] = new Piece(PieceColor.Red, PieceType.Normal);
            }
        }
    }

    public bool MakeMove(Position from, Position to)
    {
        var piece = GetPiece(from);
        if (!IsValidMove(piece, from, to)) return false;

        SetPiece(to, piece);
        RemovePiece(from);

        CheckForPromotion(piece, to);
        CheckGameOver();

        if (!IsGameOver)
        {
            SwitchTurn();
        }

        return true;
    }

    public void MakeCaptureMove(Position from, Position to, List<Position> captured)
    {
        if (!MakeMove(from, to)) return;

        foreach (var cap in captured)
            RemovePiece(cap);
    }

    public bool IsValidMove(Piece piece, Position from, Position to)
    {
        if (piece == null || piece.Color != _currentPlayer.Color) return false;
        if (!IsValidPosition(to) || GetPiece(to) != null) return false;

        int dir = piece.Color == PieceColor.Red ? -1 : 1;
        int rowDiff = to.Row - from.Row;
        int colDiff = Math.Abs(to.Col - from.Col);

        if (piece.Type == PieceType.Normal)
        {
            return rowDiff == dir && colDiff == 1;
        }

        return Math.Abs(rowDiff) == 1 && colDiff == 1;
    }

    public void CheckForPromotion(Piece piece, Position pos)
    {
        if (piece.Type == PieceType.King) return;

        if ((piece.Color == PieceColor.Red && pos.Row == 0) ||
            (piece.Color == PieceColor.Black && pos.Row == _board.Size - 1))
        {
            piece.Type = PieceType.King;
            PiecePromoted?.Invoke(pos);
        }
    }

    public void CheckGameOver()
    {
        bool hasRed = false, hasBlack = false;
        foreach (var piece in _board.Grid)
        {
            if (piece == null) continue;
            if (piece.Color == PieceColor.Red) hasRed = true;
            if (piece.Color == PieceColor.Black) hasBlack = true;
        }

        if (!hasRed || !hasBlack)
        {
            IsGameOver = true;
            GameEnded?.Invoke(_currentPlayer);
        }
    }

    public IPlayer GetCurrentPlayer() => _currentPlayer;

    public bool IsValidPosition(Position pos)
    {
        return pos.Row >= 0 && pos.Row < _board.Size && pos.Col >= 0 && pos.Col < _board.Size;
    }

    public Piece GetPiece(Position pos) => _board.Grid[pos.Row, pos.Col];

    public void SetPiece(Position pos, Piece piece) => _board.Grid[pos.Row, pos.Col] = piece;

    public void RemovePiece(Position pos) => _board.Grid[pos.Row, pos.Col] = null;

    public IBoard GetBoard() => _board;

    private void SwitchTurn()
    {
        _currentPlayer = _players.First(p => p != _currentPlayer);
        PlayerTurnChanged?.Invoke(_currentPlayer);
    }
}
class Program
{
    static void Main(string[] args)
    {
        // Inisialisasi pemain dan papan
        IPlayer player1 = new Player("Alice", PieceColor.Red);
        IPlayer player2 = new Player("Bob", PieceColor.Black);
        IBoard board = new Board(8);
        var controller = new GameController(player1, player2, board);
        var display = new Display(board);

        // Event handler
        controller.PlayerTurnChanged += display.ShowPlayerTurn;
        controller.GameEnded += display.ShowWinner;
        controller.PiecePromoted += display.ShowPromotion;

        controller.StartGame();

        while (!controller.IsGameOver)
        {
            display.DrawBoard();

            var currentPlayer = controller.GetCurrentPlayer();
            Console.WriteLine($"{currentPlayer.Name} ({currentPlayer.Color}) - Make your move!");

            Position from = ReadPosition("From");
            Position to = ReadPosition("To");

            if (!controller.MakeMove(from, to))
            {
                Console.WriteLine("Invalid move. Try again.");
                continue;
            }

            Console.Clear();
        }

        display.DrawBoard();
        Console.WriteLine("Game over. Press any key to exit.");
        Console.ReadKey();
    }

    static Position ReadPosition(string label)
    {
        while (true)
        {
            Console.Write($"{label} (row col): ");
            string input = Console.ReadLine();
            var parts = input.Split();
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int row) &&
                int.TryParse(parts[1], out int col))
            {
                return new Position(row, col);
            }
            Console.WriteLine("Invalid input. Please enter two numbers separated by space.");
        }
    }
}
