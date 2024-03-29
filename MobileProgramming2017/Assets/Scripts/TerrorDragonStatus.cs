﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonStatus : MonoBehaviour {
    
    // flag

    public bool running = false;
    public bool shouting = false;
    public bool biting = false;
    public bool breathing = false;
    public bool wingStriking = false;
    public bool flighting = false;
    public bool flightRush = false;
    public bool flightDontMove = false;
    public bool flightFire = false;

    public bool died = false;

    public GameObject lastAttacker = null;

    public float constrollerOffset = 1f;

    // status
    public int HP = 100000;
    public int MaxHP = 100000;

    public string enemyName = "TerrorDragon";

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
