using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private Transform cameraTransform;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedStickForce = -2f;

    public event Action InteractPressed;

    private CharacterController characterController;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        Move(keyboard);
        Jump(keyboard);
        ApplyGravity();
        CheckInteractionInput(keyboard);
    }

    private void Move(Keyboard keyboard)
    {
        Vector2 moveInput = ReadMoveInput(keyboard);
        Vector3 moveDirection = GetCameraRelativeDirection(moveInput);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private Vector2 ReadMoveInput(Keyboard keyboard)
    {
        Vector2 input = Vector2.zero;

        if (keyboard.wKey.isPressed)
        {
            input.y += 1f;
        }

        if (keyboard.sKey.isPressed)
        {
            input.y -= 1f;
        }

        if (keyboard.dKey.isPressed)
        {
            input.x += 1f;
        }

        if (keyboard.aKey.isPressed)
        {
            input.x -= 1f;
        }

        return input.sqrMagnitude > 1f ? input.normalized : input;
    }

    private Vector3 GetCameraRelativeDirection(Vector2 input)
    {
        if (input.sqrMagnitude <= 0.001f)
        {
            return Vector3.zero;
        }

        Vector3 forward = cameraTransform != null ? cameraTransform.forward : Vector3.forward;
        Vector3 right = cameraTransform != null ? cameraTransform.right : Vector3.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return (forward * input.y + right * input.x).normalized;
    }

    private void Jump(Keyboard keyboard)
    {
        if (!keyboard.spaceKey.wasPressedThisFrame || !characterController.isGrounded)
        {
            return;
        }

        verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = groundedStickForce;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private void CheckInteractionInput(Keyboard keyboard)
    {
        if (!keyboard.eKey.wasPressedThisFrame)
        {
            return;
        }

        InteractPressed?.Invoke();
        Debug.Log("상호작용 입력 감지, 이벤트 발생");
    }
}
