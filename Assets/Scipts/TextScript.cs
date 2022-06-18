using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextScript : MonoBehaviour
{
    [SerializeField] private GameObject TextContainer;
    private TextMeshProUGUI textMeshPro;

    public void Start()
    {
        textMeshPro = TextContainer.GetComponent<TextMeshProUGUI>();
    }
    public void addtext(string texttoadd)
    {
        textMeshPro.text += texttoadd;
    }
}
