using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreGameScript : MonoBehaviour
{
    private const int V = 0;

    //General delcarations
    public static CoreGameScript current;
    public GameObject[] GeneralPublic;
    public GameObject Textbox;
    public int GamePhase;
    private Image anImage; //used to specify images for alteration

    //day & night declerations
    public GameObject DaySky;
    public GameObject NightSky;
    public Image DayNightIcon;
    public Sprite DayIcon;
    public Sprite NightIcon;
    public bool isDay;
    //0 = night, 1 = announcements, 2 = statemnets, 3 = voting, 4 = execution

    //sprite delcarations
    public Sprite PersonIcon;
    public Sprite CompassIcon;
    public Sprite KnifeIcon;


    private void Awake()
    {
        current = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GamePhase = 1;
        GeneralPublic = GameObject.FindGameObjectsWithTag("Actor");
        Textbox = GameObject.FindGameObjectWithTag("Textbox");
        ChangeSprite(0, CompassIcon);
        TextboxAppend("test");
        //TextBox.
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeSprite(int refId, Sprite swapIn)
    {
        anImage = GeneralPublic[refId].GetComponent<Image>();
        anImage.sprite = swapIn;
    }

    public void TextboxAppend(string textToAdd) => Textbox?.GetComponent<TextboxScript>().AddText(textToAdd);

    public void DayToNight()
    {
        DaySky.SetActive(false);
        NightSky.SetActive(true);
        DayNightIcon.sprite = NightIcon;
        isDay = false;

    }
    public void NighttoDay()
    {
        DaySky.SetActive(true);
        NightSky.SetActive(false);
        DayNightIcon.sprite = DayIcon;
        isDay = true;

    }

    public void ButtonClicked(int buttonID)
    {

        TextboxAppend("Button " + buttonID.ToString() + " pressed");
    }

}
