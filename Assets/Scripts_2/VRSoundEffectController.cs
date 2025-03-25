using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedContinuousMoveProvider))]
public class VRSoundEffectController : MonoBehaviour
{
    public AudioSource audioSource;  // ��ǰ�źã����⶯̬����
    public AudioClip[] footstepClips;  // ��ͬ������Ч
    public float stepInterval = 0.5f;

    private ActionBasedContinuousMoveProvider moveProvider;
    private float stepTimer;
    private Vector2 lastMoveInput;

    void Start()
    {
        moveProvider = GetComponent<ActionBasedContinuousMoveProvider>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Ԥ����
        if (footstepClips.Length > 0)
        {
            audioSource.clip = footstepClips[0];  // Ĭ��ѡһ���������������л�
        }
    }

    void Update()
    {
        Vector2 moveInput = moveProvider.leftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
        moveInput += moveProvider.rightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

        bool isMoving = moveInput.magnitude > 0.1f;

        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        lastMoveInput = moveInput;
    }

    void PlayFootstepSound()
    {
        if (footstepClips.Length > 0)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.PlayOneShot(clip);  // ���ٲ���
        }
    }
}
