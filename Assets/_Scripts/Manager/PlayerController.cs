using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    #region FPSCameraSettings
    [Header("FPS Camera Parametres")]
    [SerializeField] private float speed;
    [SerializeField] private readonly float sensitivity = 2f;
    [SerializeField] private FPSMovement fps;
    #endregion

    #region OverviewCameraSettings
    [Header("Overview Camera Parametres")]
    [SerializeField] private float zoomSpeed = 6f;
    [SerializeField] private float dragSpeed = 600f;
    [SerializeField] private OverviewMovement ovm;
    [SerializeField] private bool loopCamera = false;
    [SerializeField] private int loopSpeed = 3;
    #endregion

    #region MISCSettings
    [Header("General Settings")]
    [SerializeField] private bool fpsCameraOn = true;
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private Camera overviewCamera;
    [field: SerializeField] public bool InMenu { get; set; } = false;
    [field: SerializeField] public bool InSubMenuOpen { get; set; } = false;
    [field: SerializeField] public bool InShowcase { get; set; } = false;
    #endregion

    #region GettersSetters
    public float Speed { get => speed; set => speed = value; }
    public bool FpsCameraOn { get => fpsCameraOn; set => fpsCameraOn = value; }
    public OverviewMovement Ovm { get => ovm; }
    public Camera FpsCamera { get => fpsCamera; }
    public Camera OverviewCamera { get => overviewCamera; }
    #endregion

    #region Startup
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        InitCameras();
        ovm.SetupOvm(zoomSpeed, dragSpeed, loopCamera, loopSpeed);
        fps.SetupFps(speed, sensitivity);
        SetCamera();
    }

    private void InitCameras()
    {
        fpsCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        overviewCamera = GameObject.FindGameObjectWithTag("SecondaryCamera").GetComponent<Camera>();
        fps = fpsCamera.GetComponent<FPSMovement>();
        ovm = overviewCamera.GetComponent<OverviewMovement>();
        if (fpsCameraOn)
            fpsCameraOn = false;
        else
            fpsCameraOn = true;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        fps.SetParams(speed, sensitivity);
        ovm.SetParams(zoomSpeed, dragSpeed, loopCamera, loopSpeed);
    }
#endif
    #endregion

    #region Camera controls
    private void SetCamera()
    {
        fpsCameraOn = !fpsCameraOn;
        if (fpsCameraOn)
        {
            fpsCamera.enabled = true;
            overviewCamera.enabled = false;
            ovm.CamController.enabled = false;
        }
        else
        {
            fpsCamera.enabled = false;
            overviewCamera.enabled = true;
            ovm.CamController.enabled = true;
        }
        SetCursorBasedOnCam();
    }

    public void SetCursorBasedOnCam()
    {
        if (fpsCameraOn)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void JumpToObject()
    {
        fpsCamera.transform.parent.position = overviewCamera.transform.position;
        fpsCamera.transform.rotation = Quaternion.Euler(overviewCamera.transform.eulerAngles.x, fpsCamera.transform.eulerAngles.y, fpsCamera.transform.eulerAngles.z);
        fpsCamera.transform.parent.rotation = Quaternion.Euler(fpsCamera.transform.parent.rotation.x, overviewCamera.transform.eulerAngles.y, fpsCamera.transform.parent.rotation.z);
        SetCamera();
        SetCursorBasedOnCam();
    }

    public void ToggleRotation()
    {
        GameObject caller = EventSystem.current.currentSelectedGameObject;
        TextMeshProUGUI buttonText = caller.GetComponent<TextMeshProUGUI>();
        loopCamera = !loopCamera;
        buttonText.text = loopCamera ? "rotate on" : "rotate off";
        ovm.LoopCamera = loopCamera;
    }
    #endregion

    #region UPDATE FUNCTIONS
    private void FixedUpdate()
    {
        try
        {
            if (InShowcase) return;
            if (InMenu) return;
            if (fpsCameraOn)
            {
            }
            else
            {
                ovm.UpdateOrbitalAngle();
            }
        }
        catch (System.Exception) { }
    }
    private void Update()
    {
        try
        {
            if (InShowcase) return;
            if (InMenu)
            {
                ovm.CamController.enabled = false;
                return;
            }
            else
            {
                ovm.CamController.enabled = true;
            }

            if (fpsCameraOn)
            {
                fps.HandlePlayerLook();
                fps.MovePlayer();
            }
            else
            {
                ovm.CamController.MovementSmoothing = false;
                ovm.UpdateLookAtObject();
                ovm.UpdateCameraAngles();
                ovm.MouseZoom();
                //ovm.RaycastOnClick();
            }
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }
        catch (System.Exception) { }
    }

    private void LateUpdate()
    {
        if (InShowcase) return;
        if (InMenu) return;
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetCamera();
            CanvasManager.Instance.HandleCanvases();
        }
        if (fpsCamera) { }
        else
        {
        }
    }
    #endregion
}