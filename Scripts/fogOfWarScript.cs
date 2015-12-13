using UnityEngine;
using System.Collections;

public class fogOfWarScript : MonoBehaviour {
    public float opacity;
    public bool gettingHit = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void FixedUpdate() {
        updateOpacity();
    }

    void updateOpacity() {
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, opacity);
        if (opacity == 0f && !gettingHit)
            opacity = .5f;
        gettingHit = false;
    }

    void HitByRay() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, .5f);
        bool containsWall = false;
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider != null) {
                if (hits[i].collider.tag == "Wall")
                    containsWall = true;
            }
        }
        if (!containsWall) {
            opacity = 0f;
            gettingHit = true;
            doRayCast(Vector2.up);
            doRayCast(Vector2.left);
            doRayCast(Vector2.right);
            doRayCast(Vector2.down);
        }
    }

    void doRayCast(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, .5f);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider != null) {
                if (hits[i].collider.tag == "Fog")
                    hits[i].transform.SendMessage("Cascade");
            }
        }
    }

    void Cascade() {
        opacity = 0f;
        gettingHit = true;
    }
}
