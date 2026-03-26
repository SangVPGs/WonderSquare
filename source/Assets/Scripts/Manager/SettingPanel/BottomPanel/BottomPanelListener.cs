using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BottomPanelListener : MonoBehaviour
{
    [SerializeField] private RectTransform gameView;
    public float height = 375f;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        SettingsManager.OnBottomPanelChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        SettingsManager.OnBottomPanelChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        bool show = SettingsManager.ShowBottomPanel;

        Debug.Log("Bottom: " + show);

        float h = show ? height : 0f;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        if (gameView != null)
        {
            gameView.offsetMin = new Vector2(gameView.offsetMin.x, h);
        }

        // Tắt hiện nội dung không tắt object
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(show);
        }
    }
}