using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreScript : MonoBehaviour
{
    //General delcarations
    public static CoreScript current;
    public event Action onActorTrigger;
    public GameObject[] GeneralPublic;
    public int gamePhase;
    public Image anImage; //used to specify images for alteration

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
        gamePhase = 1;
        GameObject[] GeneralPublic = GameObject.FindGameObjectsWithTag("actor");
        anImage = GeneralPublic[0].GetComponent<Image>();
        anImage.sprite = CompassIcon;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public event Action eventAction;
    public void triggerExampleAction()
    {
        eventAction?.Invoke();
    }

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

}
