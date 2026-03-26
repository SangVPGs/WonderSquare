using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;

    private Cell[,] cells;
    private Board board;

    public void Init(Board boardLogic)
    {
        board = boardLogic;

        cells = new Cell[board.Width, board.Height];

        for (int y = 0; y < board.Height; y++)
        {
            for (int x = 0; x < board.Width; x++)
            {
                cells[x, y] = Instantiate(cellPrefab, cellsTransform);
                cells[x, y].transform.position = new Vector3(x, y, 0f);
                cells[x, y].Hide();
            }
        }
    }

    public void Render(Piece piece)
    {
        var data = board.Data;

        // Fixed blocks
        for (int y = 0; y < board.Height; y++)
        {
            for (int x = 0; x < board.Width; x++)
            {
                if (data[x, y] > 0)
                    cells[x, y].Show(Tetrominoes.Colors[data[x, y] - 1]);
                else
                    cells[x, y].Hide();
            }
        }

        var shape = Tetrominoes.Get(piece.TypeIndex, piece.Rotation);
        var color = Tetrominoes.Colors[piece.TypeIndex];
        Vector2Int ghostPos = piece.GhostPosition;

        // Ghost piece
        foreach (var block in shape)
        {
            Vector2Int pos = ghostPos + block;

            if (pos.x >= 0 && pos.x < board.Width &&
                pos.y >= 0 && pos.y < board.Height)
            {
                cells[pos.x, pos.y].Ghost(color);
            }
        }

        // Active piece
        foreach (var block in shape)
        {
            Vector2Int pos = piece.Position + block;

            if (pos.x >= 0 && pos.x < board.Width &&
                pos.y >= 0 && pos.y < board.Height)
            {
                cells[pos.x, pos.y].Show(color);
            }
        }
    }

    public IEnumerator AnimateLinesClear(List<int> rows, float fadeDuration)
    {
        // trigger animation đồng thời
        foreach (int y in rows)
        {
            for (int x = 0; x < board.Width; x++)
            {
                if (board.Data[x, y] > 0)
                {
                    cells[x, y].PlayClearAnimation(fadeDuration);
                }
            }
        }

        // chờ tất cả animation xong
        yield return new WaitForSeconds(fadeDuration);
    }
}