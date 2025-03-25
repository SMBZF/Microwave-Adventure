using UnityEngine;
using UnityEngine.AI;

public class NavMeshChecker : MonoBehaviour
{
    public float sampleRadius = 1.0f;

    void Update()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, sampleRadius, NavMesh.AllAreas))
        {
            Debug.DrawLine(transform.position, hit.position, Color.green);
            Debug.Log("✅ 当前在 NavMesh 上");
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.up * 2f, Color.red);
            Debug.LogWarning("❌ 不在 NavMesh 上！");
        }
        var tri = NavMesh.CalculateTriangulation();
        Debug.Log("NavMesh 三角形数量：" + tri.indices.Length / 3);

    }
}
