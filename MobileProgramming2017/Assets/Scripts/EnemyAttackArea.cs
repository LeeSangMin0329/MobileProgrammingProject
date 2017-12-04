using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArea : MonoBehaviour {

    
    bool collisionTrigger = false;
    public int attackPower = 250;

    //Inner class
    public class AttackInfo
    {
        public Vector3 hitDirection;
        public int attackPower;
    };

    AttackInfo attackInfo;

    // Use this for initialization
    void Start () {
        attackInfo = new AttackInfo();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerStay(Collider other)
    {
        if (collisionTrigger)
        {
            if (other.transform.tag == "Player")
            {
                Vector3 direction = transform.root.position - other.transform.position;
                direction.y = 0;
                direction = direction.normalized;
                attackInfo.hitDirection = direction;
                attackInfo.attackPower = attackPower;

                other.transform.SendMessage("Damage", attackInfo);
            }
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
