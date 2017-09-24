using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour {

    CharacterStatus status;
    Collider ownCollider;

    // Inner class
    public class AttackInfo
    {
        public int attackPower;
        public Transform attacker;
        public Vector3 collisionPosition;
    }
    // ~Inner class

    // Use this for initialization
    void Start()
    {
        status = transform.root.GetComponent<CharacterStatus>();
        ownCollider = transform.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    AttackInfo GetAttackInfo()
    {
        AttackInfo attackInfo = new AttackInfo();

        attackInfo.attackPower = status.Power;
        attackInfo.attacker = transform.root;
        attackInfo.collisionPosition = ownCollider.transform.position;
        //bug
        attackInfo.collisionPosition.y = transform.root.position.y + 1;
        

        return attackInfo;
    }

    // @override
    void OnTriggerEnter(Collider other)
    {
        other.SendMessage("Damage", GetAttackInfo());
        status.lastAttackTarget = other.transform.root.gameObject;
    }

    void OnAttack()
    {
        ownCollider.enabled = true;
    }

    void OnAttackTermination()
    {
        ownCollider.enabled = false;
    }

    
}
