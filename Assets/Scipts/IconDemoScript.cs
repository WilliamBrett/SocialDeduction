using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IconDemoScript : MonoBehaviour
{
    public int roleid;
    public Image anImage;
    public Sprite anIcon;
    public GameObject TextContainer;
    private TextMeshProUGUI textMeshPro;
   
    public void Start()
    {
        if (TextContainer != null)
        {
            textMeshPro = TextContainer.GetComponent<TextMeshProUGUI>();
        }

    }

    public void buttonScript()
    {
        anImage.sprite = anIcon;
        switch (roleid)
        {
            case 1: //compass
                    textMeshPro.text += "\n\nThe Compass represents the outsider; in absense, their innocence is assured. This is a protagonistic social role, with the ability of being declared innocent at the game's start. This role is given to THE player, to provide some amount of safety to reduce the possibilities of early elimination.";
                break;
            case 2: //knife
                textMeshPro.text += "\n\nThe knife represents the killer. This is an antagonistic and killer role, with the capacity to eliminate other players during the night phase";
                break;
            case 3: //spyglass
                textMeshPro.text += "\n\nThe spyglass represents the lookout. This is an protagonistic investigative role, roles that are capable of uncovering other players' roles are a key component of social deduction genre.";
                break;
            case 4: //shield
                textMeshPro.text += "\n\nThe shield represents the protector. This is a protagonistic protective role, with the ability to prevent the killer from eliminating their target, if the protector can correctly guess the killer's target.";
                break;
            case 5: //consort
                textMeshPro.text += "\n\nThe lips represent the consort. This is a protagonistic social role, with the ability to prevent another player from exercising their role's abilities by 'occupying' their night phase. Similar roles are a staple of games where three or more antagonist players, but are a far more contriversial in games with only a single antagonistic killer.";
                break;
        }
    }
}
