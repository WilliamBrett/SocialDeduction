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
    public GameObject[] CountOfHeads; //used to count "Avatar" items to be assigned to Actors
    private Actor[] GeneralPublic; //The general container for "Actor" objects
    private int[] Alive; //A list of "Actor"s that are currently "alive"
    private GameObject Textbox; //The object that text is written to
    public int GamePhase; //Controls the current state of the game
    private Image anImage; //used to specify images for alteration
    private int PhaseDelay; //PD-- on Update(), on 0 trigers change of phase
    private int MicroDelay = 1; //The length of a "micro" timed delay
    private int ShortDelay = 25;//250 //This setting controls the length of a "short" timed delay
    private int StandardDelay = 50;//500 //This setting controls the length of a "normal" tomed delay
    private int LongDelay = 75;//750
    //Random names are pulled from NameRegistry to assign to Actors
    private readonly string[] NameRegistry = { "Ace", "Barbara", "Boris", "Caleb", "David", "Elizabeth", "James", "Jennifer", "Jessica", "John", "Joseph", "Linda", "Mary", "Michael", "Patricia", "Patrick", "Richard", "Robert", "Sarah", "Susan", "Thomas", "William" };
    private int[] Hat; //This is used to talley votes
    private int[] SocialIndex; //used to record the "social" role members
    private int[] InvestigativeIndex; //used to record the "investigative" role members"
    private int[] ProtectiveIndex; //used to record the "protective" role members
    private int[] KillerIndex; //used to record the "killer" role members"
    private int HatSize; 
    private int[] VoteTalley; //used to talley votes
    public int ActorQuantity = 12; //this is meant to eventually be adjustable to control the number of "Actors"
    private int rng; //used for random decisions
    private string AnnouncementBuffer; //A buffer used for time delayed announcements

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
    public Sprite CrystalBallIcon;
    public Sprite MagnifyingGlassIcon;
    public Sprite LipsIcon;

    public int testint;

    public bool debug;

    private void Awake()
    {
        current = this;
        DontDestroyOnLoad(gameObject);
        initialize(); //sets up the game, but can be recalled if a game needs to be restarted. 
    }

    // Start is called before the first frame update
    private void Start()
    {
        CountOfHeads = GameObject.FindGameObjectsWithTag("Actor"); //CountOfHeads is setup
        Textbox = GameObject.FindGameObjectWithTag("Textbox"); //Textbox is setup
        for (int i = 0; i < ActorQuantity; i++)
        {
            GeneralPublic[i].Avatar = CountOfHeads[i]; //Avatars are assigned from CountOfHeads
            GeneralPublic[i].SetupAvatar(PersonIcon); //Initial avatar is setup
        }
    }

    void initialize()
    {
        //This is used to start a new game, seperated from awake() or start() to support starting a new game mid-runtime
        GamePhase = 100;
        PhaseDelay = ShortDelay;
        GeneralPublic = new Actor[ActorQuantity];
        Alive = new int[ActorQuantity];
        for (int i = 0; i < ActorQuantity; i++)
        {
            GeneralPublic[i] = new Actor(i, ActorQuantity, PersonIcon);
            Alive[i] = i;
        }
        string[] UnusedNames = NameRegistry;
        for (int i = 1; i < ActorQuantity; i++)
        {
            rng = UnityEngine.Random.Range(0, UnusedNames.Length - 1);
            GeneralPublic[i].Name = UnusedNames[rng];
            UnusedNames = UnusedNames.Where(w => w != UnusedNames[rng]).ToArray();
        }
        SetupRoles();
    }

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

    void SetupRoles()
    {
        SetupHat();
        SocialIndex = new int[GeneralPublic.Length];
        InvestigativeIndex = new int[GeneralPublic.Length];
        ProtectiveIndex = new int[GeneralPublic.Length];
        KillerIndex  = new int[GeneralPublic.Length];
        rng = UnityEngine.Random.Range(2, Hat.Length) - 1; //anyone other than the player
        GeneralPublic[rng].Role = "Murderer";
        GeneralPublic[rng].TrueIcon = KnifeIcon;
        GeneralPublic[rng].isEvil = true;
        KillerIndex[0] = rng; //the number is added to the killerhat
        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
        rng = UnityEngine.Random.Range(2, Hat.Length) - 1; //anyone other than the player
        GeneralPublic[rng].Role = "Seer";
        GeneralPublic[rng].TrueIcon = CrystalBallIcon;
        InvestigativeIndex[0] = rng; //the number is added to the killerhat
        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
        rng = UnityEngine.Random.Range(2, Hat.Length) - 1; //anyone other than the player
        GeneralPublic[rng].Role = "Investigator";
        GeneralPublic[rng].TrueIcon = MagnifyingGlassIcon;
        InvestigativeIndex[0] = rng; //the number is added to the killerhat
        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
        rng = UnityEngine.Random.Range(2, Hat.Length) - 1; //anyone other than the player
        GeneralPublic[rng].Role = "Escort";
        GeneralPublic[rng].TrueIcon = LipsIcon;
        SocialIndex[0] = rng; //the number is added to the killerhat
        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
        SocialIndex = SocialIndex.Where(w => w != 0).ToArray(); //sanatize SocialIndex
        InvestigativeIndex = InvestigativeIndex.Where(w => w != 0).ToArray(); //sanatize InvestigativeIndex
        ProtectiveIndex = ProtectiveIndex.Where(w => w != 0).ToArray(); //sanatize ProtectiveIndex
        KillerIndex = KillerIndex.Where(w => w != 0).ToArray(); //sanatize KillerIndex
        return;
    }

    int Talleyvotes(int PlayerVote)
    {
        int accused = 0;
        return accused;
    }

    /*int CurrentlyAlive() //This is antiquated
    //This is used to determine how many players are currently alive, for purposes such as determining game state
    {
        int j = 0;
        for (int i = 0; i < GeneralPublic.Length; i++)
        {
            if (GeneralPublic[i].Alive) j++;
        }
        return j;
    }*/

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
    {
        int decision;
        if (Array.Exists(InvestigativeIndex, element => element == ActorID)){
            decision = GeneralPublic[ActorID].Verify();
        }
        else
        {
            decision = GeneralPublic[ActorID].Accuse();
        }
        //if InvestigativeIndex.Contains(ActorID) GeneralPublic[ActorID].Verify();
        switch (decision)
        {
            case -1: //The actor isn't suspicious enough of anyone to accuse them
                PhaseDelay = MicroDelay;
                break;
            case -2: //The actor is dead
                PhaseDelay = MicroDelay;
                break;
            default:
                double suspicionLevel = GeneralPublic[ActorID].Suspicion[decision] / 5;
                int decision2 = Convert.ToInt32((int) Math.Floor(suspicionLevel));
                if (decision2 > 2) decision2 = 2;
                switch (decision2)
                {
                    case -1: //The actor knows the target is not evil
                        PhaseDelay = ShortDelay;
                        int randomStatementInnocence = UnityEngine.Random.Range(1, 2);
                        switch (randomStatementInnocence)
                        {
                            case 1:
                                TextboxAppend(GeneralPublic[ActorID].Name + " vouches for " + GeneralPublic[decision].Name + ".");
                                for (int i = 1; i < GeneralPublic.Length; i++)
                                {
                                    GeneralPublic[i].AddAmnity(decision, -2);
                                }
                                break;
                            case 2:
                                TextboxAppend(GeneralPublic[ActorID].Name + " assures everyone of " + GeneralPublic[decision].Name + "'s innocence.");
                                for (int i = 1; i < GeneralPublic.Length; i++)
                                {
                                    GeneralPublic[i].AddAmnity(decision, -2);
                                }
                                break;
                        }
                        break;
                    case 0: //The actor is not suspicious
                        PhaseDelay = MicroDelay;
                        break;
                    case 1: //The actor is mildly suspicious 
                        PhaseDelay = ShortDelay;
                        int randomStatementSuspicion = UnityEngine.Random.Range(1, 4);
                        switch (randomStatementSuspicion)
                        {
                            case 1:
                                TextboxAppend(GeneralPublic[ActorID].Name + " harrases " + GeneralPublic[decision].Name + " over a petty slight.");
                                break;
                            case 2:
                                TextboxAppend(GeneralPublic[ActorID].Name + " questions " + GeneralPublic[decision].Name + "'s alibi.");
                                break;
                            case 3:
                                TextboxAppend(GeneralPublic[ActorID].Name + " scolds " + GeneralPublic[decision].Name + " for some insignificant blunder.");
                                break;
                            case 4:
                                TextboxAppend(GeneralPublic[ActorID].Name + " accuses " + GeneralPublic[decision].Name + " of a petty crime.");
                                for (int i = 1; i < GeneralPublic.Length; i++)
                                {
                                    GeneralPublic[i].AddAmnity(decision, 1);
                                }
                                break;
                        }
                        break;
                    case 2: //The actor is suspicious enough to declare someone of murder
                        PhaseDelay = ShortDelay;
                        int randomStatementGuilt = UnityEngine.Random.Range(1, 4);
                        switch (randomStatementGuilt)
                        {
                            case 1:
                                TextboxAppend(GeneralPublic[ActorID].Name + " accuses " + GeneralPublic[decision].Name + " of murder!");
                                for (int i = 1; i < GeneralPublic.Length; i++)
                                {
                                    GeneralPublic[i].AddAmnity(decision, 1);
                                }
                                break;
                            case 2:
                                TextboxAppend(GeneralPublic[ActorID].Name + " harshly interrogates " + GeneralPublic[decision].Name + " over their whereabouts the previous night.");
                                for (int i = 1; i < GeneralPublic.Length; i++)
                                {
                                    GeneralPublic[i].AddAmnity(decision, 1);
                                }
                                break;
                            case 3:
                                TextboxAppend(GeneralPublic[ActorID].Name + " accuses " + GeneralPublic[decision].Name + " of witchcraft.");
                                for (int i = 1; i < GeneralPublic.Length; i++)
                                {
                                    GeneralPublic[i].AddAmnity(decision, 1);
                                }
                                break;
                            case 4:
                                TextboxAppend(GeneralPublic[ActorID].Name + " blames " + GeneralPublic[decision].Name + " for the recent deaths.");
                                for (int i = 1; i < GeneralPublic.Length; i++)
                                {
                                    GeneralPublic[i].AddAmnity(decision, 1);
                                }
                                break;
                        }
                        break;
                }
                break;
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

    void SetupHat()
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
        if (VoteTalley.Max() >= (Alive.Length / 2))
        {
            Execution(Array.IndexOf(VoteTalley, VoteTalley.Max()));
        }
    }

    void Execution(int executee)
    {
        TextboxAppend(GeneralPublic[executee].Name + " has been found guilty. They are swiftly executed...");
        Death(executee);
    }

    void Death(int deadee)
    {
        GeneralPublic[deadee].Death();
        for (int i = 1; i < GeneralPublic.Length; i++)
        {
            GeneralPublic[i].DropSuspicion(deadee);
        }
        Alive = Alive.Where(w => w != deadee).ToArray();
        SocialIndex = SocialIndex.Where(w => w != deadee).ToArray();
        InvestigativeIndex = InvestigativeIndex.Where(w => w != deadee).ToArray();
        ProtectiveIndex = ProtectiveIndex.Where(w => w != deadee).ToArray();
        KillerIndex = KillerIndex.Where(w => w != deadee).ToArray();
        if (deadee == 0)
        {
            TextboxAppend("You are dead. Whatever the conclusion to this tale, you will not see it...");
            TextboxAppend("You loose.");
            ShowAllIcons();
            GamePhase = 999;
        }
    }


    void Murder(int MurdererId)
    {
        int decision = GeneralPublic[MurdererId].Accuse();
        while (decision == -1 || decision == MurdererId)
        {
            //if murderer doesn't hate anyone, they pick a random target (that is currently alive) ... other than themself. 
            decision = Alive[UnityEngine.Random.Range(2, Alive.Length) - 1]; 
        }
        TextboxAppend("Murderer is " + MurdererId + ", murderee is " + decision);
        if (!GeneralPublic[decision].Protected)
        {
            Death(decision);
            for (int i = 1; i < GeneralPublic.Length; i++)
            {
                GeneralPublic[i].Paranoia();
            }
            AnnouncementBuffer += (GeneralPublic[decision].Name + " was murdered during the night!"); //stores the death announcement for night's end. 
        }
        
    }

    public void ShowAllIcons()
    {
        for (int i = 1; i < GeneralPublic.Length; i++)
        {
            GeneralPublic[i].SetTrueIcon();
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
                SetupHat();
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
                SetupHat();
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
                GamePhase = 39;
                PhaseDelay = ShortDelay;
                break;
            case 39: //pre-intermission
                SetupHat();
                DayToNight();
                PhaseDelay = ShortDelay;
                GamePhase = 4;
                break;
            case 4: //Intermission
                /*    
                if (Hat.Length != 0) //if any numbers are in the hat
                {
                    rng = UnityEngine.Random.Range(1, Hat.Length) - 1; //random number in the hat
                    if (Hat[rng] == 0) //if number is player
                    {
                        GamePhase = 400; //playerphase
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray(); //remove player's number from the hat
                        PhaseDelay = MicroDelay;
                        break;
                    }
                    else
                    {
                        if (GeneralPublic[rng].Alive) ActorNight(Hat[rng]); //if actor is alive, do the night thing
                        GeneralPublic[rng].Paranoia(); //paranoia
                        //if GeneralPublic[rng]
                         //remove number from the hat
                    }
                }
                else
                { //if no numbers are in the hat
                    GamePhase = 41; //end night phase
                    PhaseDelay = LongDelay;
                    break;
                }
                GamePhase = MicroDelay;
                */
                GamePhase = 405;
                PhaseDelay = MicroDelay;
                break;
            case 405://setup social sub-phase
                Hat = SocialIndex;
                PhaseDelay = MicroDelay;
                GamePhase = 41;
                break;
            case 41: //social sub-phase
                if (Hat.Length != 0)
                {
                    rng = UnityEngine.Random.Range(1, Hat.Length) - 1;
                    if (Hat[rng] == 0) //if number is player
                    {

                        TextboxAppend("Social test: " + Hat.Length);
                        GamePhase = 400; //playerphase
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray(); //remove player's number from the hat
                        PhaseDelay = MicroDelay;
                        break;
                    }
                    else
                    {
                        rng = UnityEngine.Random.Range(1, Hat.Length) - 1;
                        TextboxAppend("InvestigativeTest: " + rng + ", " + Hat.Length);
                        switch (GeneralPublic[Hat[rng]].Role)
                        {
                            case "Escort":
                                int decision = GeneralPublic[Hat[rng]].Accuse();
                                while (decision == -1 || decision == Hat[rng])
                                {
                                    //if murderer doesn't hate anyone, they pick a random target (that is currently alive) ... other than themself. 
                                    decision = Alive[UnityEngine.Random.Range(2, Alive.Length) - 1];
                                }
                                Hat = Hat.Where(w => w != Hat[rng]).ToArray(); //remove actor's number from the hat
                                PhaseDelay = MicroDelay;
                                break;
                        }
                    }

                }
                else
                { //if no numbers are in the hat
                    GamePhase = 415; //end phase
                    PhaseDelay = MicroDelay;
                    break;
                }
                PhaseDelay = MicroDelay;
                break;
            case 415://setup investivative sub-phase
                Hat = InvestigativeIndex;
                PhaseDelay = MicroDelay;
                GamePhase = 42;
                break;
            case 42: //investigative sub-phase
                PhaseDelay = MicroDelay;
                if (Hat.Length != 0)
                {
                    rng = UnityEngine.Random.Range(1, Hat.Length) - 1;
                    if (GeneralPublic[Hat[rng]].Blocked)
                    {
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                        PhaseDelay = MicroDelay;
                        break;

                    }
                    else if (Hat[rng] == 0) //if number is player
                    {
                        TextboxAppend("Investigative test: " + Hat[rng]);
                        GamePhase = 400; //playerphase
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray(); //remove player's number from the hat
                        //PhaseDelay = MicroDelay;
                        break;
                    }
                    else
                    {
                        rng = UnityEngine.Random.Range(1, Hat.Length) - 1;
                        TextboxAppend("InvestigativeTest: " + rng + ", " + Hat.Length);
                        switch (GeneralPublic[Hat[rng]].Role)
                        {
                            case "Seer":
                                for (int i = 1; i < 4; i++)
                                {
                                    int decisionSeer = Alive[UnityEngine.Random.Range(1, Alive.Length) - 1];
                                    if (decisionSeer == Hat[rng])
                                    {
                                        i--;
                                    }
                                    else
                                    {
                                        if (!GeneralPublic[decisionSeer].isEvil){
                                            GeneralPublic[Hat[rng]].DropSuspicion(decisionSeer);
                                        }
                                    }
                                }
                                Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                                //PhaseDelay = MicroDelay;
                                break;
                            case "Investigator":
                                int decisionInvestigator = Alive[UnityEngine.Random.Range(1, Alive.Length) - 1];
                                if (GeneralPublic[decisionInvestigator].isEvil){
                                    GeneralPublic[Hat[rng]].AddAmnity(decisionInvestigator, 100);
                                }
                                else
                                {
                                    GeneralPublic[Hat[rng]].DropSuspicion(decisionInvestigator);
                                }
                                Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                                //PhaseDelay = MicroDelay;
                                break;
                        }
                    }
                }
                else
                { //if no numbers are in the hat
                    GamePhase = 425; //end phase
                    //PhaseDelay = MicroDelay;
                    break;
                }
                //PhaseDelay = MicroDelay;
                break;
            case 425://setup protective sub-phase
                Hat = ProtectiveIndex;
                PhaseDelay = MicroDelay;
                GamePhase = 43;
                break;
            case 43: //protective sub-phase
                if (Hat.Length != 0)
                {
                    //TextboxAppend("Protective test: " + Hat[rng]);
                    rng = UnityEngine.Random.Range(1, Hat.Length) - 1;
                    if (GeneralPublic[Hat[rng]].Blocked)
                    {
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                        PhaseDelay = MicroDelay;
                        break;

                    }
                    else if(Hat[rng] == 0) //if number is player
                    {
                        GamePhase = 400; //playerphase
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray(); //remove player's number from the hat
                        PhaseDelay = MicroDelay;
                        break;
                    }
                }
                else
                { //if no numbers are in the hat
                    GamePhase = 435; //end phase
                    PhaseDelay = MicroDelay;
                    break;
                }
                PhaseDelay = MicroDelay;
                break;
            case 435://setup killer sub-phase
                Hat = KillerIndex;
                PhaseDelay = MicroDelay;

                GamePhase = 44;
                break;
            case 44: //killer sub-phase
                //TextboxAppend("Hate length is " + Hat.Length); //debug feedback
                if (Hat.Length != 0)
                {
                    rng = UnityEngine.Random.Range(1, Hat.Length) - 1;
                    if (GeneralPublic[Hat[rng]].Blocked)
                    {
                        Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                        PhaseDelay = MicroDelay;
                        break;

                    }
                    else switch(GeneralPublic[Hat[rng]].Role)
                    {
                        case "Murderer":
                            Murder(Hat[rng]);
                            Hat = Hat.Where(w => w != Hat[rng]).ToArray();
                            PhaseDelay = MicroDelay;
                            break;

                    }
                }
                else
                { //if no numbers are in the hat
                    GamePhase = 49; //end phase
                    PhaseDelay = MicroDelay;
                    break;
                }
                PhaseDelay = MicroDelay;
                break;
            case 45: //Intermission Ends
                //TextboxAppend("debug: post-intermission");
                NighttoDay();
                PhaseDelay = MicroDelay;
                GamePhase = 1;
                break;
            case 49: //post-intermission
                TextboxAppend(AnnouncementBuffer);
                AnnouncementBuffer = ""; //empties the announcement buffer
                NighttoDay();
                PhaseDelay = StandardDelay;
                int EvilTalley = 0;
                for (int i = 1; i < GeneralPublic.Length; i++)
                {
                    if (GeneralPublic[i].isEvil) EvilTalley++;
                    GeneralPublic[i].Blocked = false;
                    GeneralPublic[i].Protected = false;
                }
                if (EvilTalley == 0){
                    GamePhase = 1000; //game end  (win)
                    PhaseDelay = ShortDelay;
                    TextboxAppend("Come morning, no innocent are found dead. Or the night after that, or after that");

                }
                else GamePhase = 1;
                
                break;
            case 400: //player's turn to night action
                switch (GeneralPublic[0].Role)
                {
                    case "Citizen":
                        TextboxAppend("The night is peaceful enough, though in the silence the paranoia of the villagers only grows...");
                        GamePhase = 4;
                        PhaseDelay = MicroDelay;
                        break;
                }
                    
                
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
                //DebugShowIcon();
                GamePhase = 4;
                PhaseDelay = ShortDelay;
                break;
            case 1000:
                TextboxAppend("After a week has passed, whom remains are sure that the troubled times has passed");
                PhaseDelay = ShortDelay;
                GamePhase = 1001;
                break;
            case 1001:
                TextboxAppend("You soon resume your journey, taking the well wishes of the village with you");
                GamePhase = 1002;
                PhaseDelay = ShortDelay;
                break;
            case 1002:
                TextboxAppend("\n\nYou have won.");
                ShowAllIcons();
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

}

class Actor
{
    public string Name;
    public int ID;
    public string Role;
    public int[] Amnity;
    public GameObject Avatar;
    public int[] Suspicion;
    public int[] KnownInnocent; //This is used by special roles, where an actor identified a suspect
    public bool Alive;
    public Sprite Icon;
    public Sprite TrueIcon;
    public bool isEvil; //Simpler indicator of if the actor is an "antagonist"
    public bool Blocked;
    public bool Protected;
    //private Random random;
    public Actor(int ActorNum, int totalActors, Sprite StartingIcon)
    { //Initializes an actor with default values
        ID = ActorNum;
        Amnity = new int[totalActors];
        Suspicion = new int[totalActors];
        Alive = true;
        Role = "Citizen";
        Icon = StartingIcon;
        TrueIcon = Icon;
        isEvil = false;
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
    public int Verify()
    { //Investigative exclusive, can return confirmed evil or innocent.
        if (Suspicion.Max() > 99)
        {
            return Array.IndexOf(Suspicion, Suspicion.Max());
        }
        return -1;
    }
    public void Death()
    {
        Alive = false;
        Icon = TrueIcon;
        Avatar.GetComponent<ActorScript>().SetIcon(Icon);
        Avatar.GetComponent<ActorScript>().SetAnkh(true);
    }
    public void SetIcon(Sprite InIcon)
    {
        Icon = InIcon;
        Avatar.GetComponent<ActorScript>().SetIcon(Icon);
    }
    public void SetTrueIcon()
    {
        Avatar.GetComponent<ActorScript>().SetIcon(TrueIcon);
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
