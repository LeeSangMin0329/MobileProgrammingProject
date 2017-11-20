using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimation : MonoBehaviour {

    Animator animator;
    CharacterStatus status;
    Vector3 prePosition;
    bool isDown = false;
    bool attacked = false;
    bool rolled = false;

    // propertiy
    public bool IsAttacked()
    {
        return attacked;
    }
    public bool IsRolled()
    {
        return rolled;
    }

    // animation event handling
    void StartAttackHit()
    {
        Debug.Log("Start Attack Hit");
    }

    void EndAttackHit()
    {
        Debug.Log("End Attack Hit");
    }

    void EndAttack()
    {
        attacked = true;
    }

    void RollStart()
    {
        rolled = true;
    }

    void RollEnd()
    {
        rolled = false;
    }
    //~ end
    
	// Use this for initialization
	void Start () {

        animator = GetComponent<Animator>();
        status = GetComponent<CharacterStatus>();

        prePosition = transform.position;
	}

    // Update is called once per frame
    void Update() {
        Vector3 deltaPosition = transform.position - prePosition;
        animator.SetFloat("Speed", deltaPosition.magnitude / Time.deltaTime);

        if (attacked && !status.attacking)
        {
            attacked = false;
        }

        animator.SetBool("Attacking", (!attacked && status.attacking));

        if(!isDown && status.died)
        {
            isDown = true;
            animator.SetTrigger("Down");
        }

        animator.SetBool("Rolling", rolled);


        prePosition = transform.position;
	}
}
