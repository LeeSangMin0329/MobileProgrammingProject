using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour {

    Inventory inven;

    public int HP = 1000;
    public int MaxHP = 1000;
    public float Stamina = 100;
    public float MaxStamina = 100;

    public float tumblingStamina = 30;

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
    public bool knockDown = false;
    public bool hit = false;
    public bool skill111 = false;
    public bool skill123 = false;
    public bool skill121 = false;

    public bool uncontrollableMotion = true;
    public bool skillOn = false;

    void Start()
    {
        inven = FindObjectOfType<Inventory>();
    }
    
	// Update is called once per frame
	void Update () {
        if (!skillOn)
        {
            if (Stamina < 100)
            {
                Stamina += 10.0f * Time.deltaTime;
                if (Stamina > MaxStamina)
                {
                    Stamina = MaxStamina;
                }
            }
        }
        else
        {
            if (Stamina > 0)
            {
                Stamina -= 10.0f * Time.deltaTime;
                if (Stamina < 0)
                {
                    Stamina = 0;
                }
            }
        }
        if (inven)
        {
            if (inven.hpItemTrigger)
            {
                inven.hpItemTrigger = false;
                HP += 300;
                if (HP > MaxHP)
                {
                    HP = MaxHP;
                }
            }
        }
       
	}

    [RPC]
    public void SetName(string name)
    {
        characterName = name;
       
    }
}
