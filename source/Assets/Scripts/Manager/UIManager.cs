using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private MiniGridRenderer holdRenderer;
    [SerializeField] private MiniGridRenderer nextRenderer;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private GameObject exitConfirmPanel;
    [SerializeField] private TMP_Text currentScoreText;
    private int currentScore;
    [SerializeField] private const string mainMenuScene = "Menu";
    [SerializeField] private GameObject settingsPanel;

    //public GameState currentState = GameState.Playing;

    //public enum GameState
    //{
    //    Playing,
    //    Settings,
    //    GameOver,
    //    ExitConfirm,
    //}

    public static System.Action<bool> SetPause;

    private void Awake()
    {
        gameOver.SetActive(false);
        settingsPanel.SetActive(false);
        exitConfirmPanel.SetActive(false);
    }

    private void OnEnable()
    {
        TetrisController.OnScoreChanged += UpdateScore;
        TetrisController.OnNextChanged += ShowNext;
        TetrisController.OnHoldChanged += UpdateHold;
        TetrisController.OnGameOver += GameOver;

        TetrisController.OnOpenSetting += OpenSetting;
    }

    private void OnDisable()
    {
        TetrisController.OnScoreChanged -= UpdateScore;
        TetrisController.OnNextChanged -= ShowNext;
        TetrisController.OnHoldChanged -= UpdateHold;
        TetrisController.OnGameOver -= GameOver;

        TetrisController.OnOpenSetting -= OpenSetting;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"{score}";
        currentScore = score;
    }

    private void GameOver(int finalScore)
    {
        gameOver.SetActive(true);
        finalScoreText.text += $": {finalScore}";
        Time.timeScale = 0f;
        //currentState = GameState.GameOver;
    }

    private void ShowNext(int type)
    {
        nextRenderer.Show(type);
    }

    private void UpdateHold(int? type)
    {
        if (type.HasValue)
            holdRenderer.Show(type.Value);
        else
            holdRenderer.Clear();
    }

    public void OpenSetting()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
        SetPause(true);
        //currentState = GameState.Settings;
    }

    public void CloseSetting()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        SetPause(false);
        //currentState = GameState.Playing;
    }

    public void OpenExitConfirm()
    {
        exitConfirmPanel.SetActive(true);
        Time.timeScale = 0f;
        currentScoreText.text += $": {currentScore}";
        SetPause(true);
        //currentState = GameState.ExitConfirm;
    }

    public void SaveAndBack()
    {
        HistoryManager.AddRecord(currentScore);
        BackToMainMenu();
    }

    public void CloseExitConfirm()
    {
        exitConfirmPanel.SetActive(false);
        currentScoreText.text = $"Score";
        Time.timeScale = 1f;
        SetPause(false);
        //currentState = GameState.Playing;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}