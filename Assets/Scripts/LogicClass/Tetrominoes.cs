using UnityEngine;

public static class Tetrominoes
{
    public enum Type { I, J, L, O, S, T, Z }

    // Spawn state (rotation = 0)
    private static readonly Vector2Int[][] tetrominoes =
    {
        // I (4x4)
        new Vector2Int[]
        {
            new(0,2), new(1,2), new(2,2), new(3,2)
        },

        // J
        new Vector2Int[]
        {
            new(0,2), new(0,1), new(1,1), new(2,1)
        },

        // L
        new Vector2Int[]
        {
            new(2,2), new(0,1), new(1,1), new(2,1)
        },

        // O
        new Vector2Int[]
        {
            new(1,2), new(2,2), new(1,1), new(2,1)
        },

        // S
        new Vector2Int[]
        {
            new(1,2), new(2,2), new(0,1), new(1,1)
        },

        // T
        new Vector2Int[]
        {
            new(1,2), new(0,1), new(1,1), new(2,1)
        },

        // Z
        new Vector2Int[]
        {
            new(0,2), new(1,2), new(1,1), new(2,1)
        }
    };

    public static Vector2Int[] Get(int index, int rotation)
    {
        if (index == (int)Type.O)
            return tetrominoes[index];

        var baseShape = tetrominoes[index];
        var result = new Vector2Int[baseShape.Length];

        Vector2 pivot = GetPivot(index);

        for (int i = 0; i < baseShape.Length; i++)
        {
            Vector2 p = baseShape[i];

            for (int r = 0; r < rotation; r++)
            {
                p -= pivot;
                p = new Vector2(p.y, -p.x); // rotate CW
                p += pivot;
            }

            result[i] = Vector2Int.RoundToInt(p);
        }

        return result;
    }

    private static Vector2 GetPivot(int index)
    {
        if (index == (int)Type.I)
            return new Vector2(1.5f, 1.5f);

        return new Vector2(1f, 1f);
    }

    public static readonly Color[] Colors = new Color[] { 
        new(0.0f, 0.839f, 0.957f), // I - Cyan
        new(0.141f, 0.396f, 1.0f), // J - Blue
        new(1.0f, 0.541f, 0.0f), // L - Orange
        new(1.0f, 0.882f, 0.0f), // O - Yellow
        new(0.0f, 0.827f, 0.0f), // S - Green
        new(0.733f, 0.271f, 0.871f), // T - Purple
        new(1.0f, 0.224f, 0.412f) // Z - Red
    };

    public static int length => tetrominoes.Length;
}
