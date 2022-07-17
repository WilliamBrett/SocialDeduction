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
    private Actor[] GeneralPublic;
    //public ActorProperties[] 
    private GameObject Textbox;
    public int GamePhase;
    private Image anImage; //used to specify images for alteration
    private int PhaseDelay; //PD-- on Update(), on 0 trigers change of phase
    private int MicroDelay = 1;
    private int ShortDelay = 25;//250 //This setting controls the length of a "short" timed delay
    private int StandardDelay = 50;//500 //This setting controls the length of a "normal" tomed delay
    private int LongDelay = 75;//750
    private readonly string[] NameRegistry = { "Ace", "Barbara", "Boris", "Caleb", "David", "Elizabeth", "James", "Jennifer", "Jessica", "John", "Joseph", "Linda", "Mary", "Michael", "Patricia", "Patrick", "Richard", "Robert", "Sarah", "Susan", "Thomas", "William" };
    private int[] Hat; //This is used to talley votes
    private int HatSize;
    private int[] VoteTalley;
    public int ActorQuantity = 12;
    private int rng; //used for random decisions

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

    public int testint;

    public bool debug;

    private void Awake()
    {
        current = this;
        DontDestroyOnLoad(gameObject);
        initialize();
    }

    // Start is called before the first frame update
    private void Start()
    {
        Console.WriteLine("Debug: Start");
        //GeneralPublic = GameObject.FindGameObjectsWithTag("Actor");
        CountOfHeads = GameObject.FindGameObjectsWithTag("Actor");
        Textbox = GameObject.FindGameObjectWithTag("Textbox");
        //startTest();
        testint = 0;
        for (int i = 0; i < ActorQuantity; i++)
        {
            GeneralPublic[i].Avatar = CountOfHeads[i];
            GeneralPublic[i].SetupAvatar(PersonIcon);
            //Console.WriteLine("Actor " + i + " setup.");
        }
        for (int i = 0; i < ActorQuantity; i++)
        {
            
            //Console.WriteLine("Actor " + i + " setup.");
            testint++;
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
            GeneralPublic[i] = new Actor(i, ActorQuantity, PersonIcon);
        }
        string[] UnusedNames = NameRegistry;
        for (int i = 1; i < ActorQuantity; i++)
        {
            rng = UnityEngine.Random.Range(0, UnusedNames.Length - 1);
            GeneralPublic[i].Name = UnusedNames[rng];
            UnusedNames = UnusedNames.Where(w => w != UnusedNames[rng]).ToArray();
        }
        //ActorsRemaining = ActorQuantity;
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
        TextboxAppend("Button " + buttonID.ToString() + " pressed");//buttonID
        switch (GamePhase)
        {
            case 200://player's statement
                PlayerStatement(buttonID);
                break;
            case 300://player's vote
                PlayerVote(buttonID);
                break;
            case 400://player's night action
                break;
        }
    }

    int Talleyvotes(int PlayerVote)
    {
        int accused = 0;
        return accused;
    }

    int CurrentlyAlive()
    {
        int j = 0;
        for (int i = 0; i < GeneralPublic.Length; i++)
        {
            if (GeneralPublic[i].Alive) j++;
        }
        return j;
    }

    void PlayerStatement(int ActorID)
    {
        if (ActorID != 0)
        {
            TextboxAppend("You accuse " + GeneralPublic[ActorID].Name + " of murder!");
        }
        else
        {
            TextboxAppend("You abstain from making any statements.");
        }
        PhaseDelay = ShortDelay;
        GamePhase = 2;
        return;
    }

    void ActorStatement(int ActorID)
    { //
        int decision = GeneralPublic[ActorID].Accuse();
        if (decision != -1)
        {
            if (decision != -2)
            {
                TextboxAppend(GeneralPublic[ActorID].Name + " accuses " + GeneralPublic[decision].Name + " of murder!");
                for (int i = 1; i < GeneralPublic.Length; i++)
                {
                    GeneralPublic[i].AddAmnity(decision, 1);
                }
                PhaseDelay = ShortDelay;
            }
            else
            {
                PhaseDelay = MicroDelay;
            }
        }
        else
        {
            PhaseDelay = MicroDelay;
        }
    }

    void PlayerVote(int ActorID)
    {
        if (ActorID != 0)
        {
            TextboxAppend("You vote for " + GeneralPublic[ActorID].Name + ".");
        }
        else
        {
            TextboxAppend("You abstain from voting.");
        }
        PhaseDelay = ShortDelay;
        GamePhase = 3;
        return;
    }

    void ActorVote(int ActorID)
    {
        int decision = GeneralPublic[ActorID].Accuse();
        if (decision == -1)
        {
            TextboxAppend(GeneralPublic[ActorID].Name + " abstains from voting.");
        }
        else if (decision != -2)
        {
            TextboxAppend(GeneralPublic[ActorID].Name + " votes for " + GeneralPublic[decision].Name);
            VoteTalley[decision]++;
        }
    }

    void setupHat()
    {
        Hat = new int[GeneralPublic.Length];
        for (int i = 1; i < GeneralPublic.Length; i++)
        {

            Hat[i] = i;
        }
    }

    void setupVoteTalley()
    {
        VoteTalley = new int[GeneralPublic.Length];
        for (int i = 1; i < GeneralPublic.Length; i++)
        {
            VoteTalley[i] = 0;
        }
    }

    void TalleyVote()
    {
        if (VoteTalley.Max() >= (CurrentlyAlive() / 2))
        {
            Execution(Array.IndexOf(VoteTalley, VoteTalley.Max()));
        }
    }

    void Execution(int executee)
    {
        TextboxAppend(GeneralPublic[executee].Name + " has been found guilty. They are swiftly executed...");
        GeneralPublic[executee].Death();
        for (int i = 1; i < GeneralPublic.Length; i++)
        {
            GeneralPublic[i].DropSuspicion(executee);
        }

    }

    public void ProgressPhase()
    {
        switch (GamePhase)
        {
            case 1: //mid-announcement
                //TextboxAppend("Announcement");
                PhaseDelay = MicroDelay;
                GamePhase = 11;
                break;
            case 11: //post-statement
                //TextboxAppend("debug: post-statement");
                PhaseDelay = MicroDelay;
                GamePhase = 19;
                break;
            case 19:
                setupHat();
                TextboxAppend("The time has come for announcements, there is a pause to see if anyone steps forward...");
                GamePhase = 2;
                PhaseDelay = MicroDelay;
                break;
            case 2: //statements
                //TextboxAppend("debug: statements");
                //This picks a random name out of the hat, then removes it from the hat
                if (Hat.Length != 0)
                {
                    rng = UnityEngine.Random.Range(1, Hat.Length) - 1;
                    if (Hat[rng] == 0)
                    {
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                        PhaseDelay = MicroDelay;
                        GamePhase = 200;
                        break;
                    }
                    else if (GeneralPublic[rng].Alive)
                    {

                        //TextboxAppend("RNG is: " + rng + ", Hat size is " + Hat.Length);
                        ActorStatement(Hat[rng]);
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                    }
                    else
                    {
                        PhaseDelay = MicroDelay;
                    }
                }
                else
                {
                    //TextboxAppend("debug: hat acknowledged as empty");
                    PhaseDelay = MicroDelay;
                    GamePhase = 21;
                }
                //GamePhase = 21;
                //TextboxAppend("debug: hat size is " + Hat.Length);
                break;
            case 200: //player's turn to make a statement
                if (debug)
                {
                    PhaseDelay = MicroDelay;
                    GamePhase = 2;
                    break;
                }
                TextboxAppend("Do you wish to accuse any of the villagers?./n(Click on the villager you would like to accuse, or yourself to accuse nobody)");
                break;
            case 21: //post-statement
                     //TextboxAppend("debug: post-statement");
                PhaseDelay = MicroDelay;
                GamePhase = 29;
                break;
            case 29:
                setupHat();
                setupVoteTalley();
                GamePhase = 3;
                PhaseDelay = ShortDelay;
                break;
            case 3: //Mid-votes
                //TextboxAppend("debug: Votes");
                HatSize = Hat.Length;
                if (Hat.Length != 0)
                {
                    rng = UnityEngine.Random.Range(1, Hat.Length) - 1;
                    if (Hat[rng] == 0)
                    {
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                        GamePhase = 300;
                        PhaseDelay = ShortDelay;
                        break;
                    }
                    //if (GeneralPublic[i].ali
                    if (GeneralPublic[rng].Alive) ActorVote(Hat[rng]);
                    Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                }
                else
                {
                    GamePhase = 31;
                }
                PhaseDelay = ShortDelay;

                break;
            case 300: //player's turn to vote
                if (debug)
                {
                    PhaseDelay = MicroDelay;
                    GamePhase = 3;
                    break;
                }
                TextboxAppend("The time has come for you to vote./n(Click on the villager you would like to accuse, or yourself to accuse nobody)");

                break;
            case 31: //post-vote
                TalleyVote();
                PhaseDelay = ShortDelay;
                GamePhase = 4;
                break;
            case 4: //Intermission ending
                //TextboxAppend("debug: Intermission");
                DayToNight();
                TextboxAppend("The night is peaceful enough, though in the silence the paranoia of the villagers only grows...");
                for (int i = 1; i < GeneralPublic.Length; i++)
                {
                    GeneralPublic[i].Paranoia();
                }
                PhaseDelay = LongDelay;
                GamePhase = 41;
                break;
            case 41: //post-intermission
                //TextboxAppend("debug: post-intermission");
                NighttoDay();
                PhaseDelay = MicroDelay;
                GamePhase = 1;
                break;
            case 400: //player's turn to night action
                break;
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
                GeneralPublic[0].SetIcon(CompassIcon);
                //GamePhase = 15;
                //TextboxAppend("debug: Dropping into the first statement");
                GamePhase = 103;
                PhaseDelay = LongDelay;
                break;
            case 103:
                //TextboxAppend("Soon the sun sets, and darkness blankets the town");
                //TextboxAppend("Test1:" +  GeneralPublic.Length);
               // TextboxAppend("Test2:" + CountOfHeads.Length);
                //TextboxAppend("Test3:" + (GeneralPublic[0].Avatar != null)); 
                //TextboxAppend("Test4:" + GeneralPublic[1].ID);
                //TextboxAppend("Test5:" + (GeneralPublic[1].Avatar != null));
                //TextboxAppend("Test66:" + GeneralPublic[5].ID);
                //TextboxAppend("Test7:" + (GeneralPublic[5].Avatar != null));
                //TextboxAppend("Test8:" + testint);
                GamePhase = 4;
                PhaseDelay = ShortDelay;
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
    public int[] Amnity;
    public GameObject Avatar;
    public int[] Suspicion;
    public int Suspect; //This is used by special roles, where an actor identified a suspect
    public bool Alive;
    public Sprite Icon;
    //private Random random;
    public Actor(int ActorNum, int totalActors, Sprite StartingIcon)
    {
        this.ID = ActorNum;
        this.Amnity = new int[totalActors];
        this.Suspicion = new int[totalActors];
        this.Alive = true;
        Icon = StartingIcon;
        SetAmnity(totalActors);
    }
    public void SetAmnity(int totalActors)
    {
        Amnity[0] = 0;
        for (int i = 1; i < totalActors; i++)
        {
            Amnity[i] = UnityEngine.Random.Range(1, 5);
        }
        Amnity[this.ID] = -1;
        Suspicion = Amnity;
    }
    public void AddAmnity(int ActorNum, int ammount)
    { //Adds a point of amnity, if amnity >= 4 than adds a point of suspicion. 
        if (Amnity[ActorNum] == -1) return;
        Amnity[ActorNum] += ammount;
        if (Amnity[ActorNum] >= 5)
        {
            this.Suspicion[ActorNum] += ammount;
        }
    }
    public void Paranoia()
    {
        //during the night phase, the actors become more slightly more suspicious of all other actors
        for (int i = 1; i < Suspicion.Length; i++)
        {
            AddSuspicion(i, 1);
        }
    }
    public void AddSuspicion(int ActorNum, int ammount)
    { //Adds suspicion, increasing the possibility of an accusation
        if (Amnity[ActorNum] == -1) return;
        this.Suspicion[ActorNum] += ammount;
    }
    public void DropSuspicion(int ActorNum)
    { //If an actor is found innocent, or is dead, all accumulated suspicion is lost
        this.Amnity[ActorNum] = -1;
    }
    public int Accuse()
    { //Returns an actor this actor is suspicious of
        if (!Alive) return -2;
        if (Suspicion.Max() > 4)
        {
            return Array.IndexOf(Suspicion, Suspicion.Max());
        }
        return -1;
    }
    public void Death()
    {
        Alive = false;
        Avatar.GetComponent<ActorScript>().SetAnkh(true);
    }
    public void SetIcon(Sprite InIcon)
    {
        Icon = InIcon;
        Avatar.GetComponent<ActorScript>().SetIcon(Icon);
    }
    public void SetupAvatar(Sprite InIcon)
    {
        Avatar.GetComponent<ActorScript>().SetActorId(ID);
        Avatar.GetComponent<ActorScript>().SetIcon(Icon);
        if (!Alive)
        {
            Avatar.GetComponent<ActorScript>().SetAnkh(true);
        }
        return;
    }
}
