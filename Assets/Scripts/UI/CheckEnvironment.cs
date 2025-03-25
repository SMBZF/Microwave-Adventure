using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckEnvironment : MonoBehaviour
{
    public GameObject uiPanel; // �� UI ����
    public float displayTime = 3f; // ��ʾʱ�䣨�룩

    private Coroutine hideCoroutine;

    private void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // ��ʼ���� UI
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            //Debug.Log("��⵽�֣�" + other.gameObject.name);
            ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            //Debug.Log("�����뿪��" + other.gameObject.name);
            HideUI();
        }
    }

    private void ShowUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            //Debug.Log("UI ��ʾ");
        }

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(AutoHideUI());
    }

    private IEnumerator AutoHideUI()
    {
        yield return new WaitForSeconds(displayTime);
        HideUI();
    }

    private void HideUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }
    }
}
