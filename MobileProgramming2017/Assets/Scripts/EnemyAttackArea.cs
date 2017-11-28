using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArea : MonoBehaviour {

    
    bool collisionTrigger = false;
    public int attackPower = 250;

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerStay(Collider other)
    {
        if (collisionTrigger)
        {
            Debug.Log("Hit"+this.gameObject.ToString());
            other.transform.SendMessage("Damage", attackPower);
            
        }
    }

    public void OnAttack()
    {
        collisionTrigger = true;
    }
    public void OnAttackTermination()
    {
        collisionTrigger = false;
    }

    public void SetAttackPower(int power)
    {
        attackPower = power;
    }
}
