using UnityEngine;
using UnityEngine.InputSystem;

public class DisableMovementController : MonoBehaviour
{
    public InputActionReference moveAction;  // 左摇杆移动
    public InputActionReference rotateAction; // 右摇杆旋转

    public void DisableMovement()
    {
        if (moveAction.action != null) moveAction.action.Disable();
        if (rotateAction.action != null) rotateAction.action.Disable();
        Debug.Log("禁用玩家移动和旋转");
    }

    public void EnableMovement()
    {
        if (moveAction.action != null) moveAction.action.Enable();
        if (rotateAction.action != null) rotateAction.action.Enable();
        Debug.Log("恢复玩家移动和旋转");
    }
}
