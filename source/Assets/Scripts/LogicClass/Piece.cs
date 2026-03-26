using UnityEngine;

public class Piece
{
    public int TypeIndex { get; private set; }
    public Vector2Int Position;
    public int Rotation;

    private Vector2Int ghostPosition;
    private bool ghostDirty = true;

    public Vector2Int GhostPosition => ghostPosition;

    public Piece(int type, Vector2Int spawnPos)
    {
        TypeIndex = type;
        Position = spawnPos;
        Rotation = 0;
    }

    public void Move(Vector2Int dir, Board board)
    {
        Vector2Int newPos = Position + dir;

        if (board.IsValidPosition(this, newPos, Rotation))
            Position = newPos;
            ghostDirty = true;
    }

    public void Rotate(int direction, Board board)
    {
        int oldRotation = Rotation;
        int newRotation = (Rotation + direction + 4) % 4;

        var kicks = SRS.GetKickData(TypeIndex, oldRotation, newRotation);

        foreach (var offset in kicks)
        {
            Vector2Int newPos = Position + offset;

            if (board.IsValidPosition(this, newPos, newRotation))
            {
                Position = newPos;
                Rotation = newRotation;
                ghostDirty = true;
                return;
            }
        }
    }

    public void UpdateGhost(Board board)
    {
        if (!ghostDirty) return;

        Vector2Int pos = Position;

        while (board.IsValidPosition(this, pos + Vector2Int.down, Rotation))
            pos += Vector2Int.down;

        ghostPosition = pos;
        ghostDirty = false;
    }

    public int HardDrop(Board board)
    {
        int dropDistance = 0;

        while (board.IsValidPosition(this, Position + Vector2Int.down, Rotation))
        {
            Position += Vector2Int.down;
            dropDistance++;
        }
        
        return dropDistance;
    }
}