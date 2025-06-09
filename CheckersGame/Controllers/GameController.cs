namespace CheckersGame;

public class GameController : IGameController
{
    private IBoard _board;
    private IPlayer _currentPlayer;
    private List<IPlayer> _players;
    private Position _lastCapturePosition;
    public bool IsGameOver;

    public Action<IPlayer> playerTurnChanged;
    public Action<IPlayer> gameEnded;
    public Action<Position> piecePromoted;

    public GameController(List<IPlayer> players, IBoard board)
    {
        _board = board;
        _players = players;
        _currentPlayer = players[0];
        _lastCapturePosition = new Position(-1, -1);
    }

    public void StartGame()
    {
        InitializeBoard();
        playerTurnChanged?.Invoke(_currentPlayer);
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
            playerTurnChanged?.Invoke(_currentPlayer);
        }
    }

    public void CheckForPromotion(Piece piece, Position pos)
    {
        if (piece.Type == PieceType.King) return;
        if ((piece.Color == PieceColor.Red && pos.Row == 0) ||
            (piece.Color == PieceColor.Black && pos.Row == _board.Size - 1))
        {
            piece.Type = PieceType.King;
            piecePromoted?.Invoke(pos);
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
            gameEnded?.Invoke(_currentPlayer);
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

    public IPlayer GetCurrentPlayer()
    {
        return _currentPlayer;
    }
    public bool IsValidPosition(Position pos)
    {
        return pos.Row >= 0 && pos.Row < _board.Size && pos.Col >= 0 && pos.Col < _board.Size;
    }
    public Piece? GetPiece(Position pos)
    {
        return _board.Grid[pos.Row, pos.Col];
    }
    public void SetPiece(Position pos, Piece piece)
    {
        _board.Grid[pos.Row, pos.Col] = piece;
    }
    public void RemovePiece(Position pos)
    {
        _board.Grid[pos.Row, pos.Col] = null;
    }
    public IBoard GetBoard()
    {
        return _board;
    }
}

