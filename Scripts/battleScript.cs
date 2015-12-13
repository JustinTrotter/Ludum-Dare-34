using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class battleScript : MonoBehaviour {

    public AudioClip encounterSound;
    public AudioClip forestMusic;
    public AudioClip battleMusic;
    public AudioClip attackSound;
    public AudioClip hurtSound;

    private GameObject gui;
    private GameObject soundfx;
    private GameObject musicMgr;

    private GameObject monsterFighting;

    private string[] attacks;
    private string[] defends;
    private string[] flees;
    private string[] grabs;
    private string[] dodges;
    private string[] spares;

    private string[] actions;
    private string[] intentions;

    private string[] firstGroup;

    private Dictionary<string, string[]> actionDict = new Dictionary<string, string[]>();

    // Use this for initialization
    void Start () {
        gui = GameObject.FindGameObjectWithTag("GUI");
        soundfx = GameObject.FindGameObjectWithTag("SoundFX");
        musicMgr = GameObject.FindGameObjectWithTag("Music");

        attacks = new string[] { "COUNTER\nATTACK", "NUT PUNCH", "KICK ITS\nFACE IN", "FLAIL ARMS\nWILDLY"};
        defends = new string[] { "STAND THE\nLINE", "HOLD YOUR\nGROUND","DEFEND", "MAN UP"};
        flees = new string[] { "SNEAK\nAWAY","FLEE", "RUN AWAY", "NOPE OUT"};
        grabs = new string[] { "GRAPPLE", "JUDO\nTHROW", "GROPE", "STRANGLE" };
        dodges = new string[] { "DUCK", "DIP", "DIVE", "DODGE" };
        spares = new string[] { "SPARE", "FLIRT", "PACIFISM", "MAKE\nPEACE" };

        actions = new string[] {"attack", "defend", "flee", "grab", "dodge","spare"};
        intentions = new string[] {"like its preparing to charge", "distracted", "dazzed", "pissed Off",  "confused", "depressed", "curious"};

        actionDict.Add("attack", attacks);
        actionDict.Add("defend", defends);
        actionDict.Add("flee", flees);
        actionDict.Add("grab", grabs);
        actionDict.Add("dodge", dodges);
        actionDict.Add("spare", spares);
    }
	
	// Update is called once per frame
	void Update () {
    }

    string getRandomAction(bool first) {
        
        string[] group = actionDict[actions[Random.Range(0, actions.Length)]];
        if (first)
            firstGroup = group;
        else {
            if (firstGroup == group)
                return getRandomAction(false);
        }
        return group[Random.Range(0, group.Length)];
    }

    string getRandomIntention() {
        return intentions[Random.Range(0, intentions.Length)];
    }

    void generateNextTurn(GameObject defender) {
        gui.GetComponent<GuiScript>().battleButton1Text = getRandomAction(true);
        gui.GetComponent<GuiScript>().battleButton2Text = getRandomAction(false);
        gui.GetComponent<GuiScript>().monsterFighting.GetComponent<statsScript>().intent = getRandomIntention();
        gui.GetComponent<GuiScript>().battleText += "\n" + defender.GetComponent<statsScript>().Name + " looks " + gui.GetComponent<GuiScript>().monsterFighting.GetComponent<statsScript>().intent + ".";
        gui.GetComponent<GuiScript>().typing = false;
    }

    void StartBattle(GameObject monster) {
        gui.GetComponent<GuiScript>().monsterFighting = monster;
        soundfx.transform.SendMessage("PlaySound", encounterSound);
        gui.GetComponent<GuiScript>().showBattle = true;
        musicMgr.GetComponent<musicManagerScript>().changeTrack(battleMusic);
        gui.GetComponent<GuiScript>().battleButton1Text = getRandomAction(true);
        gui.GetComponent<GuiScript>().battleButton2Text = getRandomAction(false);
        gui.GetComponent<GuiScript>().monsterFighting.GetComponent<statsScript>().intent = getRandomIntention();
        gui.GetComponent<GuiScript>().typedText = "";
        gui.GetComponent<GuiScript>().battleText = "\n" +  monster.GetComponent<statsScript>().Name + " looks " + gui.GetComponent<GuiScript>().monsterFighting.GetComponent<statsScript>().intent + ".";

    }

    IEnumerator attackAnimation() {
        yield return new WaitForSeconds(.5f);
        gui.GetComponent<GuiScript>().attacking = false;
    }

    public int checkIfDead(GameObject attacker, GameObject defender)
    {
        if (attacker.GetComponent<statsScript>().health <= 0)
        {
            Debug.Log("GAMEOVER!");
            return -1;
        }
        if (defender.GetComponent<statsScript>().health <= 0)
        {
            return 1;
        }
        return 0;
    }

    public int doAttack(GameObject attacker, GameObject defender) {
        soundfx.transform.SendMessage("PlaySound", attackSound);
        defender.GetComponent<statsScript>().health -= attacker.GetComponent<statsScript>().attack - defender.GetComponent<statsScript>().defense;
        gui.GetComponent<GuiScript>().attacking = true;
        StartCoroutine(attackAnimation());
        gui.GetComponent<GuiScript>().typedText = "";
        gui.GetComponent<GuiScript>().battleText = "You kick " + defender.GetComponent<statsScript>().Name + " in the face.\nYou just pissed it off even more!";
        if (checkIfDead(attacker, defender) != 0)
            return EndBattle(attacker, defender);
        generateNextTurn(defender);
        return 1;
    }

    public int doRunAway(GameObject attacker, GameObject defender) {
        soundfx.transform.SendMessage("PlaySound", hurtSound);
        attacker.GetComponent<statsScript>().health -= defender.GetComponent<statsScript>().attack - attacker.GetComponent<statsScript>().defense;
            gui.GetComponent<GuiScript>().typedText = "";
            gui.GetComponent<GuiScript>().battleText = "You fail to escape.\n" + defender.GetComponent<statsScript>().Name + " attacks from behind.";
            if (checkIfDead(attacker, defender) != 0)
                return EndBattle(attacker, defender);
            generateNextTurn(defender);
            return 1;
    }

    public int doDefend(GameObject attacker, GameObject defender) {
        soundfx.transform.SendMessage("PlaySound", hurtSound);
        attacker.GetComponent<statsScript>().health -= (defender.GetComponent<statsScript>().attack - attacker.GetComponent<statsScript>().defense) / 2;
        gui.GetComponent<GuiScript>().typedText = "";
        gui.GetComponent<GuiScript>().battleText = "You block the incoming attack.\nYou take half the damage!";
        if (checkIfDead(attacker, defender) != 0)
            return EndBattle(attacker, defender);
        generateNextTurn(defender);
        return 1;
    }

    public int doGrapple(GameObject attacker, GameObject defender) {
        if (defender.GetComponent<statsScript>().intent == "like its preparing to charge") {
            soundfx.transform.SendMessage("PlaySound", hurtSound);
            attacker.GetComponent<statsScript>().health -= defender.GetComponent<statsScript>().attack - attacker.GetComponent<statsScript>().defense;
            gui.GetComponent<GuiScript>().typedText = "";
            gui.GetComponent<GuiScript>().battleText = defender.GetComponent<statsScript>().Name + " crashes into you.\nYou fail to get a grip.";
            if (checkIfDead(attacker, defender) != 0)
                return EndBattle(attacker, defender);
            generateNextTurn(defender);
        }
        else {
            soundfx.transform.SendMessage("PlaySound", attackSound);
            defender.GetComponent<statsScript>().health -= attacker.GetComponent<statsScript>().attack - defender.GetComponent<statsScript>().defense;
            gui.GetComponent<GuiScript>().attacking = true;
            StartCoroutine(attackAnimation());
            gui.GetComponent<GuiScript>().typedText = "";
            gui.GetComponent<GuiScript>().battleText = "You lock " + defender.GetComponent<statsScript>().Name + " arm.\nLeg. Apendage? Whatever...";
            if (checkIfDead(attacker, defender) != 0)
                return EndBattle(attacker, defender);
            generateNextTurn(defender);
        }
        return 1;
    }

    public int doSpare(GameObject attacker, GameObject defender) {
        soundfx.transform.SendMessage("PlaySound", hurtSound);
        attacker.GetComponent<statsScript>().health -= (defender.GetComponent<statsScript>().attack - attacker.GetComponent<statsScript>().defense) * 2;
        gui.GetComponent<GuiScript>().typedText = "";
        gui.GetComponent<GuiScript>().battleText = "You Idiot...Did you think this was Undertale?\nYou take DOUBLE damage.";
        if (checkIfDead(attacker, defender) != 0)
            return EndBattle(attacker, defender);
        generateNextTurn(defender);
        return 1;
    }

    public int doDodge(GameObject attacker, GameObject defender){
        if (defender.GetComponent<statsScript>().intent == "like its preparing to charge") {
            soundfx.transform.SendMessage("PlaySound", attackSound);
            defender.GetComponent<statsScript>().health -= attacker.GetComponent<statsScript>().attack - defender.GetComponent<statsScript>().defense;
            gui.GetComponent<GuiScript>().attacking = true;
            StartCoroutine(attackAnimation());
            gui.GetComponent<GuiScript>().typedText = "";
            gui.GetComponent<GuiScript>().battleText = "You dodge successfully.\n" + defender.GetComponent<statsScript>().Name + " crashes onto the ground.";
            if (checkIfDead(attacker, defender) != 0)
                return EndBattle(attacker, defender);
            generateNextTurn(defender);
            return 1;
        }
        else {
            soundfx.transform.SendMessage("PlaySound", hurtSound);
            attacker.GetComponent<statsScript>().health -= defender.GetComponent<statsScript>().attack - attacker.GetComponent<statsScript>().defense;
            gui.GetComponent<GuiScript>().typedText = "";
            gui.GetComponent<GuiScript>().battleText = "You trip over your own feet.\n" + defender.GetComponent<statsScript>().Name + " strikes you while youre down!";
            if (checkIfDead(attacker, defender) != 0)
                return EndBattle(attacker, defender);
            generateNextTurn(defender);
            return 1;
        }
    }

    int EndBattle(GameObject attacker, GameObject defender) {
        attacker.GetComponent<statsScript>().remainingEnemies--;
        musicMgr.GetComponent<musicManagerScript>().changeTrack(forestMusic);
        attacker.GetComponent<statsScript>().experience += defender.GetComponent<statsScript>().experience;
        gui.GetComponent<GuiScript>().typedText = "";
        gui.GetComponent<GuiScript>().battleText = "You are victorious!\nYou have gained " + defender.GetComponent<statsScript>().experience + " XP.";
        gui.GetComponent<GuiScript>().battleButton1Text = "CONTINUE";
        gui.GetComponent<GuiScript>().battleButton2Text = "CONTINUE";
        gui.GetComponent<GuiScript>().typing = false;
        Destroy(gui.GetComponent<GuiScript>().monsterFighting);
        return 0;
    }
}
