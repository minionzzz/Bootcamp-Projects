using System;
using System.Collections.Generic;
using System.Linq;

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
    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }
}

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

public class Player : IPlayer
{
    public string Name { get; }
    public PieceColor Color { get; }
    public Player(string name, PieceColor color)
    {
        Name = name;
        Color = color;
    }
}

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

public class Display
{
    private GameController _controller;
    public Display(GameController controller)
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

public class GameController
{
    private IBoard _board;
    private IPlayer _currentPlayer;
    private List<IPlayer> _players;
    private Position _lastCapturePosition;
    public bool IsGameOver { get; set; }

    public Action<IPlayer> PlayerTurnChanged;
    public Action<IPlayer> GameEnded;
    public Action<Position> PiecePromoted;

    public GameController(IPlayer p1, IPlayer p2, IBoard board)
    {
        _board = board;
        _players = new List<IPlayer> { p1, p2 };
        _currentPlayer = p1;
        _lastCapturePosition = new Position(-1, -1); //Inisialisasi invalid position
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
                    _board.Grid[row, col] = new Piece(PieceColor.Black);
                else if (row >= size - 3)
                    _board.Grid[row, col] = new Piece(PieceColor.Red);
            }
        }
    }

    public bool MakeMove(Position from, Position to)
    {
        if (!IsValidPosition(from) || !IsValidPosition(to)) return false;

        var piece = GetPiece(from);
        if (piece == null || piece.Color != _currentPlayer.Color) return false;

        var captured = GetCapturedPieces(from, to);
        if (captured.Count == 0)
        {
            if (!IsValidMove(piece, from, to)) return false;
            MovePiece(from, to, piece);
            _lastCapturePosition = new Position(-1, -1);
            EndTurn();
            return true;
        }
        else
        {
            if (!IsValidMove(piece, from, to)) return false;

            MakeCaptureMove(from, to, captured);

            if (CanCaptureAgain(to))
            {
                return true;
            }
            else
            {
                _lastCapturePosition = new Position(-1, -1);
                EndTurn();
                return true;
            }
        }
    }

    public void MakeCaptureMove(Position from, Position to, List<Position> captured)
    {
        foreach (var pos in captured)
        {
            RemovePiece(pos);
        }
        var piece = GetPiece(from);

        MovePiece(from, to, piece);

        _lastCapturePosition = to;

    }

    public void MovePiece(Position from, Position to, Piece piece)
    {
        RemovePiece(from);
        SetPiece(to, piece);
        CheckForPromotion(piece, to);
    }

    public List<Position> GetCapturedPieces(Position from, Position to)
    {
        var captured = new List<Position>();
        int dRow = to.Row - from.Row, dCol = to.Col - from.Col;
        if (Math.Abs(dRow) == 2 && Math.Abs(dCol) == 2)
        {
            int midRow = from.Row + dRow / 2;
            int midCol = from.Col + dCol / 2;
            var middlePiece = GetPiece(new Position(midRow, midCol));
            var movingPiece = GetPiece(from);
            if (middlePiece != null && movingPiece != null && middlePiece.Color != movingPiece.Color)
                captured.Add(new Position(midRow, midCol));
        }
        return captured;
    }

    public bool CanCaptureAgain(Position pos)
    {
        var piece = GetPiece(pos);
        if (piece == null) return false;
        int[] dirs = { -1, 1 };
        foreach (int dr in dirs)
        {
            foreach (int dc in dirs)
            {
                int midRow = pos.Row + dr;
                int midCol = pos.Col + dc;
                int endRow = pos.Row + 2 * dr;
                int endCol = pos.Col + 2 * dc;
                if (!IsValidPosition(new Position(midRow, midCol)) ||
                    !IsValidPosition(new Position(endRow, endCol))) continue;
                var midPiece = GetPiece(new Position(midRow, midCol));
                var destPiece = GetPiece(new Position(endRow, endCol));
                if (midPiece != null && midPiece.Color != piece.Color && destPiece == null)
                {
                    if (piece.Type == PieceType.King ||
                        (piece.Color == PieceColor.Red && dr == -1) ||
                        (piece.Color == PieceColor.Black && dr == 1))
                        return true;
                }
            }
        }
        return false;
    }

    public void EndTurn()
    {
        CheckGameOver();
        if (!IsGameOver)
        {
            _currentPlayer = _players.First(p => p != _currentPlayer);
            PlayerTurnChanged?.Invoke(_currentPlayer);
        }
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
        foreach (var p in _board.Grid)
        {
            if (p == null) continue;
            if (p.Color == PieceColor.Red) hasRed = true;
            if (p.Color == PieceColor.Black) hasBlack = true;
        }
        if (!hasRed || !hasBlack)
        {
            IsGameOver = true;
            GameEnded?.Invoke(_currentPlayer);
        }
    }

    public bool IsValidMove(Piece piece, Position from, Position to)
    {
        if (piece == null || !IsValidPosition(to) || GetPiece(to) != null) return false;
        int dRow = to.Row - from.Row, dCol = to.Col - from.Col;
        if (Math.Abs(dRow) == 1 && Math.Abs(dCol) == 1)
        {
            if (piece.Type == PieceType.King) return true;
            return (piece.Color == PieceColor.Red && dRow == -1) ||
                   (piece.Color == PieceColor.Black && dRow == 1);
        }
        if (Math.Abs(dRow) == 2 && Math.Abs(dCol) == 2)
        {
            int midRow = from.Row + dRow / 2;
            int midCol = from.Col + dCol / 2;
            var midPiece = GetPiece(new Position(midRow, midCol));
            return midPiece != null && midPiece.Color != piece.Color;
        }
        return false;
    }

    public IPlayer GetCurrentPlayer() => _currentPlayer;
    public bool IsValidPosition(Position pos) => pos.Row >= 0 && pos.Row < _board.Size && pos.Col >= 0 && pos.Col < _board.Size;
    public Piece? GetPiece(Position pos) => _board.Grid[pos.Row, pos.Col];
    public void SetPiece(Position pos, Piece piece) => _board.Grid[pos.Row, pos.Col] = piece;
    public void RemovePiece(Position pos) => _board.Grid[pos.Row, pos.Col] = null;
    public IBoard GetBoard() => _board;
}

class Program
{
    static void Main(string[] args)
    {
        // Inisialisasi pemain dan papan
        IPlayer player1 = new Player("ADIT", PieceColor.Red);
        IPlayer player2 = new Player("DENIS", PieceColor.Black);
        IBoard board = new Board(8);
        var controller = new GameController(player1, player2, board);
        var display = new Display(controller);

        //Event handler
        controller.PlayerTurnChanged += display.ShowPlayerTurn;
        controller.GameEnded += display.ShowWinner;
        controller.PiecePromoted += display.ShowPromotion;

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