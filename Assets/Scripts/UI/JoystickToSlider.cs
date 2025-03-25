using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class JoystickToSlider : MonoBehaviour
{
    public Slider frequencySlider;
    public InputActionReference joystickInputAction; // 额外绑定摇杆输入
    public float sliderSpeed = 2.0f;
    public bool isUIActive = false;

    private DisableMovementController movementController; // 用于管理移动的脚本

    private void OnEnable()
    {
        ToggleUIWithButton.OnUIStateChanged += UpdateUIState;
    }

    private void OnDisable()
    {
        ToggleUIWithButton.OnUIStateChanged -= UpdateUIState;
    }

    private void Start()
    {
        // 在场景中找到 DisableMovementController
        movementController = FindObjectOfType<DisableMovementController>();
        if (movementController == null)
        {
            Debug.LogError("找不到 DisableMovementController，请确保它挂在玩家对象上！");
        }
    }

    private void UpdateUIState(bool active)
    {
        isUIActive = active;
        Debug.Log($"UI 状态变更: isUIActive = {isUIActive}");

        if (movementController != null)
        {
            if (isUIActive)
                movementController.DisableMovement();
            else
                movementController.EnableMovement();
        }
    }

    private void Update()
    {
        if (!isUIActive) return; // 如果 UI 关闭，不执行滑动条操作

        float joystickX = joystickInputAction.action.ReadValue<Vector2>().x; // 读取摇杆 X 轴输入
        if (Mathf.Abs(joystickX) > 0.1f) // 避免误触
        {
            frequencySlider.value += joystickX * sliderSpeed * Time.deltaTime;
        }
    }
}
