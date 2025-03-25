using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ���� UI ���

public class CodeCheck : MonoBehaviour
{
    public Renderer[] targetObjects; // �ĸ�Ŀ�����壨��Renderer�Ķ���
    public Texture2D[] alphaTextures; // 9��Alpha��ͼ����˳����1��9
    public string correctCode = "1234"; // Ԥ����ȷ��4λ����
    public Animator targetAnimator1;
    public Animator targetAnimator2;

    public GameObject uiPanel; // UI ��壨�����ı���
    public Text uiText; // UI ��ʾ���ı�
    public float uiDisplayTime = 5f; // UI ��ʾ��ʱ�䣨3����Զ����أ�

    private Dictionary<Texture, int> textureToNumber; // ��ͼ����ŵ�ӳ��
    private bool hasUnlocked = false; // ��ֹ�ظ����� UI

    void Start()
    {
        // ��ʼ����ͼ���ӳ��
        textureToNumber = new Dictionary<Texture, int>();
        for (int i = 0; i < alphaTextures.Length; i++)
        {
            textureToNumber[alphaTextures[i]] = i + 1; // ��ͼ1��Ӧ���1���Դ�����
        }

        // ȷ�� UI Ĭ������
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    public void CheckCode()
    {
        if (targetObjects.Length != 4)
        {
            Debug.LogError("������4��Ŀ�����壡");
            return;
        }

        string currentCode = "";

        foreach (Renderer renderer in targetObjects)
        {
            Material mat = renderer.material;
            Texture currentAlphaTex = mat.GetTexture("_Alpha");

            if (textureToNumber.TryGetValue(currentAlphaTex, out int number))
            {
                currentCode += number.ToString();
            }
            else
            {
                Debug.LogError($"���� {renderer.gameObject.name} ʹ�õ�Alpha��ͼ����Ԥ���б��");
                return;
            }
        }

        //Debug.Log("��ǰ��ϱ�ţ�" + currentCode);

        if (currentCode == correctCode && !hasUnlocked)
        {
            Debug.Log("Access Granted");
            targetAnimator1.enabled = true;
            targetAnimator2.enabled = true;

            // ���� UI ��ʾ
            ShowSuccessUI();

            hasUnlocked = true; // ��ֹ�ظ�����
        }
        //else
        //{
        //   Debug.Log("�������");
        // }
    }

    private void ShowSuccessUI()
    {
        if (uiPanel != null && uiText != null)
        {
            uiPanel.SetActive(true);
            uiText.text = "Access Granted"; // ��ʾ "������ȷ"
            Debug.Log("������ȷ - UI ��ʾ");

            // 3 ������� UI
            StartCoroutine(HideUIAfterDelay());
        }
    }

    private IEnumerator HideUIAfterDelay()
    {
        yield return new WaitForSeconds(uiDisplayTime);
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            Debug.Log("������ȷ - UI ����");
        }
    }

    private void Update()
    {
        CheckCode();
    }
}
