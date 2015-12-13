using UnityEngine;
using System.Collections;

public class blueboy_sightScript : MonoBehaviour {
    public float viewDistance;
    public Vector2[] directions;
    private LayerMask mask = 1 << 10;
    private GameObject gui;
    
    // Use this for initialization
    void Start () {
        gui = GameObject.FindGameObjectWithTag("GUI");
        directions = new Vector2[] { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        mask = ~mask;
    }
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate() {
        foreach (Vector2 v in directions) {
            doRayCast(v);
        }
    }

    void doRayCast(Vector2 direction) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, viewDistance, mask);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider != null) {
                if (hits[i].collider.tag == "Fog")
                    hits[i].transform.SendMessage("HitByRay");
                else
                    break;
            }
        }
    }

    void LookForMonsters() {
        foreach (Vector2 direction in directions) {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, viewDistance + 1, mask);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null)
                {
                    if (hits[i].collider.tag == "Monster")
                        gui.transform.SendMessage("UpdateEventText", "\nYou see a monster!\n");
                    //else
                        //break;
                }
            }
        }
    }
}
