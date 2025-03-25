using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class SlideOnLine : MonoBehaviour
{
    [Header("轨道起点和终点")]
    public Transform startPoint;
    public Transform endPoint;

    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor interactor;

    private bool isGrabbed = false;
    private Vector3 lineDirection;
    private float lineLength;

    private Vector3 grabOffset; // 抓的位置与物体位置之间的初始偏移

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // 禁用默认手动跟随
        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;

        // 添加事件
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject as XRBaseInteractor;
        isGrabbed = true;

        // 计算轨道方向和长度
        lineDirection = (endPoint.position - startPoint.position).normalized;
        lineLength = Vector3.Distance(startPoint.position, endPoint.position);

        // 抓住时，记录手部与物体的距离偏移（用于修正）
        Vector3 projected = ProjectPointOnLine(startPoint.position, lineDirection, interactor.transform.position);
        grabOffset = transform.position - projected;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        interactor = null;
        isGrabbed = false;

        // 抓取结束后不再移动物体，也不交给物理
        // 如你希望交给物理，请加 Rigidbody 并设置 isKinematic=false
    }

    private void Update()
    {
        if (isGrabbed && interactor != null)
        {
            // 把手的位置投影到轨道上
            Vector3 projectedPoint = ProjectPointOnLine(startPoint.position, lineDirection, interactor.transform.position);
            float distance = Vector3.Dot(projectedPoint - startPoint.position, lineDirection);
            distance = Mathf.Clamp(distance, 0f, lineLength);

            // 最终目标位置：起点 + 方向 * 限制距离 + 抓取时偏移
            Vector3 constrainedPosition = startPoint.position + lineDirection * distance + grabOffset;
            transform.position = constrainedPosition;
        }
    }

    // 将一个点投影到指定线段上
    private Vector3 ProjectPointOnLine(Vector3 lineStart, Vector3 lineDir, Vector3 point)
    {
        Vector3 toPoint = point - lineStart;
        float dot = Vector3.Dot(toPoint, lineDir);
        return lineStart + lineDir * dot;
    }

    // 可视化轨道
    private void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
        }
    }
}
