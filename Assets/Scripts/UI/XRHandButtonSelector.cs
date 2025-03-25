using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class JoystickUIController : MonoBehaviour
{
    [Header("UI 控制")]
    public GameObject uiPanel; // UI 面板（X 键切换显示/隐藏）
    public Button[] buttons; // UI 按钮数组（从上到下）
    public Text displayText; // UI 文字
    public Image displayImage; // UI 图片
    public Sprite[] buttonSprites; // 对应按钮的图片
    [TextArea] public string[] buttonTexts; // 对应按钮的文字
    public Image uvModeImage; // 紫外线模式解锁后显示的 UI 图片
    public Button uvModeButton; // 紫外线模式解锁后显示的 UI 按钮

    [Header("手柄输入")]
    public InputActionReference toggleUIAction; // X 键控制 UI 显示/隐藏
    public InputActionReference joystickInputAction; // 左摇杆输入
    public float yThreshold = 0.5f; // 摇杆 Y 轴触发的最小值（避免误触）

    private bool isUIActive = false;
    private int currentIndex = 0; // 当前按钮索引
    private bool canMove = true; // 控制是否允许再次移动
    private DisableMovementController movementController; // 移动控制器

    private void Start()
    {
        // UI 默认隐藏
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }

        // UV 模式的 UI 组件默认隐藏
        if (uvModeImage != null) uvModeImage.gameObject.SetActive(false);
        if (uvModeButton != null) uvModeButton.gameObject.SetActive(false);

        // 获取 DisableMovementController
        movementController = FindObjectOfType<DisableMovementController>();
        if (movementController == null)
        {
            Debug.LogError("找不到 DisableMovementController，请确保它挂在玩家对象上");
        }

        // 监听 X 键
        toggleUIAction.action.performed += ctx => ToggleUI();
    }

    private void OnDestroy()
    {
        // 取消监听，防止报错
        toggleUIAction.action.performed -= ctx => ToggleUI();
    }

    private void ToggleUI()
    {
        if (!ElectromagneticMode.isRadioModeUnlocked)
        {
            Debug.Log("无线电模式未解锁，无法打开 UI");
            return;
        }

        isUIActive = !isUIActive;

        if (isUIActive)
        {
            UIManager.Instance.ShowUI(uiPanel); // 使用 UIManager 控制 UI，避免冲突
        }
        else
        {
            UIManager.Instance.HideUI(uiPanel);
        }

        if (movementController != null)
        {
            if (isUIActive)
                movementController.DisableMovement();
            else
                movementController.EnableMovement();
        }

        Debug.Log($"UI 切换，当前状态: {isUIActive}");
    }

    private void Update()
    {
        if (!isUIActive || !canMove) return; // UI 关闭或冷却状态，不做处理

        CheckAndFixCurrentIndex(); // 避免选中隐藏按钮

        float joystickY = joystickInputAction.action.ReadValue<Vector2>().y;

        if (joystickY > yThreshold) // 上推
        {
            MoveUp();
            StartCoroutine(ResetMoveCooldown()); // 启动冷却，防止连续切换
        }
        else if (joystickY < -yThreshold) // 下推
        {
            MoveDown();
            StartCoroutine(ResetMoveCooldown()); // 启动冷却，防止连续切换
        }

        // 检查 UV 模式是否解锁，解锁后显示 UI 组件
        if (ElectromagneticMode.isUVModeUnlocked)
        {
            ShowUVModeUI();
        }
    }

    private IEnumerator ResetMoveCooldown()
    {
        canMove = false;
        yield return new WaitUntil(() => Mathf.Abs(joystickInputAction.action.ReadValue<Vector2>().y) < 0.2f);
        canMove = true;
    }

    private void MoveUp()
    {
        int nextIndex = currentIndex;
        if (GetVisibleButtonCount() <= 1) return;

        do
        {
            nextIndex--;
        } while (nextIndex >= 0 && !buttons[nextIndex].gameObject.activeSelf);

        if (nextIndex >= 0)
        {
            currentIndex = nextIndex;
            SelectButton(currentIndex);
        }
    }

    private void MoveDown()
    {
        int nextIndex = currentIndex;
        if (GetVisibleButtonCount() <= 1) return;

        do
        {
            nextIndex++;
        } while (nextIndex < buttons.Length && !buttons[nextIndex].gameObject.activeSelf);

        if (nextIndex < buttons.Length)
        {
            currentIndex = nextIndex;
            SelectButton(currentIndex);
        }
    }

    private void SelectButton(int index)
    {
        if (!buttons[index].gameObject.activeSelf) return;

        buttons[index].Select();
        Debug.Log($"当前选中按钮索引: {index}, 按钮名称: {buttons[index].name}");

        if (displayText != null && buttonTexts.Length > index)
        {
            displayText.text = buttonTexts[index];
        }

        if (displayImage != null && buttonSprites.Length > index)
        {
            displayImage.sprite = buttonSprites[index];
        }
    }

    private void CheckAndFixCurrentIndex()
    {
        if (!buttons[currentIndex].gameObject.activeSelf)
        {
            UpdateCurrentIndex();
        }
    }

    private void UpdateCurrentIndex()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].gameObject.activeSelf)
            {
                currentIndex = i;
                SelectButton(currentIndex);
                break;
            }
        }
    }

    private int GetVisibleButtonCount()
    {
        int count = 0;
        foreach (var btn in buttons)
        {
            if (btn.gameObject.activeSelf) count++;
        }
        return count;
    }

    private void ShowUVModeUI()
    {
        if (uvModeImage != null && !uvModeImage.gameObject.activeSelf)
        {
            uvModeImage.gameObject.SetActive(true);
            Debug.Log("紫外线模式 UI 图片已显示");
        }

        if (uvModeButton != null && !uvModeButton.gameObject.activeSelf)
        {
            uvModeButton.gameObject.SetActive(true);
            Debug.Log("紫外线模式 UI 按钮已显示");
        }
    }
}
