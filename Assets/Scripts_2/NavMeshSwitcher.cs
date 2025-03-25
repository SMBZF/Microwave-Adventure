using UnityEngine;
using UnityEngine.AI;

public class NavMeshSwitcher : MonoBehaviour
{
    public Transform targetPosition;  // ��ɫҪ���͵�Ŀ���
    public Transform targetPosition2;
    public Transform targetPosition3;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tele1")) // ȷ�����͵��� "Portal" ��ǩ
        {
            agent.Warp(targetPosition.position); // ֱ����ת����һ�� NavMesh
        }
        if (other.CompareTag("Tele2")) // ȷ�����͵��� "Portal" ��ǩ
        {
            agent.Warp(targetPosition2.position); // ֱ����ת����һ�� NavMesh
        }
        if (other.CompareTag("Tele3")) // ȷ�����͵��� "Portal" ��ǩ
        {
            agent.Warp(targetPosition3.position); // ֱ����ת����һ�� NavMesh
        }
    }
}
