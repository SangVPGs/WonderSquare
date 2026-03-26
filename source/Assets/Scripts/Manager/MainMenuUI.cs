using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject historyPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject guidePanel;

    private string gameScene = "Game";

    private GameObject currentPanel;

    private void Awake()
    {
        mainMenuPanel.SetActive(false);
        historyPanel.SetActive(false);
        settingsPanel.SetActive(false);
        guidePanel.SetActive(false);

        ShowPanel(mainMenuPanel);
    }

    public void Play()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void OpenSettings()
    {
        ShowPanel(settingsPanel);
    }

    public void OpenHistory()
    {
        ShowPanel(historyPanel);
    }

    public void OpenGuide()
    {
        ShowPanel(guidePanel);
    }

    public void BackToMenu()
    {
        ShowPanel(mainMenuPanel);
    }

    private void ShowPanel(GameObject panel)
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);

        panel.SetActive(true);
        currentPanel = panel;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}