using UnityEngine;
using UnityEngine.UI;

public class UIFollow3DObject : MonoBehaviour
{
    public Transform mainCam;
    public Transform target;
    public Transform worldSpaceCanvas;
    public Vector3 offset;


    private void Start()
    {
        mainCam = Camera.main.transform;
        transform.SetParent(worldSpaceCanvas);

    }
    void Update()
    {
        // ���� UI ���������
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position); // look at camera

        // ���� UI ��λ��
        transform.position = target.position + offset;

    }
}