using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ActionType
    {
        Left, Right, Down, LeftRotate, RightRotate, HardDrop, Hold
    }

    public ActionType action;
    public TetrisController controller;

    [Header("Hold")]
    public float holdDelay = 0.1f;
    public float repeatRate = 0.05f;

    private bool isHolding;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        StartCoroutine(HoldRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    IEnumerator HoldRoutine()
    {
        Trigger(); // click lần đầu

        yield return new WaitForSeconds(holdDelay);

        while (isHolding)
        {
            Trigger();
            yield return new WaitForSeconds(repeatRate);
        }
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