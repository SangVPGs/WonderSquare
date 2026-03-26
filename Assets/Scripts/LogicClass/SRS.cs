using UnityEngine;

public static class SRS
{
    // JLSTZ kicks
    private static readonly Vector2Int[,] JLSTZ =
    {
        { new(0,0), new(-1,0), new(-1,1), new(0,-2), new(-1,-2) }, // 0->R
        { new(0,0), new(1,0), new(1,-1), new(0,2), new(1,2) },     // R->0

        { new(0,0), new(1,0), new(1,-1), new(0,2), new(1,2) },     // R->2
        { new(0,0), new(-1,0), new(-1,1), new(0,-2), new(-1,-2) }, // 2->R

        { new(0,0), new(1,0), new(1,1), new(0,-2), new(1,-2) },    // 2->L
        { new(0,0), new(-1,0), new(-1,-1), new(0,2), new(-1,2) },  // L->2

        { new(0,0), new(-1,0), new(-1,-1), new(0,2), new(-1,2) },  // L->0
        { new(0,0), new(1,0), new(1,1), new(0,-2), new(1,-2) }     // 0->L
    };

    // I kicks
    private static readonly Vector2Int[,] I =
    {
        { new(0,0), new(-2,0), new(1,0), new(-2,-1), new(1,2) },   // 0->R
        { new(0,0), new(2,0), new(-1,0), new(2,1), new(-1,-2) },   // R->0

        { new(0,0), new(-1,0), new(2,0), new(-1,2), new(2,-1) },   // R->2
        { new(0,0), new(1,0), new(-2,0), new(1,-2), new(-2,1) },   // 2->R

        { new(0,0), new(2,0), new(-1,0), new(2,1), new(-1,-2) },   // 2->L
        { new(0,0), new(-2,0), new(1,0), new(-2,-1), new(1,2) },   // L->2

        { new(0,0), new(1,0), new(-2,0), new(1,-2), new(-2,1) },   // L->0
        { new(0,0), new(-1,0), new(2,0), new(-1,2), new(2,-1) }    // 0->L
    };

    public static Vector2Int[] GetKickData(int index, int from, int to)
    {
        if (index == (int)Tetrominoes.Type.O)
            return new Vector2Int[] { Vector2Int.zero };

        int pair = GetPairIndex(from, to);

        Vector2Int[] result = new Vector2Int[5];

        for (int i = 0; i < 5; i++)
        {
            result[i] = (index == (int)Tetrominoes.Type.I)
                ? I[pair, i]
                : JLSTZ[pair, i];
        }

        return result;
    }

    private static int GetPairIndex(int from, int to)
    {
        if (from == 0 && to == 1) return 0;
        if (from == 1 && to == 0) return 1;

        if (from == 1 && to == 2) return 2;
        if (from == 2 && to == 1) return 3;

        if (from == 2 && to == 3) return 4;
        if (from == 3 && to == 2) return 5;

        if (from == 3 && to == 0) return 6;
        if (from == 0 && to == 3) return 7;

        return 0;
    }
}