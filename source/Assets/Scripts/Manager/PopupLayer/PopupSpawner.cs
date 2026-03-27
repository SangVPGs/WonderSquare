using UnityEngine;
using System.Collections;

public class PopupSpawner : MonoBehaviour
{
    [SerializeField] private TextPopup prefab;
    [SerializeField] private RectTransform parent;

    [SerializeField] private float countdownDelay = 1f;

    public static System.Action<bool> SetCountdown;

    private void OnEnable()
    {
        TetrisController.OnScorePopup += OnScore;
        TetrisController.OnCountdown += OnCountdown;
    }

    private void OnDisable()
    {
        TetrisController.OnScorePopup -= OnScore;
        TetrisController.OnCountdown -= OnCountdown;
    }

    // ===== SCORE =====
    private void OnScore(int cleared)
    {
        if (cleared <= 0) return;

        string text;

        switch(cleared)
        {
            case 1:
                text = "+10";
                break;
            case 2:
                text = "+30";
                break;
            case 3:
                text = "+50";
                break;
            default:
                text = "Tetris!";
                break;
        }

        Spawn(new PopupRequest(text, cleared));
    }

    // ===== COUNTDOWN =====
    private void OnCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        SetCountdown(true);

        yield return Show("3", 3);
        yield return Show("2", 3);
        yield return Show("1", 3);
        yield return Show("GO!", 4);

        SetCountdown(false);
    }

    private IEnumerator Show(string text, int level)
    {
        Spawn(new PopupRequest(text, level));

        if(level < 4)
        {
            AudioManager.Instance.PlayCountdown(0);
        }
        else
        {
            AudioManager.Instance.PlayCountdown(1);
        }

        yield return new WaitForSeconds(countdownDelay);
    }

    // ===== CORE =====
    public void Spawn(PopupRequest request)
    {
        var popup = Instantiate(prefab, parent);
        popup.Init(request);
    }
}