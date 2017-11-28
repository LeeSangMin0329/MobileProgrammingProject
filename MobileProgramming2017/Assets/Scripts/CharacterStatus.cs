using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour {

    public int HP = 1000;
    public int MaxHP = 1000;

    public int Power = 100;
    public GameObject lastAttackTarget = null;

    public string characterName = "Player";

    public int attack1Power = 30;
    public int attack2Power = 50;
    public int attack3Power = 50;

    // animation variable
    public bool basicAttack1 = false;
    public bool basicAttack2 = false;
    public bool basicAttack3 = false;
    public bool tumbling = false;
    public bool died = false;

    public bool uncontrollableMotion = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
