using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class blueboy_moveScript : MonoBehaviour {
    public float curSpeed;
    public enum Direction { North, East, South, West, Stop };
    public Direction moveDir;
    public Direction lastDir;

    public int MovX;
    public int MovY;

    Dictionary<string, Direction> moveDict = new Dictionary<string, Direction>();

    // Use this for initialization
    void Start() {
        moveDir = Direction.South;
        lastDir = Direction.South;

        moveDict.Add("NORTH", Direction.North);
        moveDict.Add("WEST", Direction.West);
        moveDict.Add("EAST", Direction.East);
        moveDict.Add("SOUTH", Direction.South);
        moveDict.Add("EQUIP", lastDir);
        moveDict.Add("LEAVE", lastDir);
    }

    // Update is called once per frame
    void FixedUpdate() {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * curSpeed, 0.8f),
        //                          Mathf.Lerp(0, Input.GetAxis("Vertical") * curSpeed, 0.8f));
        calcMove(moveDir);
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Lerp(0, MovX * curSpeed, 0.8f),
                                  Mathf.Lerp(0, MovY * curSpeed, 0.8f));
    }

    void calcMove(Direction direction) {
        switch (direction) {
            case Direction.North:
                MovX = 0;
                MovY = 1;
                transform.position = new Vector2(Mathf.Round(transform.position.x * 4f) * 0.25f, transform.position.y);
                break;
            case Direction.East:
                MovX = 1;
                MovY = 0;
                transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y * 4f) * 0.25f);
                break;
            case Direction.South:
                MovX = 0;
                MovY = -1;
                transform.position = new Vector2(Mathf.Round(transform.position.x * 4f) * 0.25f, transform.position.y);
                break;
            case Direction.West:
                MovX = -1;
                MovY = 0;
                transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y * 4f) * 0.25f);
                break;
            case Direction.Stop:
                MovX = 0;
                MovY = 0;
                transform.position = new Vector2(Mathf.Round(transform.position.x * 4f) * 0.25f, Mathf.Round(transform.position.y * 4f) * 0.25f);
                break;
        }
    }

    void ChooseDirection(string dir) {
        lastDir = moveDir;
        moveDir = moveDict[dir];
    }
}