using UnityEngine;
using UnityEngine.UI;

public class AirWall : MonoBehaviour
{
    public GameObject uiCanvas; // UI 面板
    public Text uiText; // UI 文本组件
    public string message = "There is a tiny little battery around you, please pick it up!!!\n(We will find a better way to make u find it in the future)"; // UI 显示的文本

    private void Start()
    {
        // UI 默认隐藏
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 只有 Left Hand 或 Right Hand 碰撞才触发 UI
        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            ShowMessage();
            Debug.Log($"AirWall triggered by: {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 当 Left Hand 或 Right Hand 离开时隐藏 UI
        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            HideMessage();
            Debug.Log($"AirWall exited by: {other.gameObject.name}");
        }
    }

    private void ShowMessage()
    {
        if (uiCanvas != null && uiText != null)
        {
            uiCanvas.SetActive(true);
            uiText.text = message;
        }
    }

    private void HideMessage()
    {
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }
    }
}
