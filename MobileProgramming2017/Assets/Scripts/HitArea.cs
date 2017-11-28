using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Damage(int attackPower)
    {
        transform.root.SendMessage("HitDamage", attackPower);
    }
}
