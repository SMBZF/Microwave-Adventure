using UnityEngine;
using UnityEngine.InputSystem;

public class DisableMovementController : MonoBehaviour
{
    public InputActionReference moveAction;  // ��ҡ���ƶ�
    public InputActionReference rotateAction; // ��ҡ����ת

    public void DisableMovement()
    {
        if (moveAction.action != null) moveAction.action.Disable();
        if (rotateAction.action != null) rotateAction.action.Disable();
        Debug.Log("��������ƶ�����ת");
    }

    public void EnableMovement()
    {
        if (moveAction.action != null) moveAction.action.Enable();
        if (rotateAction.action != null) rotateAction.action.Enable();
        Debug.Log("�ָ�����ƶ�����ת");
    }
}
