using UnityEngine;
using System.Collections;

public class TeleportTrigger : MonoBehaviour
{
    public Transform teleportDestination; // ����Ŀ��λ��

    private void OnTriggerEnter(Collider other)
    {
        GameObject rootObject = other.transform.root.gameObject;
        if (rootObject.CompareTag("Player"))
        {
            Debug.Log("����: " + rootObject.name);

            CharacterController controller = rootObject.GetComponent<CharacterController>();
            if (controller != null)
            {
                Debug.Log("���");
                controller.enabled = false; // �Ƚ��ý�ɫ����������ֹ Unity ����ϵͳ��ֹλ�ñ仯
                rootObject.transform.position = teleportDestination.position; // ���ͽ�ɫ
                StartCoroutine(EnableCharacterController(controller)); // �ӳ���������
            }
            else
            {
                rootObject.transform.position = teleportDestination.position;
            }
        }
    }

    private IEnumerator EnableCharacterController(CharacterController controller)
    {
        yield return new WaitForSeconds(0.05f); // �ȴ�һ֡
        controller.enabled = true; // �������� CharacterController
        Debug.Log("CharacterController ����������");
    }
}
