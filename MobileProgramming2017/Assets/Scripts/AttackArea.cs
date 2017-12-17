using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour {

    CharacterStatus status;
    Collider ownCollider;
    FollowCamera ownCamera;

    public AudioClip attackSeClip;
    AudioSource attackSeAudio;
    public AudioClip hitSeClip;
    AudioSource hitSeAudio;

    // Inner class
    public class AttackInfo
    {
        public int attackPower;
        public GameObject attacker;
        public Vector3 collisionPosition;
    }
    // ~Inner class

    AttackInfo attackInfo; 
    

    // Use this for initialization
    void Awake()
    {
        status = transform.root.GetComponent<CharacterStatus>();
        ownCollider = transform.GetComponent<Collider>();
        attackInfo = new AttackInfo();
        ownCollider.enabled = false;
        ownCamera = FindObjectOfType<FollowCamera>();

        // audio

        attackSeAudio = gameObject.AddComponent<AudioSource>();
        attackSeAudio.clip = attackSeClip;
        attackSeAudio.loop = false;
        attackSeAudio.volume = 0.5f;

        hitSeAudio = gameObject.AddComponent<AudioSource>();
        hitSeAudio.clip = hitSeClip;
        hitSeAudio.loop = false;
        hitSeAudio.volume = 0.3f;
    }
    
    void GetAttackInfo()
    {
        //attackInfo.attackPower = status.Power;
        attackInfo.attacker = transform.root.gameObject;
        attackInfo.collisionPosition = ownCollider.transform.position;
        //bug
        //attackInfo.collisionPosition.y = transform.root.position.y + 1;
      
    }

    // @override
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            GetAttackInfo();
            other.transform.root.SendMessage("Damage", attackInfo);
            status.lastAttackTarget = other.transform.root.gameObject;
            ownCamera.ShakeOn(1);
            hitSeAudio.Play();
        }
    }

    public void OnAttack(int attackPower)
    {
        attackInfo.attackPower = attackPower;
        ownCollider.enabled = true;
        attackSeAudio.Play();
    }

    public void OnAttackTermination()
    {
        ownCollider.enabled = false;
    }

}
