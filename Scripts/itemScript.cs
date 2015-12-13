using UnityEngine;
using System.Collections;

public class itemScript : MonoBehaviour {
    public string itemName;
    public int healthMod;
    public int attackMod;
    public int defenseMod;
    public int experienceMod;
    public bool weapon;
    public bool armor;

    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void UseItem() {
        player.GetComponent<statsScript>().health += healthMod;
        player.GetComponent<statsScript>().attack += attackMod;
        player.GetComponent<statsScript>().defense += defenseMod;
        player.GetComponent<statsScript>().experience += experienceMod;
        if (weapon)
            player.GetComponent<statsScript>().weapon = itemName;
        if (armor)
            player.GetComponent<statsScript>().armor = itemName;
        DestroyObject(gameObject);
    }
}
