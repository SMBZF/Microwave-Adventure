using UnityEngine;
using UnityEngine.AI;

public class NavMeshSwitcher : MonoBehaviour
{
    public Transform targetPosition;  // 角色要传送的目标点
    public Transform targetPosition2;
    public Transform targetPosition3;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tele1")) // 确保传送点有 "Portal" 标签
        {
            agent.Warp(targetPosition.position); // 直接跳转到另一个 NavMesh
        }
        if (other.CompareTag("Tele2")) // 确保传送点有 "Portal" 标签
        {
            agent.Warp(targetPosition2.position); // 直接跳转到另一个 NavMesh
        }
        if (other.CompareTag("Tele3")) // 确保传送点有 "Portal" 标签
        {
            agent.Warp(targetPosition3.position); // 直接跳转到另一个 NavMesh
        }
    }
}
