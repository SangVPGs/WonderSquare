using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public readonly int Width;
    public readonly int Height;

    private int[,] data; // [x,y]

    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        data = new int[Width, Height];
    }

    public int[,] Data => data;

    public bool IsValidPosition(Piece piece, Vector2Int pos, int rotation)
    {
        var shape = Tetrominoes.Get(piece.TypeIndex, rotation);

        foreach (var block in shape)
        {
            Vector2Int check = pos + block;

            if (check.x < 0 || check.x >= Width) return false;
            if (check.y < 0 || check.y >= Height) return false;
            if (data[check.x, check.y] > 0) return false;
        }

        return true;
    }

    public void LockPiece(Piece piece)
    {
        var shape = Tetrominoes.Get(piece.TypeIndex, piece.Rotation);

        foreach (var block in shape)
        {
            Vector2Int pos = piece.Position + block;
            data[pos.x, pos.y] = piece.TypeIndex + 1;
        }
    }

    public int ClearFullRows()
    {
        int cleared = 0;

        for (int y = 0; y < Height;)
        {
            if (IsRowFull(y))
            {
                ShiftDown(y);
                cleared++;
            }
            else
            {
                y++;
            }
        }

        return cleared;
    }

    public List<int> GetFullRows()
    {
        List<int> rows = new List<int>();

        for (int y = 0; y < Height; y++)
        {
            if (IsRowFull(y))
                rows.Add(y);
        }

        return rows;
    }

    public bool IsRowFull(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (data[x, y] == 0)
                return false;
        }

        return true;
    }

    public void ShiftDown(int clearedRow)
    {
        for (int y = clearedRow; y < Height - 1; y++)
        {
            for (int x = 0; x < Width; x++)
                data[x, y] = data[x, y + 1];
        }

        for (int x = 0; x < Width; x++)
            data[x, Height - 1] = 0;
    }
}