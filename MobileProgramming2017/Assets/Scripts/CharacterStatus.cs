using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour {

    public int HP = 100;
    public int MaxHP = 100;

    public int Power = 10;
    public GameObject lastAttackTarget = null;

    public string characterName = "Player";


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
