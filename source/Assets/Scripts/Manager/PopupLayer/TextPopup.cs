using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class TextPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [Header("Animation")]
    private float countdownDuration = 1.0f;
    private float scorePopupDuration = 0.8f;
    private float moveY = 80f;

    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Init(PopupRequest request)
    {
        text.text = request.text;

        StopAllCoroutines();
        text.enableVertexGradient = false;
        text.color = Color.white;

        bool isCountdown = IsCountdown(request.text);

        if (isCountdown)
        {
            ApplyCountdownStyle(request.text);
        }
        else
        {
            ApplyScoreStyle(request.level);
        }

        StartCoroutine(Animate(isCountdown, request.level));
    }

    // ===== TYPE CHECK =====

    private bool IsCountdown(string t)
    {
        return t == "3" || t == "2" || t == "1" || t == "GO!";
    }

    // ===== SCORE STYLE =====

    private void ApplyScoreStyle(int level)
    {
        switch (level)
        {
            case 1:
                text.color = Color.white;
                break;

            case 2:
                ApplyGradient2();
                break;

            case 3:
                ApplyGradient4();
                break;

            case 4:
                StartCoroutine(ApplyRainbow());
                break;
        }
    }

    // ===== COUNTDOWN STYLE =====

    private void ApplyCountdownStyle(string t)
    {
        text.enableVertexGradient = false;

        if (t == "GO!")
            text.color = Color.green;
        else
            text.color = Color.white;

        rect.localScale = Vector3.one * 1.5f;
    }

    // ===== GRADIENT =====

    private void ApplyGradient2()
    {
        text.enableVertexGradient = true;

        Color left = Color.yellow;
        Color right = new Color(1f, 0.5f, 0f);

        text.colorGradient = new VertexGradient(left, right, left, right);
    }

    private void ApplyGradient4()
    {
        text.enableVertexGradient = true;

        text.colorGradient = new VertexGradient(
            Color.red,
            Color.yellow,
            Color.blue,
            Color.magenta
        );
    }

    private IEnumerator ApplyRainbow()
    {
        text.enableVertexGradient = true;

        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * 1.2f;

            text.colorGradient = new VertexGradient(
                Color.HSVToRGB(Mathf.Repeat(t, 1f), 1f, 1f),
                Color.HSVToRGB(Mathf.Repeat(t + 0.25f, 1f), 1f, 1f),
                Color.HSVToRGB(Mathf.Repeat(t + 0.5f, 1f), 1f, 1f),
                Color.HSVToRGB(Mathf.Repeat(t + 0.75f, 1f), 1f, 1f)
            );

            yield return null;
        }
    }

    // ===== ANIMATION =====

    private IEnumerator Animate(bool isCountdown, int level)
    {
        float time = 0f;

        float animDuration = isCountdown ? countdownDuration : scorePopupDuration;

        Vector2 start = rect.anchoredPosition;
        Vector2 end = isCountdown
            ? start
            : start + Vector2.up * moveY;

        Vector3 baseScale = rect.localScale;

        while (time < animDuration)
        {
            float t = time / animDuration;

            rect.anchoredPosition = Vector2.Lerp(start, end, t);

            if (!isCountdown)
            {
                float scale = Mathf.Lerp(
                    0.8f,
                    1.2f + level * 0.1f,
                    Mathf.Sin(t * Mathf.PI)
                );

                rect.localScale = baseScale * scale;
            }

            // fade
            canvasGroup.alpha = 1f - t;

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}