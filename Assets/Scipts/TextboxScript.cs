using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextboxScript : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void AddText(string textToAdd)
    {
        textMeshPro.text += textToAdd;
    }
}

