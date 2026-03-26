using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private Image background;

    public void Setup(GameRecord record, int rank, bool dark)
    {
        rankText.text = rank.ToString();
        scoreText.text = record.score.ToString();
        dateText.text = record.date;

        if (dark)
            background.color = new Color(0.18f, 0.18f, 0.18f);
        else
            background.color = new Color(0.25f, 0.25f, 0.25f);
    }
}