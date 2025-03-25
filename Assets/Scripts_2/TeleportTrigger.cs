using UnityEngine;
using System.Collections;

public class TeleportTrigger : MonoBehaviour
{
    public Transform teleportDestination; // 传送目标位置

    private void OnTriggerEnter(Collider other)
    {
        GameObject rootObject = other.transform.root.gameObject;
        if (rootObject.CompareTag("Player"))
        {
            Debug.Log("传送: " + rootObject.name);

            CharacterController controller = rootObject.GetComponent<CharacterController>();
            if (controller != null)
            {
                Debug.Log("检测");
                controller.enabled = false; // 先禁用角色控制器，防止 Unity 物理系统阻止位置变化
                rootObject.transform.position = teleportDestination.position; // 传送角色
                StartCoroutine(EnableCharacterController(controller)); // 延迟重新启用
            }
            else
            {
                rootObject.transform.position = teleportDestination.position;
            }
        }
    }

    private IEnumerator EnableCharacterController(CharacterController controller)
    {
        yield return new WaitForSeconds(0.05f); // 等待一帧
        controller.enabled = true; // 重新启用 CharacterController
        Debug.Log("CharacterController 已重新启用");
    }
}
