using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconScriptPrototype : MonoBehaviour
{
    public Image anImage;
    public Sprite anIcon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ImageSwap()
    {
        anImage.sprite = anIcon;
    }
}
