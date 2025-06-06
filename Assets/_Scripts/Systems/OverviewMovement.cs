using UnityEngine;
using TMPro.Examples;

public class OverviewMovement : MonoBehaviour
{
    #region OverviewSettings
    public float ZoomSpeed { get; set; }
    public float DragSpeed { get; set; }
    public bool LoopCamera { get; set; } = false;
    public float CameraLoopSpeed { get; set; } = 3f;
    public CameraController CamController { get; private set; }
    public GameObject LookedAtObject { get => lookAtObj; set => lookAtObj = value; }
    [SerializeField] private GameObject lookAtObj;
    #endregion

    #region Startup
    public void SetupOvm(float zoomSpeed, float dragSpeed, bool loopCamera, float loopSpeed)
    {
        SetParams(zoomSpeed, dragSpeed, loopCamera, loopSpeed);
        lookAtObj = GameObject.FindGameObjectWithTag("Star");
        transform.position = new Vector3(5, 14, -20);
        CamController = GetComponent<CameraController>();
        CamController.CameraTarget = lookAtObj.transform;
    }

    public void SetParams(float zoomSpeed, float dragSpeed, bool loopCamera, float loopSpeed)
    {
        ZoomSpeed = zoomSpeed;
        DragSpeed = dragSpeed;
        LoopCamera = loopCamera;
        CameraLoopSpeed = loopSpeed;
    }
    #endregion

    #region Camera Update
    public void UpdateOrbitalAngle()
    {
        if (LoopCamera)
            CamController.OrbitalAngle += CameraLoopSpeed * Time.fixedDeltaTime;
    }

    public void UpdateLookAtObject()
    {
        if (lookAtObj == null)
        {
            lookAtObj = GameObject.FindGameObjectWithTag("Star");
            CamController.CameraTarget = lookAtObj.transform;
        }
    }

    public void UpdateCameraAngles()
    {
        if (LoopCamera) return;

        float axisX = Input.GetAxisRaw("Mouse X");
        float axisY = Input.GetAxisRaw("Mouse Y");
        if (Input.GetMouseButton(0))
        {
            float newOrbitalAngle = CamController.OrbitalAngle + axisX * DragSpeed / 100;
            if (newOrbitalAngle > 360f)
                newOrbitalAngle -= 360f;
            else if (newOrbitalAngle < -360f)
                newOrbitalAngle += 360f;

            CamController.OrbitalAngle = newOrbitalAngle;
            CamController.ElevationAngle = Mathf.Clamp(CamController.ElevationAngle - axisY * DragSpeed / 100, -4f, 90f);
        }
    }
    #endregion

    #region Mouse Logic
    public void MouseZoom()
    {
        float mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (mouseWheel != 0)
            CamController.FollowDistance += mouseWheel * -10 * ZoomSpeed * 75 * Time.deltaTime;
    }

    public void RaycastOnClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera cam = GetComponent<Camera>();
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("OVM CAM RAY: " + hit.collider.gameObject);
                lookAtObj = hit.collider.gameObject;
                CamController.CameraTarget = lookAtObj.transform;
            }
        }
    }
    #endregion
}