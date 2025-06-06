using System;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    #region PlayerSettings
    public float PlayerSpeed { get; set; }
    public float PlayerSensitivity { get; set; }
    public float CurrentSpeed { get; private set; }
    private float xRotation = 0f;
    #endregion

    #region Mouse Logic
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CurrentSpeed = PlayerSpeed;
    }
    #endregion

    #region Player logic
    public void HandlePlayerLook()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * PlayerSensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * PlayerSensitivity;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }

    public void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        transform.parent.Translate(CurrentSpeed * Time.deltaTime * direction, Space.World);
    }

    public void SetupFps(float speed, float sensitivity)
    {
        SetParams(speed, sensitivity);
        LockCursor();
    }

    public void SetParams(float speed, float sensitivity)
    {
        PlayerSpeed = speed;
        PlayerSensitivity = sensitivity;
    }
    #endregion
}
