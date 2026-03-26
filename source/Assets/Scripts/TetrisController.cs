using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TetrisController : MonoBehaviour
{
    public static readonly Vector2Int Size = new Vector2Int(10, 22);

    private Board board;
    private Piece currentPiece;
    private BagRandomizer bag = new BagRandomizer();
    private new BoardRenderer renderer;

    private float dropTimer;
    private bool isPlaying = true;
    private bool isGameOver;

    private int totalLines;
    private int score;

    private int? heldType = null;
    private bool canHold = true;
    private bool isLocking = false;

    // Events UI can subscribe to for updates
    public static System.Action<int> OnScoreChanged;
    public static System.Action<int> OnNextChanged;
    public static System.Action<int?> OnHoldChanged;
    public static System.Action OnCountdown;
    public static System.Action<int> OnGameOver;
    public static System.Action<int> OnScorePopup;

    private AudioManager audioManager => AudioManager.Instance;
    private UIManager uiManager => UIManager.Instance;

    private int Level => totalLines / 10;

    private float GetGravity()
    {
        return Mathf.Max(0.04f, 0.9f * Mathf.Pow(0.82f, Level));
    }

    public void SetPlaying (bool value)
    {
        isPlaying = value;
    }

    private void Awake()
    {
        board = new Board(Size.x, Size.y);
        renderer = GetComponent<BoardRenderer>();
    }

    private void Start()
    {
        renderer.Init(board);
        OnCountdown?.Invoke();

        SpawnPiece();       
        currentPiece.UpdateGhost(board);
        renderer.Render(currentPiece);
    }

    private void Update()
    {
        if (isGameOver || !isPlaying) return;

        dropTimer += Time.deltaTime;
        if (dropTimer >= GetGravity())
        {
            dropTimer = 0f;
            SoftDrop();
        }

        currentPiece.UpdateGhost(board);
        renderer.Render(currentPiece);
    }

    private void SpawnPiece()
    {
        int type = bag.Next();
        currentPiece = new Piece(type, new Vector2Int(3, Size.y - 4));
        OnNextChanged?.Invoke(bag.Peek());

        if (!board.IsValidPosition(currentPiece, currentPiece.Position, currentPiece.Rotation))
        {
            isGameOver = true;
            OnGameOver?.Invoke(score);
            HistoryManager.AddRecord(score);
            audioManager.PlayGameOver();
        }

        canHold = true;
    }

    private void SoftDrop()
    {
        Vector2Int newPos = currentPiece.Position + Vector2Int.down;

        if (board.IsValidPosition(currentPiece, newPos, currentPiece.Rotation))
        {
            currentPiece.Position = newPos;
        }
        else
        {
            LockAndSpawn();
        }
    }

    private void HardDrop()
    {
        int dropped = currentPiece.HardDrop(board);
        score += dropped;
        OnScoreChanged?.Invoke(score);
        LockAndSpawn();
    }

    private void Hold()
    {
        if (!canHold) return;

        int currentType = currentPiece.TypeIndex;

        if (heldType == null)
        {
            heldType = currentType;
            SpawnPiece();
        }
        else
        {
            int swap = heldType.Value;
            heldType = currentType;
            currentPiece = new Piece(swap, new Vector2Int(3, Size.y - 4));
        }

        canHold = false;

        OnHoldChanged?.Invoke(heldType);
    }

    private void LockAndSpawn()
    {
        if (isLocking) return;
        StartCoroutine(LockAndSpawnRoutine());
    }

    private IEnumerator LockAndSpawnRoutine()
    {
        isLocking = true;

        board.LockPiece(currentPiece);

        yield return StartCoroutine(ClearLinesRoutine());

        SpawnPiece();
        currentPiece.UpdateGhost(board);
        renderer.Render(currentPiece);

        isLocking = false;
    }

    private IEnumerator ClearLinesRoutine()
    {
        List<int> rows = board.GetFullRows();

        if (rows.Count == 0)
        {
            yield break;
        }

        // animate tất cả dòng
        yield return StartCoroutine(renderer.AnimateLinesClear(rows, 0.2f));


        int cleared = board.ClearFullRows();
        totalLines += cleared;

        switch (cleared)
        {
            case 1:
            case 2:
                audioManager.PlayLineClear();
                break;
            case 3:
            case 4:
                audioManager.PlayHighLineClear();
                break;
        }

        OnScorePopup?.Invoke(cleared);
        AddLineScore(cleared);
    }

    private void AddLineScore(int linesCleared)
    {
        if (linesCleared == 0) return;

        int lv = Level + 1;
        int baseScore = 0;

        switch (linesCleared)
        {
            case 1: baseScore = 10; break;
            case 2: baseScore = 30; break;
            case 3: baseScore = 50; break;
            case 4: baseScore = 80; break;
        }

        score += baseScore * lv;
        OnScoreChanged?.Invoke(score);
    }

    // ================= INPUT API =================

    public void InputMoveLeft()
    {
        if (!CanInput()) return;

        currentPiece.Move(Vector2Int.left, board);
        audioManager?.PlayMove();
    }

    public void InputMoveRight()
    {
        if (!CanInput()) return;

        currentPiece.Move(Vector2Int.right, board);
        audioManager?.PlayMove();
    }

    public void InputSoftDrop()
    {
        if (!CanInput()) return;

        SoftDrop();
        audioManager?.PlayMove();

        score += 1;
        OnScoreChanged?.Invoke(score);
    }

    public void InputRightRotate()
    {
        if (!CanInput()) return;

        currentPiece.Rotate(1, board);
        audioManager?.PlayRotate();
    }

    public void InputLeftRotate()
    {
        if (!CanInput()) return;
        currentPiece.Rotate(-1, board);
        audioManager?.PlayRotate();
    }

    public void InputHardDrop()
    {
        if (!CanInput()) return;

        HardDrop();
        audioManager?.PlayHardDrop();
    }

    public void InputHold()
    {
        if (!CanInput()) return;

        Hold();
        audioManager?.PlayHold();
    }

    public void InputOpenSetting()
    {
        if(!CanInput()) return;
        uiManager.OpenSetting();
    }

    //public void InputESC()
    //{
    //    switch(uiManager.currentState)
    //    {
    //        case UIManager.GameState.Playing:
    //            uiManager.OpenExitConfirm();
    //            break;
    //        case UIManager.GameState.Settings:
    //            uiManager.CloseSetting();
    //            break;
    //        case UIManager.GameState.ExitConfirm:
    //            uiManager.CloseExitConfirm();
    //            break;
    //        case UIManager.GameState.GameOver:
    //            uiManager.BackToMainMenu();
    //            break;
    //    }
    //}

    // ================= HELPER =================

    private bool CanInput()
    {
        return !isGameOver && isPlaying && currentPiece != null;
    }
}