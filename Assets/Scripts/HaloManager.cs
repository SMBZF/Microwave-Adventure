using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HaloSequence : MonoBehaviour
{
    public List<GameObject> halos; // �洢��Ȧ���б�
    public GameObject uiPanel; // UI ���
    public UnityEngine.UI.Text uiText; // UI �ı����
    public float messageDuration = 3f; // UI ��ʾʱ��

    private int currentIndex = -1; // Ĭ�ϲ������κι�Ȧ
    private readonly string[] haloMessages =
    {
        "Finally, I got a radio signal! I can use it to navigate.",
        "Which way is right? Let's take a closer look.",
        "The halo is getting brighter, the signal stronger. I'm getting closer!",
        "High ground can reflect signals, causing 'dead zones'. I have to find my way now."
    };

    private void Start()
    {
        // ��ʼ��ʱ���й�Ȧ������
        foreach (GameObject halo in halos)
        {
            halo.SetActive(false);
        }
        if (uiPanel != null) uiPanel.SetActive(false);
    }

    /// <summary>
    /// �� Slider ���Ƽ����һ����Ȧ
    /// </summary>
    public void ActivateFirstHalo()
    {
        if (halos.Count > 0 && currentIndex == -1)
        {
            currentIndex = 0;
            halos[currentIndex].SetActive(true);
            Debug.Log("First halo activated by slider.");

            // **ǿ�ƴ��� Collider ���**
            Collider haloCollider = halos[currentIndex].GetComponent<Collider>();
            if (haloCollider != null)
            {
                haloCollider.enabled = false; // �Ƚ���
                haloCollider.enabled = true;  // �����ã�ȷ�������������
            }
        }
    }

    private void Update()
    {
        if (currentIndex >= 0 && currentIndex < halos.Count)
        {
            Collider[] colliders = Physics.OverlapSphere(halos[currentIndex].transform.position, 1f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Left Hand") || collider.CompareTag("Right Hand"))
                {
                    Debug.Log("Halo detected hand: " + collider.gameObject.name);
                    AdvanceHaloSequence();
                    break;
                }
            }
        }
    }

    private void AdvanceHaloSequence()
    {
        if (currentIndex >= 0 && currentIndex < halos.Count)
        {
            Debug.Log("Halo " + currentIndex + " triggered.");
            halos[currentIndex].SetActive(false);
            ShowMessage(currentIndex);

            currentIndex++;
            if (currentIndex < halos.Count)
            {
                halos[currentIndex].SetActive(true);
                Debug.Log("Next halo activated: " + currentIndex);
            }
            else
            {
                Debug.Log("All halos completed.");
            }
        }
    }

    private void ShowMessage(int index)
    {
        if (uiPanel != null && uiText != null && index < haloMessages.Length)
        {
            uiPanel.SetActive(true);
            uiText.text = haloMessages[index];
            Debug.Log("UI Message: " + haloMessages[index]);
            StartCoroutine(HideMessageAfterDelay());
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        HideMessage();
    }

    private void HideMessage()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }
}
