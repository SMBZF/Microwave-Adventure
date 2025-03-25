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
                Debug.Log("X 射线锁定触发器已被触发");
            }
            else
            {
                Debug.LogWarning("找不到 SliderTriggerUIDelay 脚本实例！");
            }
        }
    }
}
