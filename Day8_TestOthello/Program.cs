public class GameController
{
    private readonly Board board;
    private readonly Player[] players;
    private int currentPlayerIndex;

    public Action<string>? OnMoveMade;
    public Action<Player?>? OnGameOver;

    public GameController(Player player1, Player player2)
    {
        board = new Board();
        players = new[] { player1, player2 };
        currentPlayerIndex = 0;
    }

    public void StartGame()
    {
        while (!IsGameOver())
        {
            var currentPlayer = GetCurrentPlayer();
            var move = currentPlayer.MakeMove(board);

            if (board.IsValidMove(move, currentPlayer.Color))
            {
                board.PlacePiece(move, currentPlayer.Color);
                OnMoveMade?.Invoke($"{currentPlayer.Name} placed at ({move.Row}, {move.Col})");
                SwitchTurn();
            }
            else
            {
                OnMoveMade?.Invoke($"{currentPlayer.Name} attempted invalid move at ({move.Row}, {move.Col})");
            }
        }

        OnGameOver?.Invoke(GetWinner());
    }

    private void SwitchTurn() => currentPlayerIndex = 1 - currentPlayerIndex;
    private bool IsGameOver() => board.IsFull() || !players.Any(p => board.HasValidMove(p.Color));
    private Player GetCurrentPlayer() => players[currentPlayerIndex];

    private Player? GetWinner()
    {
        var (black, white) = board.CountPieces();
        if (black == white) return null;
        return black > white
            ? players.First(p => p.Color == PieceColor.Black)
            : players.First(p => p.Color == PieceColor.White);
    }
}

public class Board
{
    private readonly Piece?[,] grid;

    public Board()
    {
        grid = new Piece[8, 8];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        grid[3, 3] = new Piece(PieceColor.White);
        grid[3, 4] = new Piece(PieceColor.Black);
        grid[4, 3] = new Piece(PieceColor.Black);
        grid[4, 4] = new Piece(PieceColor.White);
    }

    public bool IsValidMove(Position pos, PieceColor color)
    {
        if (!IsInBounds(pos) || grid[pos.Row, pos.Col] != null) return false;

        foreach (var dir in Directions)
        {
            int r = pos.Row + dir.Row;
            int c = pos.Col + dir.Col;
            bool foundOpponent = false;

            while (IsInBounds(new Position(r, c)) && grid[r, c]?.Color != color && grid[r, c] != null)
            {
                foundOpponent = true;
                r += dir.Row;
                c += dir.Col;
            }

            if (foundOpponent && IsInBounds(new Position(r, c)) && grid[r, c]?.Color == color)
                return true;
        }

        return false;
    }

    public void PlacePiece(Position pos, PieceColor color)
    {
        grid[pos.Row, pos.Col] = new Piece(color);
        FlipPieces(pos, color);
    }

    public bool HasValidMove(PieceColor color)
    {
        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                if (IsValidMove(new Position(r, c), color)) return true;

        return false;
    }

    public bool IsFull()
    {
        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                if (grid[r, c] == null) return false;
        return true;
    }

    public (int black, int white) CountPieces()
    {
        int black = 0, white = 0;
        foreach (var piece in grid)
        {
            if (piece?.Color == PieceColor.Black) black++;
            if (piece?.Color == PieceColor.White) white++;
        }
        return (black, white);
    }

    private void FlipPieces(Position pos, PieceColor color)
    {
        foreach (var dir in Directions)
        {
            List<Position> toFlip = new();
            int r = pos.Row + dir.Row;
            int c = pos.Col + dir.Col;

            while (IsInBounds(new Position(r, c)) && grid[r, c]?.Color != color && grid[r, c] != null)
            {
                toFlip.Add(new Position(r, c));
                r += dir.Row;
                c += dir.Col;
            }

            if (IsInBounds(new Position(r, c)) && grid[r, c]?.Color == color)
                foreach (var p in toFlip)
                    grid[p.Row, p.Col] = new Piece(color);
        }
    }

    private static readonly List<Position> Directions = new()
    {
        new(-1, -1), new(-1, 0), new(-1, 1),
        new(0, -1),            new(0, 1),
        new(1, -1), new(1, 0), new(1, 1)
    };

    private bool IsInBounds(Position pos) => pos.Row >= 0 && pos.Row < 8 && pos.Col >= 0 && pos.Col < 8;
}

public class Player
{
    public string Name { get; }
    public PieceColor Color { get; }
    private readonly IMoveStrategy strategy;

    public Player(string name, PieceColor color, IMoveStrategy strategy)
    {
        Name = name;
        Color = color;
        this.strategy = strategy;
    }

    public Position MakeMove(Board board) => strategy.GetMove(board, Color);
}

public class Piece
{
    public PieceColor Color { get; }

    public Piece(PieceColor color)
    {
        Color = color;
    }
}

public enum PieceColor
{
    Black,
    White
}

public readonly struct Position
{
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }
}

public interface IMoveStrategy
{
    Position GetMove(Board board, PieceColor color);
}

public class HumanMoveStrategy : IMoveStrategy
{
    public Position GetMove(Board board, PieceColor color)
    {
        Console.Write("Enter row and col (e.g., 2 3): ");
        var input = Console.ReadLine()?.Split();
        return new Position(int.Parse(input![0]), int.Parse(input[1]));
    }
}

public class AIMoveStrategy : IMoveStrategy
{
    public Position GetMove(Board board, PieceColor color)
    {
        var moves = new List<Position>();
        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                if (board.IsValidMove(new Position(r, c), color))
                    moves.Add(new Position(r, c));

        return moves.Count > 0 ? moves[0] : new Position(-1, -1);
    }
}

public class Program
{
    public static void Main()
    {
        var player1 = new Player("Player 1", PieceColor.Black, new HumanMoveStrategy());
        var player2 = new Player("Player 2", PieceColor.White, new AIMoveStrategy());

        var controller = new GameController(player1, player2);

        controller.OnMoveMade += msg => Console.WriteLine(msg);
        controller.OnGameOver += winner =>
        {
            Console.WriteLine(winner == null ? "Draw!" : $"{winner.Name} wins!");
        };

        controller.StartGame();
    }
}
