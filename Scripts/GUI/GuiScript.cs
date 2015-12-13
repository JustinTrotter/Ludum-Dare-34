using UnityEngine;
using System.Collections;

public class GuiScript : MonoBehaviour {

    public AutoType atScript;

    public GameObject player;
    public GUISkin skin;
    public GameObject battleMgr;

    public bool showWindow;
    public bool showBattle;

    float pX;
    float pY;

    private Rect windowTL = new Rect(50, 50, 500, 500);
    private Rect windowTR = new Rect(Screen.width - 550, 50, 500, 500);
    private Rect windowBL = new Rect(50, Screen.height - 550, 500, 500);
    private Rect windowBR = new Rect(Screen.width - 550, Screen.height - 550, 500, 500);

    private Rect windowBattle = new Rect(32, 32, Screen.width - 64, Screen.height - 64);

    public Texture thugTexture;
    public Texture attackTexture;
    public GameObject monsterFighting;

    private string statusText = "------Status------\nLevel:\nHP:\nAttack:\nDefence:\nXP:\n\n------Equipment------\nWeapon:\nArmor:\n\n------Quest: Recover Lost Sword and Armor------\nEnemies Left:";
    private string eventText = "";
    public string button1Text = "";
    public string button2Text = "";
    private string statusValues;


    public string battleText;
    public string typedText = "";

    public string battleButton1Text = "KICK ITS\nFACE IN";
    public string battleButton2Text = "RUN AWAY";

    private GameObject usedItem;

    public AudioClip equipSound;
    public AudioClip selectSound;

    public GUIStyle battleTextStyle;
    public bool typing;
    public bool typingDone;
    public bool attacking;
    public bool victory;

    private GameObject soundfx;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        soundfx = GameObject.FindGameObjectWithTag("SoundFX");
        battleMgr = GameObject.FindGameObjectWithTag("Battle");
        showWindow = false;
        showBattle = false;
        typing = false;
        typingDone = true;
        attacking = false;
        victory = false;

