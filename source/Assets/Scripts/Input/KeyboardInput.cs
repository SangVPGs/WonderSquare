using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private TetrisController controller;

    [Header("DAS / ARR")]
    [SerializeField] private float das = 0.1f;
    [SerializeField] private float arr = 0.05f;

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

        if (Keyboard.current.leftArrowKey.isPressed) inputDir = -1;
        else if (Keyboard.current.rightArrowKey.isPressed) inputDir = 1;

        if (inputDir != 0)
        {
            if (direction != inputDir)
            {
                // Nhấn lần đầu
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
        var kb = Keyboard.current;

        if (kb.downArrowKey.wasPressedThisFrame)
            controller.InputSoftDrop();

        if (kb.upArrowKey.wasPressedThisFrame)
            controller.InputRightRotate();

        if (kb.zKey.wasPressedThisFrame)
            controller.InputLeftRotate();

        if (kb.spaceKey.wasPressedThisFrame)
            controller.InputHardDrop();

        if (kb.cKey.wasPressedThisFrame)
            controller.InputHold();

        if (kb.sKey.wasPressedThisFrame)
            controller.InputOpenSetting();

        // if (kb.escapeKey.wasPressedThisFrame)
        //     controller.InputESC();
    }
}