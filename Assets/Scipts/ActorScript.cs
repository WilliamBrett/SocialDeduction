using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorScript : MonoBehaviour
{
    private Image ActorImage;

    public void Start()
    {
        ActorImage = this.GetComponent<Image>();
    }

        public void swapSprite(Sprite swapIn)
    {
        ActorImage.sprite = swapIn;
    }
}
