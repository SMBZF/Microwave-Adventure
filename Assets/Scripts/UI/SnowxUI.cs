using UnityEngine;

public class XRayLockTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            SliderTriggerUIDelay controller = FindObjectOfType<SliderTriggerUIDelay>();
            if (controller != null)
            {
                controller.LockXRayEffect();
                Debug.Log("X ���������������ѱ�����");
            }
            else
            {
                Debug.LogWarning("�Ҳ��� SliderTriggerUIDelay �ű�ʵ����");
            }
        }
    }
}
