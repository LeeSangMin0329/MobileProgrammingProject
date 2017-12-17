using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArea : MonoBehaviour {

    
    bool collisionTrigger = false;
    public int attackPower = 250;

    public AudioClip attackSeClip;
    AudioSource attackSeAudio;

    public AudioClip hitSeClip;
    AudioSource hitSeAudio;


    //Inner class
    public class AttackInfo
    {
        public Vector3 hitDirection;
        public int attackPower;
    };

    AttackInfo attackInfo;

    // Use this for initialization
    void Awake () {
        attackInfo = new AttackInfo();

        // audio
        attackSeAudio = gameObject.AddComponent<AudioSource>();
        attackSeAudio.clip = attackSeClip;
        attackSeAudio.loop = false;
        attackSeAudio.volume = 0.5f;
        hitSeAudio = gameObject.AddComponent<AudioSource>();
        hitSeAudio.clip = hitSeClip;
        hitSeAudio.loop = false;
        hitSeAudio.volume = 0.5f;
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
                if (hitSeAudio)
                {
                    if(!hitSeAudio.isPlaying)
                        hitSeAudio.Play();
                }
                other.transform.SendMessage("Damage", attackInfo);
            }
        }
    }

    public void OnAttack()
    {
        collisionTrigger = true;
        if (attackSeAudio)
        {
            attackSeAudio.Play();
        }
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
