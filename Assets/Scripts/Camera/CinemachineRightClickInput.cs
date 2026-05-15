using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[AddComponentMenu("Cinemachine/Helpers/Cinemachine Right Click Input")]
public class CinemachineRightClickInput : InputAxisControllerBase<CinemachineRightClickInput.Reader>
{
    [Header("Cursor")]
    [SerializeField] private bool requireRightClick = true;
    [SerializeField] private bool lockCursorWhileRotating = true;

    [Header("Sensitivity")]
    [SerializeField] private float horizontalSensitivity = 3f;
    [SerializeField] private float verticalSensitivity = 3f;

    protected override void OnDisable()
    {
        base.OnDisable();
        UnlockCursor();
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        if (requireRightClick && !mouse.rightButton.isPressed)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }

        if (Application.isPlaying)
        {
            UpdateControllers();
        }
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

    [Serializable]
    public class Reader : IInputAxisReader
    {
        [SerializeField] private bool requireRightClick = true;
        [SerializeField] private float horizontalSensitivity = 3f;
        [SerializeField] private float verticalSensitivity = 3f;

        public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint)
        {
            CinemachineRightClickInput owner = null;
            if (context is Component component)
            {
                owner = component.GetComponent<CinemachineRightClickInput>();
            }

            Mouse mouse = Mouse.current;
            if (mouse == null)
            {
                return 0f;
            }

            bool needsRightClick = owner != null ? owner.requireRightClick : requireRightClick;
            if (needsRightClick && !mouse.rightButton.isPressed)
            {
                return 0f;
            }

            float xSensitivity = owner != null ? owner.horizontalSensitivity : horizontalSensitivity;
            float ySensitivity = owner != null ? owner.verticalSensitivity : verticalSensitivity;

            Vector2 mouseDelta = mouse.delta.ReadValue();
            return hint == IInputAxisOwner.AxisDescriptor.Hints.Y
                ? -mouseDelta.y * ySensitivity
                : mouseDelta.x * xSensitivity;
        }
    }
}
