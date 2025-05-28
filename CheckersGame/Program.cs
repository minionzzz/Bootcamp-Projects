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
    private IBoard _board;
    public Display(IBoard board)
    {
        _board = board;
    }

    public void DrawBoard()
    {
    Console.Write("   ");
    for (int col = 0; col < _board.Size; col++)
    {
        Console.Write($"{col} ");
    }
    Console.WriteLine();

    for (int row = 0; row < _board.Size; row++)
    {
        Console.Write($"{row}  ");
        for (int col = 0; col < _board.Size; col++)
        {
            var piece = _board.Grid[row, col];
            char symbol = '.';

            if (piece != null)
            {
                if (piece.Type == PieceType.King)
                {
                    symbol = piece.Color == PieceColor.Red ? 'R' : 'B'; // Uppercase = King
                }
                else
                {
                    symbol = piece.Color == PieceColor.Red ? 'r' : 'b'; // Lowercase = Normal
                }
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
    private  IBoard _board;
    private IPlayer _currentPlayer;
    private  List<IPlayer> _players;
    private Position _lastCapturePosition;
    public bool IsGameOver { get; private set; }
    public bool HasPendingCapture { get; set;}  


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
         //kode debug untuk verifikasi
        int redCount = 0, blackCount = 0;
        foreach (var piece in _board.Grid)
        {
            if (piece != null)
            {
                if (piece.Color == PieceColor.Red) redCount++;
                if (piece.Color == PieceColor.Black) blackCount++;
            }
        }
        Console.WriteLine($"DEBUG: Red={redCount}, Black={blackCount}");
    }

    public List<Position> GetCapturedPieces(Position from, Position to)
    {
        var captured = new List<Position>();

        int dRow = to.Row - from.Row;
        int dCol = to.Col - from.Col;

        // Harus melompat dua kotak secara diagonal
        if (Math.Abs(dRow) == 2 && Math.Abs(dCol) == 2)
        {
            int midRow = from.Row + dRow / 2;
            int midCol = from.Col + dCol / 2;
            var middlePos = new Position(midRow, midCol);
            var middlePiece = GetPiece(middlePos);
            var movingPiece = GetPiece(from);

            if (middlePiece != null && middlePiece.Color != movingPiece.Color)
            {
                captured.Add(middlePos);
            }
        }

        return captured;
    }


    public bool MakeMove(Position from, Position to)
    {
        if (!IsValidPosition(from) || !IsValidPosition(to)) return false;

        var piece = GetPiece(from);
        if (piece == null || piece.Color != _currentPlayer.Color) return false;

        var captured = GetCapturedPieces(from, to);
        if (captured.Count == 0)
        {
            // Tidak melakukan capture
            if (HasPendingCapture) return false; // pemain wajib lanjut jump
            if (!IsValidMove(piece, from, to)) return false;

            // Pindah biasa
            SetPiece(to, piece);
            RemovePiece(from);
            CheckForPromotion(piece, to);
            EndTurn();
            return true;
        }
        else
        {
            // Capture move
            foreach (var pos in captured)
            {
                RemovePiece(pos);
            }

            RemovePiece(from);
            SetPiece(to, piece);
            CheckForPromotion(piece, to);

            _lastCapturePosition = to;

            // Cek apakah masih bisa capture dari posisi baru
            if (CanCaptureAgain(to))
            {
                HasPendingCapture = true;
                return true; // pemain harus lanjut
            }
            else
            {
                HasPendingCapture = false;
                EndTurn();
                return true;
            }
        }
    }

    public bool CanCaptureAgain(Position pos)
    {
        var piece = GetPiece(pos);
        if (piece == null) return false;

        int[] directions = { -1, 1 };
        foreach (int dr in directions)
        {
            foreach (int dc in directions)
            {
                int midRow = pos.Row + dr;
                int midCol = pos.Col + dc;
                int destRow = pos.Row + 2 * dr;
                int destCol = pos.Col + 2 * dc;

                var mid = new Position(midRow, midCol);
                var dest = new Position(destRow, destCol);

                if (!IsValidPosition(mid) || !IsValidPosition(dest)) continue;

                var middlePiece = GetPiece(mid);
                var destinationPiece = GetPiece(dest);

                if (middlePiece != null &&
                    middlePiece.Color != piece.Color &&
                    destinationPiece == null)
                {
                    // valid jump
                    if (piece.Type == PieceType.King ||
                        (piece.Color == PieceColor.Red && dr == -1) ||
                        (piece.Color == PieceColor.Black && dr == 1))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void EndTurn()
    {
        _currentPlayer = _players.First(p => p != _currentPlayer);
        PlayerTurnChanged?.Invoke(_currentPlayer);
        HasPendingCapture = false;
        CheckGameOver();
    }

    public bool HasMoreCaptures(Piece piece, Position pos)
    {
        int[] directions = { -1, 1 };
        foreach (int dRow in directions)
        {
            foreach (int dCol in directions)
            {
                int newRow = pos.Row + dRow * 2;
                int newCol = pos.Col + dCol * 2;
                var newPos = new Position(newRow, newCol);
                if (!IsValidPosition(newPos)) continue;

                int midRow = pos.Row + dRow;
                int midCol = pos.Col + dCol;
                var midPiece = GetPiece(new Position(midRow, midCol));
                if (GetPiece(newPos) == null &&
                    midPiece != null && midPiece.Color != piece.Color)
                {
                    if (piece.Type == PieceType.King ||
                    (piece.Color == PieceColor.Red && dRow == -1) ||
                    (piece.Color == PieceColor.Black && dRow == 1))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
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

        int rowDiff = to.Row - from.Row;
        int colDiff = to.Col - from.Col;
        int absRow = Math.Abs(rowDiff);
        int absCol = Math.Abs(colDiff);

        // Normal move
        if (absRow == 1 && absCol == 1)
        {
            if (piece.Type == PieceType.Normal)
            {
                int dir = piece.Color == PieceColor.Red ? -1 : 1;
                return rowDiff == dir;
            }
            return true; // King can move both directions
        }

        // Capture move
        if (absRow == 2 && absCol == 2)
        {
            int midRow = from.Row + rowDiff / 2;
            int midCol = from.Col + colDiff / 2;
            var middlePiece = GetPiece(new Position(midRow, midCol));
            return middlePiece != null && middlePiece.Color != piece.Color;
        }

        return false;
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

            if (hasRed && hasBlack) break; // cukup cek sampai keduanya ditemukan
        }

        if (!hasRed || !hasBlack)
        {
            IsGameOver = true;
            var winner = hasRed ? _players.First(p => p.Color == PieceColor.Red) : _players.First(p => p.Color == PieceColor.Black);
            GameEnded?.Invoke(winner);
        }
    }


    public IPlayer GetCurrentPlayer() => _currentPlayer;

    public bool IsValidPosition(Position pos)
    {
        return pos.Row >= 0 && pos.Row < _board.Size && pos.Col >= 0 && pos.Col < _board.Size;
    }

    public Piece GetPiece(Position pos) => _board.Grid?[pos.Row, pos.Col];

    public void SetPiece(Position pos, Piece piece) => _board.Grid[pos.Row, pos.Col] = piece;

    public void RemovePiece(Position pos) => _board.Grid[pos.Row, pos.Col] = null;

    public IBoard GetBoard() => _board;

    private void SwitchTurn()
    {
        _currentPlayer = _players.First(p => p != _currentPlayer);
        PlayerTurnChanged?.Invoke(_currentPlayer);
    }
}

class Program{
    static void Main(string[] args)
    {
        // Inisialisasi pemain dan papan
        IPlayer player1 = new Player("Alice", PieceColor.Red);
        IPlayer player2 = new Player("Bob", PieceColor.Black);
        IBoard board = new Board(8);
        var controller = new GameController(player1, player2, board);
        var display = new Display(board);

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
                    if (controller.HasPendingCapture)
                    {
                        Console.WriteLine("You must continue your jump! Try next capture move.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Try again.");
                    }
                }
            } while (!moveSucceeded && controller.HasPendingCapture);
        }
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
    

    