using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IconScriptPrototype : MonoBehaviour
{
    public Image anImage;
    public Sprite anIcon;
    //[SerializeField] private GameObject TextContainer;
    public GameObject TextContainer;
    private TextMeshProUGUI textMeshPro;
    public GameObject ankh;

    public void Start()
    {
        if (TextContainer != null)
        {
            textMeshPro = TextContainer.GetComponent<TextMeshProUGUI>();
        }

    }
    public void dotheankhthing()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text += "The inverted ankh is used to denote that a actor is 'dead'.";
        }
        ankh.SetActive(true);
    }

    public void ImageSwap()
    {
        anImage.sprite = anIcon;
    }
}
