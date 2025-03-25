using UnityEngine;

public class DynamicSoundGuide : MonoBehaviour
{
    public Transform xrOrigin; // �� XR Origin (XR Rig)
    public Transform leftHand; // ���ֵ� Transform
    public Transform rightHand; // ���ֵ� Transform
    public AudioSource audioSource; // ����Ƶ
    public float maxVolume = 1.0f; // �������
    public float minVolume = 0.1f; // ��С����
    public float maxDistance = 20f; // ������󴫲�����
    public bool useLeftHand = true; // ѡ�������ֻ�������Ϊ����ο�

    private Transform referenceHand;

    private void Start()
    {
        // ѡ���ĸ�����Ϊ�ο�
        referenceHand = useLeftHand ? leftHand : rightHand;

        if (xrOrigin == null)
        {
            Debug.LogError("���� Inspector �� XR Origin (XR Rig)");
        }
        if (audioSource == null)
        {
            Debug.LogError("���� Inspector �� AudioSource");
        }
    }

    private void Update()
    {
        if (xrOrigin == null || referenceHand == null || audioSource == null) return;

        // ������ң�XR Origin���������ľ���
        float distance = Vector3.Distance(xrOrigin.position, transform.position);

        // �������������ݾ���˥����
        float volume = Mathf.Lerp(minVolume, maxVolume, 1 - (distance / maxDistance));
        audioSource.volume = Mathf.Clamp(volume, minVolume, maxVolume);

        // �����ֳ��������ĽǶ�
        Vector3 directionToSound = (transform.position - referenceHand.position).normalized;
        float angle = Vector3.Angle(referenceHand.forward, directionToSound);

        // �������ֳ�����������������
        if (angle < 30f)
        {
            audioSource.volume *= 1.2f;
        }

        // �������ֱ�����������������
        if (angle > 150f)
        {
            audioSource.volume *= 0.5f;
        }
    }
}
