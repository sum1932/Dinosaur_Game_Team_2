using UnityEngine;

[DefaultExecutionOrder(100)]
public class BillboardToCamera : MonoBehaviour
{
    public enum BillboardMode
    {
        YAxis,
        Full
    }

    [Header("Billboard")]
    [SerializeField] private BillboardMode mode = BillboardMode.YAxis;
    [SerializeField] private Transform targetCamera;
    [SerializeField] private bool invertForward;

    private void LateUpdate()
    {
        Transform cameraTransform = GetCameraTransform();
        if (cameraTransform == null)
        {
            return;
        }

        Vector3 lookDirection = transform.position - cameraTransform.position;
        if (invertForward)
        {
            lookDirection = -lookDirection;
        }

        if (mode == BillboardMode.YAxis)
        {
            lookDirection.y = 0f;
        }

        if (lookDirection.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
    }

    private Transform GetCameraTransform()
    {
        if (targetCamera != null)
        {
            FindAnyObjectByType<Camera>();
            return targetCamera;
        }

        Camera mainCamera = Camera.main;
        return mainCamera != null ? mainCamera.transform : null;
    }
}
