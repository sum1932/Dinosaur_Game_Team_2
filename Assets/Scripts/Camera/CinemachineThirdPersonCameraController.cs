using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class CinemachineThirdPersonCameraController : MonoBehaviour
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
    [SerializeField] private bool snapPositionWhileRotating = true;

    [Header("Zoom")]
    [SerializeField] private float minDistance = 2.5f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float zoomSensitivity = 0.01f;
    [SerializeField] private float zoomSmoothTime = 0.08f;

    [Header("Cursor")]
    [SerializeField] private bool lockCursorWhileRotating = true;

    private Vector3 followVelocity;
    private float distanceVelocity;
    private float targetDistance;
    private float yaw;
    private float pitch = 20f;

    private void OnEnable()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        yaw = eulerAngles.y;
        pitch = eulerAngles.x;
        targetDistance = Mathf.Clamp(distance, minDistance, maxDistance);
        distance = targetDistance;

        UnlockCursor();
    }

    private void OnDisable()
    {
        UnlockCursor();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        bool isRotating = RotateFromMouse();
        ZoomFromMouse();
        FollowTarget(isRotating);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private bool RotateFromMouse()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            return false;
        }

        if (!mouse.rightButton.isPressed)
        {
            UnlockCursor();
            return false;
        }

        LockCursor();

        Vector2 mouseDelta = mouse.delta.ReadValue();
        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        return true;
    }

    private void ZoomFromMouse()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        float scrollY = mouse.scroll.ReadValue().y;
        if (Mathf.Abs(scrollY) > 0.001f)
        {
            targetDistance = Mathf.Clamp(
                targetDistance - scrollY * zoomSensitivity,
                minDistance,
                maxDistance);
        }

        distance = Mathf.SmoothDamp(
            distance,
            targetDistance,
            ref distanceVelocity,
            zoomSmoothTime);
    }

    private void FollowTarget(bool isRotating)
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = target.position + targetOffset;
        Vector3 desiredPosition = focusPoint - rotation * Vector3.forward * distance;

        if (isRotating && snapPositionWhileRotating)
        {
            followVelocity = Vector3.zero;
            transform.SetPositionAndRotation(desiredPosition, rotation);
            return;
        }

        Vector3 smoothedPosition = followSmoothTime <= 0f
            ? desiredPosition
            : Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref followVelocity,
                followSmoothTime);

        transform.SetPositionAndRotation(smoothedPosition, rotation);
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
}
