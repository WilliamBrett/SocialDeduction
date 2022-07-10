using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorScript : MonoBehaviour
{
    private CoreGameScript CoreScript;
    private Image ActorImage;
    public int ActorId;

    public void Start()
    {
        //ActorImage = this.GetComponent<Image>();
        CoreScript = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<CoreGameScript>();

    }

    public void SwapSprite(Sprite swapIn)
    {
        ActorImage.sprite = swapIn;
    }

    public void ButtonClicked()
    {
        //CoreScript.ButtonClicked(int.Parse(name) - 1);
        CoreScript.ButtonClicked(ActorId);
    }
}
