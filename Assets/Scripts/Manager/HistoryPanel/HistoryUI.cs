using UnityEngine;

public class HistoryUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject rowPrefab;

    private void OnEnable()
    {
        LoadTable();
    }

    private void LoadTable()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        GameHistory history = HistoryManager.LoadHistory();
        history.records.Sort((a, b) => b.score.CompareTo(a.score));

        int rank = 1;

        foreach (var record in history.records)
        {
            GameObject row = Instantiate(rowPrefab, content);

            bool dark = rank % 2 == 0;

            row.GetComponent<HistoryRowUI>()
               .Setup(record, rank, dark);

            rank++;
        }
    }
}