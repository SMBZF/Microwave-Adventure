using UnityEngine;
using UnityEngine.UI;

public class AirWall : MonoBehaviour
{
    public GameObject uiCanvas; // UI ���
    public Text uiText; // UI �ı����
    public string message = "There is a tiny little battery around you, please pick it up!!!\n(We will find a better way to make u find it in the future)"; // UI ��ʾ���ı�

    private void Start()
    {
        // UI Ĭ������
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ֻ�� Left Hand �� Right Hand ��ײ�Ŵ��� UI
        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            ShowMessage();
            Debug.Log($"AirWall triggered by: {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �� Left Hand �� Right Hand �뿪ʱ���� UI
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