        battleTextStyle.normal.textColor = Color.white;
        if (monsterFighting != null)
            battleText = monsterFighting.GetComponent<statsScript>().Name + " approaches you.\nIt looks really pissed off!\nWhat do you do?";
    }

    // Update is called once per frame
    void Update() {
        //if(monsterFighting != null)
        //battleText = monsterFighting.GetComponent<statsScript>().Name + " approaches you.\nIt looks really pissed off!\nWhat do you do?";
        statusValues = "\n" +
            player.GetComponent<statsScript>().level + "\n" +
            player.GetComponent<statsScript>().health + "/" + player.GetComponent<statsScript>().maxHealth + "\n" +
            player.GetComponent<statsScript>().attack + "\n" +
            player.GetComponent<statsScript>().defense + "\n" +
            player.GetComponent<statsScript>().experience + "/" + player.GetComponent<statsScript>().maxExperience + "\n" +
            "\n" +
            "\n" +
            player.GetComponent<statsScript>().weapon + "\n" +
            player.GetComponent<statsScript>().armor + "\n" +
            "\n" +
            "\n" +
            player.GetComponent<statsScript>().remainingEnemies;

        if (!typing && showBattle) {
            typing = true;
            typingDone = false;
            typedText = battleText;
            GetComponent<AutoType>().doTypeText();
        }

    }

    void FixedUpdate() {
        pX = player.transform.position.x;
        pY = player.transform.position.y;
    }


    void OnGUI() {
        GUI.skin = skin;
        if (showWindow) {
            if (pX > 0 && pY < 0)
                windowTL = GUI.Window(0, windowTL, WindowFunction, "");
            if (pX < 0 && pY < 0)
                windowTR = GUI.Window(1, windowTR, WindowFunction, "");
            if (pX > 0 && pY > 0)
                windowBL = GUI.Window(2, windowBL, WindowFunction, "");
            if (pX < 0 && pY > 0)
                windowBR = GUI.Window(3, windowBR, WindowFunction, "");
        }

        if (showBattle) {
            
            windowBattle = GUI.Window(4, windowBattle, WindowBattleFunction, "------BATTLE------");
        }
    }

    void WindowBattleFunction( int windowID) {
        GUI.DrawTexture(new Rect(windowBattle.width / 5, 64, 448 * 2, 256 * 2), thugTexture);
        if(attacking)
            GUI.DrawTexture(new Rect(windowBattle.width / 2 - 150, windowBattle.height / 4, 256, 256), attackTexture);
        GUI.Label(new Rect(windowBattle.width - 300, 25, 400, 400), statusText);
        GUI.Label(new Rect(windowBattle.width - 300 + 140, 25, 400, 400), statusValues);
        GUI.Label(new Rect(Screen.width / 5, 575, 448 * 3, 256 * 2), typedText, battleTextStyle);

        if (typingDone)
        {
            if (GUI.Button(new Rect(Screen.width / 8 + 100, 775, 300, 175), battleButton1Text))
                doAction(battleButton1Text);
            if (GUI.Button(new Rect(Screen.width / 3 + 400, 775, 300, 175), battleButton2Text))
                doAction(battleButton2Text);
        }
    }

    void doAction(string text)
    {
        switch (text) {
            case "COUNTER\nATTACK":
            case "NUT PUNCH":
            case "KICK ITS\nFACE IN":
            case "FLAIL ARMS\nWILDLY":
                battleMgr.GetComponent<battleScript>().doAttack(player, monsterFighting);
                break;
            case "SNEAK\nAWAY":
            case "FLEE":
            case "RUN AWAY":
            case "NOPE OUT":
                battleMgr.GetComponent<battleScript>().doRunAway(player, monsterFighting);
                break;
            case "DUCK":
            case "DIP":
            case "DIVE":
            case "DODGE":
                battleMgr.GetComponent<battleScript>().doDodge(player, monsterFighting);
                break;
            case "STAND THE\nLINE":
            case "HOLD YOUR\nGROUND":
            case "DEFEND":
            case "MAN UP":
                battleMgr.GetComponent<battleScript>().doDefend(player, monsterFighting);
                break;
            case "GRAPPLE":
            case "JUDO\nTHROW":
            case "GROPE":
            case "STRANGLE":
                battleMgr.GetComponent<battleScript>().doGrapple(player, monsterFighting);
                break;
            case "SPARE":
            case "FLIRT":
            case "PACIFISM":
            case "MAKE\nPEACE":
                battleMgr.GetComponent<battleScript>().doSpare(player, monsterFighting);
                break;
            case "CONTINUE":
                showBattle = false;
                victory = true;
                showWindow = true;
                player.GetComponent<blueboy_moveScript>().moveDir = player.GetComponent<blueboy_touchScript>().victoryDirection;
                break;
            
        }
    }

    void WindowFunction (int windowID)
    {
        GUI.Label(new Rect(30, 25, 400, 400), statusText);
        GUI.Label(new Rect(140, 25, 400, 400), statusValues);
        GUI.Label (new Rect( 30, 285, 400, 400), eventText);
        if (GUI.Button(new Rect(30, 375, 200, 100), button1Text)) {
            player.transform.SendMessage("ChooseDirection", button1Text);
            if (button1Text == "EQUIP" || button1Text == "USE") {
                usedItem.transform.SendMessage("UseItem");
                soundfx.transform.SendMessage("PlaySound", equipSound);
                player.GetComponent<blueboy_moveScript>().moveDir = player.GetComponent<blueboy_touchScript>().itemDirection;
            }
            else
                soundfx.transform.SendMessage("PlaySound", selectSound);
            eventText = "";
        }
        
            if (GUI.Button(new Rect(280, 375, 200, 100), button2Text))
            {
                player.transform.SendMessage("ChooseDirection", button2Text);
                if (button2Text == "EQUIP" || button2Text == "USE")
                {
                    usedItem.transform.SendMessage("UseItem");
                    soundfx.transform.SendMessage("PlaySound", equipSound);
                    player.GetComponent<blueboy_moveScript>().moveDir = player.GetComponent<blueboy_touchScript>().itemDirection;
            }
                else
                    soundfx.transform.SendMessage("PlaySound", selectSound);
                eventText = "";
            }
        
    }

    void UpdateEventText (string msg){
        eventText += msg;
    }

    void UpdateButtons(string[] buttonText) {
        button1Text = buttonText[0];
        button2Text = buttonText[1];
    }

    void UpdateItem(GameObject item) {
        usedItem = item;
    }
}
