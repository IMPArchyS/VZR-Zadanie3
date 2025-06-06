using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldText : MonoBehaviour
{
    [field: SerializeField] TextMeshProUGUI InfoText { get; set; }
    [field: SerializeField] Image TextBackground { get; set; }

    public void ActivateText()
    {
        InfoText.gameObject.SetActive(true);
        TextBackground.gameObject.SetActive(true);
    }

    public void DisableText()
    {
        InfoText.gameObject.SetActive(false);
        TextBackground.gameObject.SetActive(false);
    }
}
