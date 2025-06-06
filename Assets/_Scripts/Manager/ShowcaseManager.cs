using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowcaseManager : MonoBehaviour
{
    public static ShowcaseManager Instance;

    [SerializeField] private GameObject plane1;
    [SerializeField] private GameObject plane2;
    [SerializeField] private GameObject showcaseFokkerCamera;
    [SerializeField] private GameObject showcaseSbdCamera;
    [SerializeField] private GameObject showcaseMainCamera;
    [SerializeField] private List<WorldText> textToDisplay1;
    [SerializeField] private float fokkerSeconds;
    [SerializeField] private float sbdSeconds;
    [SerializeField] private Material materialPlane1;
    [SerializeField] private Material materialPlane2;
    [SerializeField] private MeshRenderer ground;
    private Animator plane1Animator;
    private Animator plane2Animator;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        plane1Animator = plane1.GetComponent<Animator>();
        plane2Animator = plane2.GetComponent<Animator>();
        SoundManager.Instance.AdjustMusic(0.5f);
        if (plane1Animator.gameObject.activeInHierarchy)
            SoundManager.Instance.PlayMusic("fokker-main");
        else
            SoundManager.Instance.PlayMusic("sbd-main");
    }

    public void SwapPlanes()
    {
        if (plane1.activeInHierarchy)
        {
            PlayerController.Instance.Ovm.LookedAtObject = plane2;
            plane2.SetActive(true);
            plane1.SetActive(false);
            ground.material = materialPlane2;
            SoundManager.Instance.StopMusic("fokker-main");
            SoundManager.Instance.PlayMusic("sbd-main");
        }
        else
        {
            PlayerController.Instance.Ovm.LookedAtObject = plane1;
            plane1.SetActive(true);
            plane2.SetActive(false);
            ground.material = materialPlane1;
            SoundManager.Instance.StopMusic("sbd-main");
            SoundManager.Instance.PlayMusic("fokker-main");
        }
    }

    public void StartShowcase()
    {
        PlayerController.Instance.InShowcase = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        CanvasManager.Instance.OvmCanvas.gameObject.SetActive(false);
        showcaseMainCamera.SetActive(true);
        PlayerController.Instance.OverviewCamera.enabled = false;
        if (plane1Animator.gameObject.activeInHierarchy)
        {
            showcaseFokkerCamera.SetActive(true);
            SoundManager.Instance.PlayMusic("fokker-show");
            StartCoroutine(Showcase(plane1Animator, fokkerSeconds, showcaseFokkerCamera, "fokker"));
        }
        else
        {
            showcaseSbdCamera.SetActive(true);
            SoundManager.Instance.PlayMusic("sbd-show");
            StartCoroutine(Showcase(plane2Animator, sbdSeconds, showcaseSbdCamera, "sbd"));
        }
    }

    private IEnumerator Showcase(Animator anim, float seconds, GameObject showCam, string planeName)
    {
        anim.SetBool("startShowcase", true);
        yield return new WaitForSeconds(seconds);
        anim.SetBool("startShowcase", false);
        PlayerController.Instance.OverviewCamera.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        CanvasManager.Instance.OvmCanvas.gameObject.SetActive(true);
        showCam.SetActive(false);
        showcaseMainCamera.SetActive(false);
        PlayerController.Instance.InShowcase = false;
        SoundManager.Instance.StopMusic(planeName + "-show");
        SoundManager.Instance.PlayMusic(planeName + "-main");
    }

    public void UpdateInfoBox()
    {
        if (plane1.activeInHierarchy)
        {
            CanvasManager.Instance.InfoBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "PLANE - Fokker Dr.I";
            CanvasManager.Instance.InfoBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "triplane - often known simply as the Fokker Triplane, was a World War I fighter aircraft built by Fokker-Flugzeugwerke. The Dr.I saw widespread service in the spring of 1918. It became famous as the aircraft in which Manfred von Richthofen gained his last 17 victories. The Fokker Dr. 1 was flown with great success by many German aces, most notably Josef Jacobs with 30 confirmed kills in the type.";
            CanvasManager.Instance.InfoBox.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "BEST PILOT - Red Baron";
            CanvasManager.Instance.InfoBox.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text =
            "Manfred von Richthofen 2 May 1892 – 21 April 1918), known in English as Baron von Richthofen or the Red Baron, was a fighter pilot with the German Air Force during World War I. He is considered the ace-of-aces of the war, being officially credited with 80 air combat victories.";
        }
        else
        {
            CanvasManager.Instance.InfoBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "PLANE - SBD Dauntless";
            CanvasManager.Instance.InfoBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "dive bomber - The SBD is best remembered as the bomber that delivered the fatal blows to the Japanese carriers at the Battle of Midway in June 1942. The type earned its nickname 'Slow But Deadly' (from its SBD initials) during this period. During its combat service, the SBD proved to be an effective naval scout plane and dive bomber. It possessed long range, good handling characteristics, maneuverability, potent bomb load, great diving characteristics from the perforated dive brakes.";
            CanvasManager.Instance.InfoBox.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "BEST PILOT - Dick";
            CanvasManager.Instance.InfoBox.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text =
            "Richard Halsey Best  (March 24, 1910 – October 28, 2001) was a dive bomber pilot and squadron commander in the United States Navy during World War II. Stationed on the aircraft carrier USS Enterprise, Best led his dive bomber squadron at the 1942 Battle of Midway, sinking two Japanese aircraft carriers in one day, before being medically retired that same year due to damage to his lungs caused by breathing bad oxygen during the battle.";
        }
    }
}
