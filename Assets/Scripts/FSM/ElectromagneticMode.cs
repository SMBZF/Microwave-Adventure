using UnityEngine;
using UnityEngine.UI;

public class ElectromagneticMode : MonoBehaviour
{
    public static bool isUVModeUnlocked = false;  // UV ģʽ�Ƿ����
    public static bool isXRayModeUnlocked = false; // X ����ģʽ�Ƿ����
    public static bool isRadioModeUnlocked = true; // ���ߵ�ģʽĬ�Ͽ���

    public GameObject frequencyUI; // Ƶ��ͼ UI ���
    public Text modeText; // ��ʾ��ǰģʽ�� UI �ı�
    public Image modeIcon; // ģʽͼ��
    public Sprite uvIcon, xrayIcon, radioIcon; // ����ͬģʽ��ͼ��

    //private void Start()
    //{
    //    UpdateUI(); // ȷ�� UI ��ʼ״̬
    //}

    public void UnlockUVMode()
    {
        isUVModeUnlocked = true;
        Debug.Log("UV ģʽ�ѽ�����");
    }

    public void UnlockXRayMode()
    {
        isXRayModeUnlocked = true;
        Debug.Log("X-ray ģʽ�ѽ�����");
    }

    public void UnlockRadioMode()
    {
        isRadioModeUnlocked = true;
        Debug.Log("���ߵ�ģʽ�ѽ�����");
    }


}
