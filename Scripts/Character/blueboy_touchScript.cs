using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class blueboy_touchScript : MonoBehaviour {
    public float touchDistance;
    public bool atIntersection;

    public AudioClip popupSound;

    public string intersectionMsg = "You have encountered an intersection.\n";
    private string itemMsg = "You have found an item: ";
    private GameObject gui;
    private GameObject soundfx;
    private GameObject battleMgr;

    public blueboy_moveScript.Direction victoryDirection;
    public blueboy_moveScript.Direction itemDirection;

    Dictionary<Vector2, Vector2[]> directionDict = new Dictionary<Vector2, Vector2[]>();
    Dictionary<Vector2, string> stringDict = new Dictionary<Vector2, string>();
    Dictionary<Vector2, Vector2[]> otherDirectionsDict = new Dictionary<Vector2, Vector2[]>();
    Dictionary<Vector2, blueboy_moveScript.Direction> moveDirDict = new Dictionary<Vector2, blueboy_moveScript.Direction>();
    Dictionary<Vector2, blueboy_moveScript.Direction> turnAroundDict = new Dictionary<Vector2, blueboy_moveScript.Direction>();
    Dictionary<blueboy_moveScript.Direction, Vector2> getVect2Dict = new Dictionary<blueboy_moveScript.Direction, Vector2>();

    // Use this for initialization
    void Start () {
        victoryDirection = blueboy_moveScript.Direction.Stop;
        itemDirection = blueboy_moveScript.Direction.Stop;
        gui = GameObject.FindGameObjectWithTag("GUI");
        soundfx = GameObject.FindGameObjectWithTag("SoundFX");
        battleMgr = GameObject.FindGameObjectWithTag("Battle");

        directionDict.Add(Vector2.up, new Vector2[] { Vector2.left, Vector2.right });
        directionDict.Add(Vector2.left, new Vector2[] { Vector2.up, Vector2.down });
        directionDict.Add(Vector2.right, new Vector2[] { Vector2.up, Vector2.down });
        directionDict.Add(Vector2.down, new Vector2[] { Vector2.left, Vector2.right });

        stringDict.Add(Vector2.up, "NORTH" );
        stringDict.Add(Vector2.left, "WEST" );
        stringDict.Add(Vector2.right, "EAST" );
        stringDict.Add(Vector2.down,  "SOUTH" );

        otherDirectionsDict.Add(Vector2.up, new Vector2[] { Vector2.left, Vector2.right, Vector2.up });
        otherDirectionsDict.Add(Vector2.left, new Vector2[] { Vector2.up, Vector2.down, Vector2.left });
        otherDirectionsDict.Add(Vector2.right, new Vector2[] { Vector2.up, Vector2.down, Vector2.right });
        otherDirectionsDict.Add(Vector2.down, new Vector2[] { Vector2.left, Vector2.right, Vector2.down });

        moveDirDict.Add(Vector2.up, blueboy_moveScript.Direction.North);
        moveDirDict.Add(Vector2.left, blueboy_moveScript.Direction.West);
        moveDirDict.Add(Vector2.right, blueboy_moveScript.Direction.East);
        moveDirDict.Add(Vector2.down, blueboy_moveScript.Direction.South);

        turnAroundDict.Add(Vector2.up, blueboy_moveScript.Direction.South);
        turnAroundDict.Add(Vector2.left, blueboy_moveScript.Direction.East);
        turnAroundDict.Add(Vector2.right, blueboy_moveScript.Direction.West);
        turnAroundDict.Add(Vector2.down, blueboy_moveScript.Direction.North);

        getVect2Dict.Add(blueboy_moveScript.Direction.South, Vector2.down);
        getVect2Dict.Add(blueboy_moveScript.Direction.East, Vector2.right);
        getVect2Dict.Add(blueboy_moveScript.Direction.West, Vector2.left);
        getVect2Dict.Add(blueboy_moveScript.Direction.North, Vector2.up);
        getVect2Dict.Add(blueboy_moveScript.Direction.Stop, Vector2.zero);

        atIntersection = false;
    }
	
	// Update is called once per frame 
	void Update () {
	
	}

    void FixedUpdate() {
        doRayCastWall(getVect2Dict[GetComponent<blueboy_moveScript>().moveDir], true);
        atIntersection = doRayCastFloor(getVect2Dict[GetComponent<blueboy_moveScript>().moveDir]);
        if (!atIntersection)
            gui.GetComponent<GuiScript>().showWindow = false;
    }

    bool doRayCastFloor(Vector2 direction) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, 1);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider != null) {
                if (hits[i].collider.tag == "Intersection") {
                    if (!atIntersection) {
                        GetComponent<blueboy_moveScript>().moveDir = blueboy_moveScript.Direction.Stop;
                        gui.GetComponent<GuiScript>().showWindow = true;
                        gui.transform.SendMessage("UpdateEventText", intersectionMsg);
                        gui.transform.SendMessage("UpdateButtons", getAvailableDirections(direction));
                        transform.SendMessage("LookForMonsters");
                        gui.transform.SendMessage("UpdateEventText", "\nWhich direction do you want to go?");
                        soundfx.transform.SendMessage("PlaySound", popupSound);

                    }
                    return true;
                }
                if (hits[i].collider.tag == "Item") {
                    if (!atIntersection) {
                        itemDirection = GetComponent<blueboy_moveScript>().moveDir;
                        GetComponent<blueboy_moveScript>().moveDir = blueboy_moveScript.Direction.Stop;
                        gui.GetComponent<GuiScript>().showWindow = true;
                        gui.transform.SendMessage("UpdateEventText", itemMsg + hits[i].transform.GetComponent<itemScript>().itemName + "\n");
                        gui.transform.SendMessage("UpdateItem", hits[i].transform.gameObject);
                        if (hits[i].transform.GetComponent<itemScript>().weapon || hits[i].transform.GetComponent<itemScript>().armor) {   
                            gui.transform.SendMessage("UpdateButtons", new string[] { "EQUIP", "LEAVE" });
                        }
                        else
                            gui.transform.SendMessage("UpdateButtons", new string[] { "USE", "LEAVE" });
                        transform.SendMessage("LookForMonsters");
                        gui.transform.SendMessage("UpdateEventText", "What do you want to do?");
                        soundfx.transform.SendMessage("PlaySound", popupSound);
                    }
                    return true;
                }

                if (hits[i].collider.tag == "Monster") {
                    if (!atIntersection) {
                        victoryDirection = GetComponent<blueboy_moveScript>().moveDir;
                        GetComponent<blueboy_moveScript>().moveDir = blueboy_moveScript.Direction.Stop;
                        //Start Monster Battle!
                        battleMgr.transform.SendMessage("StartBattle", hits[i].transform.gameObject);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public string[] getAvailableDirections(Vector2 direction) {
        int count = 0;
        string[] str = new string[] { "", "", "" };
        for (int i = 0; i < otherDirectionsDict[direction].Length; i++) {
            if (doRayCastWall(otherDirectionsDict[direction][i], false)) {
                str[count] = stringDict[otherDirectionsDict[direction][i]];
                count++;
            }
        }
        return new string[] { str[0], str[1] };
    }

    bool doRayCastWall(Vector2 direction, bool recur) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, touchDistance);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider != null) {
                if (hits[i].collider.tag == "Wall"){
                    if (recur)
                        touchedWall(direction);
                    return false;
                }
            }
        }
        return true;
    }

    void touchedWall(Vector2 direction) {
        GetComponent<blueboy_moveScript>().moveDir = blueboy_moveScript.Direction.Stop;
        bool found = false;
        for (int i = 0; i < directionDict[direction].Length; i++) {
            if(doRayCastWall(directionDict[direction][i], false)) {
                found = true;
                GetComponent<blueboy_moveScript>().lastDir = GetComponent<blueboy_moveScript>().moveDir;
                GetComponent<blueboy_moveScript>().moveDir = moveDirDict[directionDict[direction][i]];
                break;
            }
        }
        if (!found)
            GetComponent<blueboy_moveScript>().moveDir = turnAroundDict[direction];  
    }
}
