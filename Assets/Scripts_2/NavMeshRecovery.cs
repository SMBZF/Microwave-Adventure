using UnityEngine;
using UnityEngine.AI;

public class NavMeshRecovery : MonoBehaviour
{
    public float checkInterval = 0.6f; // 多久检查一次（避免每帧浪费）
    public float sampleRadius = 5.0f;  // 检测 NavMesh 的半径
    public float fallThreshold = -10f; // 掉太下面就强制传送回来

    private Vector3 lastSafePosition;
    private float checkTimer;

    void Start()
    {
        lastSafePosition = transform.position;
    }

    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;

            // 如果玩家掉得太深了（比如掉出地图）
            if (transform.position.y < fallThreshold)
            {
                RecoverToLastSafePosition();
                return;
            }

            if (IsOnNavMesh(transform.position))
            {
                lastSafePosition = transform.position;
            }
            else
            {
                RecoverToLastSafePosition();
            }
        }
    }

    void RecoverToLastSafePosition()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(lastSafePosition, out hit, sampleRadius, NavMesh.AllAreas))
        {
            Debug.Log("🚀 玩家脱离导航区域，传送回最近合法位置");
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // 禁用再启用，避免 Move() 和 Teleport 冲突
                transform.position = hit.position;
                cc.enabled = true;
            }
            else
            {
                transform.position = hit.position;
            }
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到可用的 NavMesh 点，无法恢复！");
        }
    }

    bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, 2f, NavMesh.AllAreas);
    }
}

