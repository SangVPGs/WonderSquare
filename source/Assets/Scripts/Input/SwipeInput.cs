using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwipeInput : MonoBehaviour
{
    [SerializeField] private TetrisController controller;

    private Vector2 startPos;
    private bool isTouching = false;

    private float minSwipeDistance;

    // Tuning
    private float swipeThresholdInches = 0.2f; // khoảng 0.2 inch là swipe
    private float directionDeadZone = 0.4f; // tránh chéo

    void Awake()
    {
        float dpi = Screen.dpi;

        // fallback nếu device không trả DPI
        if (dpi == 0)
            dpi = 160f;

        minSwipeDistance = dpi * swipeThresholdInches;
    }

    void Update()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        if(controller.isPlaying)
            HandleTouch();
        else
            ResetSwipe();
#endif
    }

    void HandleTouch()
    {
        // Tránh lẫn giữa tap và click UI
        if (IsTouchButtonUI())
        {
            isTouching = false;
            return;
        }

        // MOBILE TOUCH
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.isPressed)
            {
                if (!isTouching)
                {
                    isTouching = true;
                    startPos = touch.position.ReadValue();
                }
            }
            else if (isTouching)
            {
                isTouching = false;

                Vector2 endPos = touch.position.ReadValue();
                ProcessSwipe(endPos);
            }
        }

#if UNITY_EDITOR
        // MOUSE TEST
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                isTouching = true;
                startPos = Mouse.current.position.ReadValue();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isTouching = false;

                Vector2 endPos = Mouse.current.position.ReadValue();
                ProcessSwipe(endPos);
            }
        }
#endif
    }

    void ProcessSwipe(Vector2 endPos)
    {
        Vector2 delta = endPos - startPos;

        // Tap
        if (delta.magnitude < minSwipeDistance)
        {
            controller.InputRightRotate();
            return;
        }

        Vector2 dir = delta.normalized;

        // Dead zone để tránh swipe chéo bị hiểu sai
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (Mathf.Abs(dir.x) < directionDeadZone) return;

            if (dir.x > 0)
            {
                controller.InputMoveRight();
            }
            else
            {
                controller.InputMoveLeft();
            }               
        }
        else
        {
            if (Mathf.Abs(dir.y) < directionDeadZone) return;

            if (dir.y < 0)
            {
                controller.InputHardDrop();
            }              
        }
    }

    bool IsTouchButtonUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

#if UNITY_EDITOR
        pointerData.position = Mouse.current.position.ReadValue();
#else
    pointerData.position = Touchscreen.current.primaryTouch.position.ReadValue();
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var r in results)
        {
            // Nếu trúng UI Button / Slider / InputField / Toggle
            if (r.gameObject.GetComponent<Selectable>() != null)
                return true;
        }

        return false;
    }

    public void ResetSwipe()
    {
        isTouching = false; // xóa touch hiện tại
        startPos = Vector2.zero;
    }
}