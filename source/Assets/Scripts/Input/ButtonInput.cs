using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public enum ActionType
    {
        Left, Right, Down, LeftRotate, RightRotate, HardDrop, Hold
    }

    public ActionType action;
    public TetrisController controller;

    [Header("Hold (DAS / ARR)")]
    [SerializeField] private float holdDelay = 0.1f;
    [SerializeField] private float repeatRate = 0.05f;

    private bool isHolding;
    private float holdTimer;
    private float repeatTimer;

    void Update()
    {
        if (!isHolding) return;

        // Những action KHÔNG hold
        if (!IsHoldable(action)) return;

        holdTimer += Time.deltaTime;

        if (holdTimer >= holdDelay)
        {
            repeatTimer += Time.deltaTime;

            if (repeatTimer >= repeatRate)
            {
                Trigger();
                repeatTimer = 0f;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdTimer = 0f;
        repeatTimer = 0f;

        Trigger(); // click lần đầu
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Fix bug kéo tay ra ngoài vẫn giữ
        isHolding = false;
    }

    bool IsHoldable(ActionType type)
    {
        return type == ActionType.Left ||
               type == ActionType.Right ||
               type == ActionType.Down;
    }

    void Trigger()
    {
        switch (action)
        {
            case ActionType.Left: controller.InputMoveLeft(); break;
            case ActionType.Right: controller.InputMoveRight(); break;
            case ActionType.Down: controller.InputSoftDrop(); break;

            case ActionType.LeftRotate: controller.InputLeftRotate(); break;
            case ActionType.RightRotate: controller.InputRightRotate(); break;
            case ActionType.HardDrop: controller.InputHardDrop(); break;
            case ActionType.Hold: controller.InputHold(); break;
        }
    }
}