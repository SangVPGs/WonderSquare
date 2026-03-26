using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private TetrisController controller;

    [Header("DAS / ARR")]
    private float das = 0.1f;   // delay trước khi auto
    private float arr = 0.05f;  // tốc độ lặp

    private float holdTimer;
    private float repeatTimer;
    private int direction; // -1 trái, 1 phải

    void Update()
    {
        if (Time.timeScale == 0f) return;

        HandleHorizontal();
        HandleOtherInput();
    }

    void HandleHorizontal()
    {
        int inputDir = 0;

        if (Input.GetKey(KeyCode.LeftArrow)) inputDir = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) inputDir = 1;

        if (inputDir != 0)
        {
            if (direction != inputDir)
            {
                // nhấn lần đầu
                direction = inputDir;
                holdTimer = 0f;
                repeatTimer = 0f;

                Move(direction);
                return;
            }

            holdTimer += Time.deltaTime;

            if (holdTimer >= das)
            {
                repeatTimer += Time.deltaTime;

                if (repeatTimer >= arr)
                {
                    Move(direction);
                    repeatTimer = 0f;
                }
            }
        }
        else
        {
            direction = 0;
        }
    }

    void Move(int dir)
    {
        if (dir == -1) controller.InputMoveLeft();
        else controller.InputMoveRight();
    }

    void HandleOtherInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            controller.InputSoftDrop();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            controller.InputRightRotate();

        if (Input.GetKeyDown(KeyCode.Z))
            controller.InputLeftRotate();

        if (Input.GetKeyDown(KeyCode.Space))
            controller.InputHardDrop();

        if (Input.GetKeyDown(KeyCode.C))
            controller.InputHold();

        if (Input.GetKeyDown(KeyCode.S))
            controller.InputOpenSetting();

        //if (Input.GetKeyDown(KeyCode.Escape))
        //    controller.InputESC();

    }
}