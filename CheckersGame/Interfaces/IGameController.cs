using System.Xml.Serialization;

namespace CheckersGame
{
    public interface IGameController
    {
        void StartGame();
        void InitializeBoard();
        bool MakeMove(Position from, Position to);
        void MakeCaptureMove(Position from, Position to, List<Position> captured);
        void MovePiece(Position from, Position to, Piece piece);
        List<Position> GetCapturedPieces(Position from, Position to);
        bool CanCaptureAgain(Position pos);
        void EndTurn();
        void CheckForPromotion(Piece piece, Position pos);
        void CheckGameOver();
        bool IsValidMove(Piece piece, Position from, Position to);
        IPlayer GetCurrentPlayer();
        bool IsValidPosition(Position pos);
        Piece? GetPiece(Position pos);
        void SetPiece(Position pos, Piece piece);
        void RemovePiece(Position pos);
        IBoard GetBoard();
    }
}

