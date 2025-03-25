using UnityEngine;

public class TriggerShowUI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            // 找到场景中第一个带有 SliderTriggerUIDelay 的物体
            SliderTriggerUIDelay sliderScript = FindObjectOfType<SliderTriggerUIDelay>();

            if (sliderScript != null)
            {
                sliderScript.LockUVEffect();
                Debug.Log("触发器已触发，紫外线效果被锁定");
            }
            else
            {
                Debug.LogWarning("找不到 SliderTriggerUIDelay 脚本！");
            }
        }
    }
}
