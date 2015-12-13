using UnityEngine;
using System.Collections;

public class statsScript : MonoBehaviour {
    public string Name;
    public int level;
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public int experience;
    public int maxExperience;
    public string weapon;
    public string armor;
    public string intent;
    public int remainingEnemies;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (experience >= maxExperience)
            levelUp();
	
	}

    void levelUp() {
        level++;
        maxHealth += 5 * level;
        attack += 2 * level;
        defense += 1 * level;
        maxExperience += 10 * level;
    }
}
