using UnityEngine;
using UnityEngine.UI;

public class MiniGridRenderer : MonoBehaviour
{
    [SerializeField] private Image miniCellPrefab;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Show(int typeIndex)
    {
        Clear();

        var shape = Tetrominoes.Get(typeIndex, 0);
        var color = Tetrominoes.Colors[typeIndex];

        // Tính bounding box thật
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        foreach (var b in shape)
        {
            if (b.x < minX) minX = b.x;
            if (b.x > maxX) maxX = b.x;
            if (b.y < minY) minY = b.y;
            if (b.y > maxY) maxY = b.y;
        }

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        float cellSize = Mathf.Min(
            rect.rect.width / width,
            rect.rect.height / height
        );

        float totalWidth = width * cellSize;
        float totalHeight = height * cellSize;

        float startX = -totalWidth / 2f + cellSize / 2f;
        float startY = -totalHeight / 2f + cellSize / 2f;

        foreach (var b in shape)
        {
            int x = b.x - minX; // normalize về 0
            int y = b.y - minY;

            Image img = Instantiate(miniCellPrefab, transform);
            RectTransform rt = img.rectTransform;

            rt.sizeDelta = new Vector2(cellSize, cellSize);
            rt.anchoredPosition = new Vector2(
                startX + x * cellSize,
                startY + y * cellSize
            );

            img.color = color;
        }
    }

    public void Clear()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
}