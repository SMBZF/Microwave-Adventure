using UnityEngine;

public class MoveObjectOnTrigger : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform targetObject;       // ��Ҫ�ƶ�������
    public float moveDistance = 2.0f;    // Y���ƶ��ľ���
    public float moveDuration = 1.0f;    // �ƶ�����ʱ�䣨�룩

    [Header("Trigger Settings")]
    public string triggeringTag = "Player"; // �����ı�ǩ��Ĭ��"Player"��

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float elapsedTime = 0f;

    void Start()
    {
        if (targetObject != null)
        {
            originalPosition = targetObject.position;
            targetPosition = originalPosition + Vector3.up * moveDistance;
            Debug.Log("[MoveObjectOnTrigger] ��ʼλ��: " + originalPosition);
            Debug.Log("[MoveObjectOnTrigger] Ŀ��λ��: " + targetPosition);
        }
        else
        {
            Debug.LogWarning("[MoveObjectOnTrigger] ���� Inspector ������ targetObject��");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("������");
        // �жϱ�ǩ�Ƿ�ƥ��
        if (!isMoving && targetObject != null && other.CompareTag(triggeringTag))
        {
            Debug.Log("[MoveObjectOnTrigger] �������� " + other.name + " ������");
            isMoving = true;
            elapsedTime = 0f;
        }
    }

    void Update()
    {
        if (isMoving && targetObject != null)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            targetObject.position = Vector3.Lerp(originalPosition, targetPosition, t);

            Debug.Log("[MoveObjectOnTrigger] �����ƶ�... t=" + t.ToString("F2"));

            if (t >= 1f)
            {
                isMoving = false;
                Debug.Log("[MoveObjectOnTrigger] �ƶ���ɣ�");
            }
        }
    }
}
