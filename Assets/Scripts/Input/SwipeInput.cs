using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    [SerializeField] private TetrisController controller;

    private Vector2 startPos;
    private float minSwipeDistance = 50f;

    void Update()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        HandleTouch();
#endif
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            Vector2 delta = touch.position - startPos;

            if (delta.magnitude < minSwipeDistance)
            {
                // tap
                controller.InputRightRotate();
                return;
            }

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 0)
                    controller.InputMoveRight();
                else
                    controller.InputMoveLeft();
            }
            else
            {
                if (delta.y < 0)
                    controller.InputHardDrop();
            }
        }
    }
}