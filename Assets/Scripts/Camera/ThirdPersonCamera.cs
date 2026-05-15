using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.6f, 0f);

    [Header("Orbit")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;
    [SerializeField] private float followSmoothTime = 0.05f;

    [Header("Cursor")]
    [SerializeField] private bool lockCursorWhileRotating = true;

    private Vector3 followVelocity;
    private float yaw;
    private float pitch = 20f;

    private void Start()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        yaw = eulerAngles.y;
        pitch = eulerAngles.x;

        UnlockCursor();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        RotateFromMouse();
        FollowTarget();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void RotateFromMouse()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        if (!mouse.rightButton.isPressed)
        {
            UnlockCursor();
            return;
        }

        LockCursor();

        Vector2 mouseDelta = mouse.delta.ReadValue();
        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void LockCursor()
    {
        if (!lockCursorWhileRotating)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        if (!lockCursorWhileRotating)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = target.position + targetOffset;
        Vector3 desiredPosition = focusPoint - rotation * Vector3.forward * distance;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref followVelocity,
            followSmoothTime);

        transform.rotation = rotation;
    }
}
