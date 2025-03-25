using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class SlideOnLine : MonoBehaviour
{
    [Header("��������յ�")]
    public Transform startPoint;
    public Transform endPoint;

    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor interactor;

    private bool isGrabbed = false;
    private Vector3 lineDirection;
    private float lineLength;

    private Vector3 grabOffset; // ץ��λ��������λ��֮��ĳ�ʼƫ��

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // ����Ĭ���ֶ�����
        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;

        // ����¼�
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

        // ����������ͳ���
        lineDirection = (endPoint.position - startPoint.position).normalized;
        lineLength = Vector3.Distance(startPoint.position, endPoint.position);

        // ץסʱ����¼�ֲ�������ľ���ƫ�ƣ�����������
        Vector3 projected = ProjectPointOnLine(startPoint.position, lineDirection, interactor.transform.position);
        grabOffset = transform.position - projected;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        interactor = null;
        isGrabbed = false;

        // ץȡ���������ƶ����壬Ҳ����������
        // ����ϣ������������� Rigidbody ������ isKinematic=false
    }

    private void Update()
    {
        if (isGrabbed && interactor != null)
        {
            // ���ֵ�λ��ͶӰ�������
            Vector3 projectedPoint = ProjectPointOnLine(startPoint.position, lineDirection, interactor.transform.position);
            float distance = Vector3.Dot(projectedPoint - startPoint.position, lineDirection);
            distance = Mathf.Clamp(distance, 0f, lineLength);

            // ����Ŀ��λ�ã���� + ���� * ���ƾ��� + ץȡʱƫ��
            Vector3 constrainedPosition = startPoint.position + lineDirection * distance + grabOffset;
            transform.position = constrainedPosition;
        }
    }

    // ��һ����ͶӰ��ָ���߶���
    private Vector3 ProjectPointOnLine(Vector3 lineStart, Vector3 lineDir, Vector3 point)
    {
        Vector3 toPoint = point - lineStart;
        float dot = Vector3.Dot(toPoint, lineDir);
        return lineStart + lineDir * dot;
    }

    // ���ӻ����
    private void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
        }
    }
}
