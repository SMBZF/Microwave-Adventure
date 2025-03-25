using UnityEngine;

public class TriggerShowUI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            // �ҵ������е�һ������ SliderTriggerUIDelay ������
            SliderTriggerUIDelay sliderScript = FindObjectOfType<SliderTriggerUIDelay>();

            if (sliderScript != null)
            {
                sliderScript.LockUVEffect();
                Debug.Log("�������Ѵ�����������Ч��������");
            }
            else
            {
                Debug.LogWarning("�Ҳ��� SliderTriggerUIDelay �ű���");
            }
        }
    }
}
