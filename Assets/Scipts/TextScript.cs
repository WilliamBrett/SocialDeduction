using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextScript : MonoBehaviour
{
    //[SerializeField] private GameObject TextContainer;
    public GameObject TextContainer;
    private TextMeshProUGUI textMeshPro;

    public void Start()
    {
        if (TextContainer != null)
        {
            textMeshPro = TextContainer.GetComponent<TextMeshProUGUI>();
        }
        
    }
    public void addtext(string texttoadd)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text += texttoadd;
        }
        
    }
}
