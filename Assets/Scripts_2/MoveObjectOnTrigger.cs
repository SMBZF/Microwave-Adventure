using UnityEngine;

public class MoveObjectOnTrigger : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform targetObject;       // 需要移动的物体
    public float moveDistance = 2.0f;    // Y轴移动的距离
    public float moveDuration = 1.0f;    // 移动持续时间（秒）

    [Header("Trigger Settings")]
    public string triggeringTag = "Player"; // 触发的标签（默认"Player"）

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
            Debug.Log("[MoveObjectOnTrigger] 初始位置: " + originalPosition);
            Debug.Log("[MoveObjectOnTrigger] 目标位置: " + targetPosition);
        }
        else
        {
            Debug.LogWarning("[MoveObjectOnTrigger] 请在 Inspector 中设置 targetObject！");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("触发器");
        // 判断标签是否匹配
        if (!isMoving && targetObject != null && other.CompareTag(triggeringTag))
        {
            Debug.Log("[MoveObjectOnTrigger] 触发器被 " + other.name + " 触发！");
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

            Debug.Log("[MoveObjectOnTrigger] 正在移动... t=" + t.ToString("F2"));

            if (t >= 1f)
            {
                isMoving = false;
                Debug.Log("[MoveObjectOnTrigger] 移动完成！");
            }
        }
    }
}
