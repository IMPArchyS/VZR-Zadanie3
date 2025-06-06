using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    [SerializeField] private Canvas fpsCanvas;
    [SerializeField] private Canvas ovmCanvas;
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private GameObject infoBox;

    public Canvas OvmCanvas { get => ovmCanvas; }
    public Canvas WorldCanvas { get => worldCanvas; }
    public GameObject InfoBox { get => infoBox; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        HandleCanvases();
    }

    public void HandleCanvases()
    {
        if (PlayerController.Instance.FpsCameraOn)
        {
            fpsCanvas.gameObject.SetActive(true);
            ovmCanvas.gameObject.SetActive(false);
        }
        else
        {
            fpsCanvas.gameObject.SetActive(false);
            ovmCanvas.gameObject.SetActive(true);
        }
    }

    public void ToggleInfoBox()
    {
        infoBox.SetActive(!infoBox.activeInHierarchy);
    }
}
