using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Show(Color color)
    {
        gameObject.SetActive(true);
        spriteRenderer.color = color;
    }

    public void Ghost(Color color)
    {
        color.a = 0.3f;
        Show(color);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //// 🔥 Fade animation - bản cũ
    //public void PlayClearAnimation(float duration)
    //{
    //    if (animCoroutine != null)
    //        StopCoroutine(animCoroutine);

    //    animCoroutine = StartCoroutine(FadeOut(duration));
    //}

    //private IEnumerator FadeOut(float duration)
    //{
    //    float t = 0;
    //    Color start = spriteRenderer.color;

    //    while (t < duration)
    //    {
    //        t += Time.deltaTime;

    //        float alpha = Mathf.Lerp(1f, 0f, t / duration);
    //        spriteRenderer.color = new Color(start.r, start.g, start.b, alpha);

    //        // scale nhẹ cho đẹp
    //        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, t / duration);

    //        yield return null;
    //    }

    //    spriteRenderer.color = new Color(start.r, start.g, start.b, 0f);
    //}

    public void PlayClearAnimation(float duration)
    {
        StartCoroutine(ClearAnim(duration));
    }

    private IEnumerator ClearAnim(float duration)
    {
        float time = 0f;
        Vector3 startScale = transform.localScale;
        Color startColor = spriteRenderer.color;

        while (time < duration)
        {
            float t = time / duration;

            // scale nhỏ dần
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            // fade out
            Color c = startColor;
            c.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = c;

            time += Time.deltaTime;
            yield return null;
        }

        // reset để dùng lại
        transform.localScale = startScale;
        spriteRenderer.color = startColor;

        Hide();
    }
}
