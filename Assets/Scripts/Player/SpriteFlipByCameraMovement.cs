using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[DefaultExecutionOrder(110)]
public class SpriteFlipByCameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform movementRoot;
    [SerializeField] private Transform cameraTransform;

    [Header("Flip")]
    [SerializeField] private bool flipWhenMovingRight;
    [SerializeField] private float deadZone = 0.01f;

    private SpriteRenderer spriteRenderer;
    private Vector3 previousRootPosition;
    private bool hasPreviousPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (movementRoot == null)
        {
            movementRoot = transform.root;
        }

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (movementRoot == null)
        {
            return;
        }

        if (!hasPreviousPosition)
        {
            previousRootPosition = movementRoot.position;
            hasPreviousPosition = true;
            return;
        }

        Vector3 movement = movementRoot.position - previousRootPosition;
        previousRootPosition = movementRoot.position;

        movement.y = 0f;
        if (movement.sqrMagnitude <= deadZone * deadZone)
        {
            return;
        }

        Vector3 right = cameraTransform != null ? cameraTransform.right : Vector3.right;
        right.y = 0f;

        if (right.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        float sideAmount = Vector3.Dot(movement.normalized, right.normalized);
        if (Mathf.Abs(sideAmount) <= deadZone)
        {
            return;
        }

        bool movingRight = sideAmount > 0f;
        spriteRenderer.flipX = flipWhenMovingRight ? movingRight : !movingRight;
    }
}
