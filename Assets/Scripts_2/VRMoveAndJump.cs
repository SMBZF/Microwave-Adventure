using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRMoveAndJump : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2.0f;       // 移动速度
    public float jumpHeight = 1.5f;      // 跳跃高度
    public float gravity = -9.81f;       // 重力

    [Header("Input Actions")]
    public InputActionProperty moveAction;   // 摇杆移动
    public InputActionProperty jumpAction;   // 跳跃按钮

    [Header("References")]
    public Transform headTransform;      // 摄像机 Transform（头部朝向）

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpQueued = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // 如果没在 Inspector 指定 headTransform，自动使用主摄像机
        if (headTransform == null && Camera.main != null)
        {
            headTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        // 检测跳跃输入
        if (jumpAction.action.WasPressedThisFrame() && isGrounded)
        {
            jumpQueued = true;
        }

        // 读取摇杆输入
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // 以头部方向为参考来计算移动方向
        Vector3 headForward = headTransform.forward;
        headForward.y = 0;
        headForward.Normalize();

        Vector3 headRight = headTransform.right;
        headRight.y = 0;
        headRight.Normalize();

        Vector3 moveDirection = headForward * inputDirection.z + headRight * inputDirection.x;
        Vector3 horizontalMove = moveDirection * moveSpeed;

        // 更新水平速度
        velocity.x = horizontalMove.x;
        velocity.z = horizontalMove.z;

        // 实际移动角色
        characterController.Move(velocity * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // 处理跳跃
        if (isGrounded && jumpQueued)
        {
            Jump();
            jumpQueued = false;
        }

        // 应用重力
        velocity.y += gravity * Time.fixedDeltaTime;
    }

    void Jump()
    {
        // 计算初始跳跃速度（基于公式 v = sqrt(2gh)）
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
