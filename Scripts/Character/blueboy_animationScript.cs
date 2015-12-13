using UnityEngine;
using System.Collections;

public class blueboy_animationScript : MonoBehaviour {
   // public enum Direction { North, East, South, West };
   // public Direction facing;

    // Use this for initialization
    void Start () {
        //facing = Direction.South;
	}
	
	// Update is called once per frame
	void Update () {
        updateFacing();
	}

    void updateFacing(){
        if (GetComponent<Rigidbody2D>().velocity.x > 0) {
            //facing = Direction.East;
            GetComponent<Animator>().SetInteger("Direction",6);
        }
        if (GetComponent<Rigidbody2D>().velocity.x < 0) {
            //facing = Direction.West;
            GetComponent<Animator>().SetInteger("Direction", 4);
        }
        if (GetComponent<Rigidbody2D>().velocity.y > 0) {
            //facing = Direction.North;
            GetComponent<Animator>().SetInteger("Direction", 8);
        }
        if (GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            //facing = Direction.South;
            GetComponent<Animator>().SetInteger("Direction", 2);
        }
    }

    void updateAnimation(){

    }
}
