using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaActivator : MonoBehaviour {

    Collider[] attackAreaColliders;

	// Use this for initialization
	void Start () {
        AttackArea[] attackAreas = GetComponentsInChildren<AttackArea>();
        attackAreaColliders = new Collider[attackAreas.Length];

        for(int attackAreaCnt = 0; attackAreaCnt < attackAreas.Length; attackAreaCnt++)
        {
            attackAreaColliders[attackAreaCnt] = attackAreas[attackAreaCnt].GetComponent<Collider>();
            attackAreaColliders[attackAreaCnt].enabled = false;
        }
	}
	
    // Animation Event
    void StartAttackHit()
    {
        foreach(Collider attackAreaCollider in attackAreaColliders)
        {
            attackAreaCollider.enabled = true;
        }
    }

    // Animation Event
    void EndAttackHit()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
        {
            attackAreaCollider.enabled = false;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
