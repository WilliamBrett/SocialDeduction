using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CoreGameScript : MonoBehaviour
{
    //private const int V = 0;

    //General delcarations
    public static CoreGameScript current;
    //public GameObject[] AddressBook;
    public GameObject[] CountOfHeads;
    Actor[] GeneralPublic;
    //public ActorProperties[] 
    private GameObject Textbox;
    public int GamePhase;
    private Image anImage; //used to specify images for alteration
    private int PhaseDelay; //PD-- on Update(), on 0 trigers change of phase
    private int ShortDelay = 250; //This setting controls the length of a "short" timed delay
    private int StandardDelay = 500; //This setting controls the length of a "normal" tomed delay
    private int LongDelay = 750;
    private readonly string[] NameRegistry = { "Ace", "Barbara", "Boris", "Caleb", "David", "Elizabeth", "James", "Jennifer", "Jessica", "John", "Joseph", "Linda", "Mary", "Michael", "Patricia", "Patrick", "Richard", "Robert", "Sarah", "Susan", "Thomas", "William" };
    public int ActorQuantity = 12;

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
        initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        //GeneralPublic = GameObject.FindGameObjectsWithTag("Actor");
        CountOfHeads = GameObject.FindGameObjectsWithTag("Actor");
        Textbox = GameObject.FindGameObjectWithTag("Textbox");
        //startTest();
        for (int i = 0; i < ActorQuantity; i++)
        {
            GeneralPublic[i].Avatar = CountOfHeads[i];
        }
        //gameBoardSetup();
    }

    void initialize()
    {
        //This is used to start a new game, seperated from awake() or start() to support starting a new game mid-runtime
        GamePhase = 100;
        PhaseDelay = ShortDelay;
        GeneralPublic = new Actor[ActorQuantity];
        for (int i = 0; i < ActorQuantity; i++)
        {
            GeneralPublic[i] = new Actor(i, ActorQuantity);
        }
        string[] UnusedNames = NameRegistry;
        for (int i = 1; i < ActorQuantity; i++)
        {
            int rng = UnityEngine.Random.Range(0, UnusedNames.Length - 1);
            GeneralPublic[i].Name = UnusedNames[rng];
            UnusedNames = UnusedNames.Where(w => w != UnusedNames[rng]).ToArray();
        }
    }

    /*void gameBoardSetup()
    {
        
        //Used on start() to "load" the current in-progress game

    }*/

    // Update is called once per frame
    void Update()
    {
        PhaseDelay--;
        if (PhaseDelay == 0) ProgressPhase();
    }

    public void startTest()
    {
        ChangeSprite(0, CompassIcon);
        TextboxAppend("test");
    }

    public void ChangeSprite(int refId, Sprite swapIn)
    {
        anImage = GeneralPublic[refId].Avatar.GetComponent<Image>();
        anImage.sprite = swapIn;
    }

    public void TextboxAppend(string textToAdd) => Textbox?.GetComponent<TextboxScript>().AddText("\n" + textToAdd);
    public void TextboxAppend2(string textToAdd)
    {
        Textbox?.GetComponent<TextboxScript>().AddText(textToAdd);
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

    public void ButtonClicked(int buttonID)
    {
        //string stringAppend = "Button ";
        //stringAppend += buttonID.ToString();
        //stringAppend += " pressed";
        TextboxAppend("Button " + buttonID.ToString() + " pressed");//buttonID
        //startTest();
    }

    public void ProgressPhase()
    {
        switch (GamePhase)
        {
            case 1: GamePhase = 15; break;
            case 15: GamePhase = 2; break;
            case 2: GamePhase = 25; break;
            case 25: GamePhase = 3; break;
            case 3: GamePhase = 35; break;
            case 35: GamePhase = 4; break;
            case 4: GamePhase = 45; break;
            case 45: GamePhase = 1; break;
            case 100:
                TextboxAppend("You have arrived in a town along you journey, with " + (GeneralPublic.Length - 1).ToString() + " inhabitants.");
                GamePhase = 101;
                PhaseDelay = StandardDelay;
                break;
            case 101:
                TextboxAppend("The town is in an uprour over the murder of it's leader. Everyone suspects everyone else ...");
                GamePhase = 102;
                PhaseDelay = StandardDelay;
                break;
            case 102:
                TextboxAppend("...Except you. In absense, your innocence is proven. You are an outsider, and have been asked to help find the culpret before resuming your journey.");
                ChangeSprite(0, CompassIcon);
                GamePhase = 15;
                PhaseDelay = LongDelay;
                break;
        }
    }

    public void ButtonsOn()
    {
        for (int i = 0; i < GeneralPublic.Length; i++)
        {
            GeneralPublic[i].Avatar.GetComponent<Button>().interactable = true;
        }
    }
    public void ButtonsOff()
    {
        for (int i = 0; i < GeneralPublic.Length; i++)
        {
            GeneralPublic[i].Avatar.GetComponent<Button>().interactable = false;

        }
    }
    public void Announcement()
    {

    }
    public void StartStatement()
    {

    }
    public void EndStatement()
    {

    }
    public void StartVote()
    {

    }
    public void EndVote()
    {

    }
    public void Intermission()
    {

    }
}

class Actor
{
    public string Name;
    public int ID;
    public string Role;
    public int[] amnity;
    public GameObject Avatar;
    //private Random random;
    public Actor(int ActorNum, int totalActors)
    {
        this.ID = ActorNum;
        this.amnity = new int[totalActors];
        SetAmnity(totalActors);
    }
    public void SetAmnity(int ActorNum)
    {
        amnity[0] = 0;
        for (int i = 1; i < ActorNum; i++)
        {
            amnity[i] = UnityEngine.Random.Range(1, 5);
        }
    }
}
