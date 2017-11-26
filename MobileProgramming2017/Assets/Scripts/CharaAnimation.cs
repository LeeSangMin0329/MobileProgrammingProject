using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimation : MonoBehaviour {

   
    Animator animator;
    CharacterStatus status;
    Vector3 prePosition;
    bool isDown = false;
    bool attacked01 = false;
    bool attacked05 = false;
    bool attacked09 = false;

    bool tumbled = false;

    

    // propertiy
    public bool IsAttacked()
    {
        return attacked01 || attacked05 || attacked09;
    }
   
    public bool IsTumbleEnd()
    {
        return tumbled;
    }

    // animation event handling
    //idle
    void StartIdle()
    {
        if (animator.GetBool("Tumbling"))
        {
            animator.Play("tumbling");
        }
    }

    // Basic Attack 1
    void StartAttack01()
    {
        status.uncontrollableMotion = true;
    }
    void StartAttackHit01()
    {
        Debug.Log("Start Attack Hit 1");
    }

    void EndAttackHit01()
    {
        Debug.Log("End Attack Hit 1");
        status.uncontrollableMotion = false;
    }
    void EndCancel01()
    {
        status.uncontrollableMotion = true;
    }
    void EndAttack01()
    {
        attacked01 = true;
    }

    // Basic Attack 2
    void StartAttack05()
    {
        status.uncontrollableMotion = true;
    }
    void StartAttackHit05()
    {
        Debug.Log("Start Attack Hit 5");
    }

    void EndAttackHit05()
    {
        Debug.Log("End Attack Hit 5");
        status.uncontrollableMotion = false;
    }
    void EndCancel05()
    {
        status.uncontrollableMotion = true;
    }
    void EndAttack05()
    {
        attacked05 = true;
    }

    // basic attack 3
    void StartAttack09()
    {
        status.uncontrollableMotion = true;
    }
    void StartAttackHit09()
    {

    }
    void EndAttackHit09()
    {
        status.uncontrollableMotion = false;
    }
    void EndCancel09()
    {
        status.uncontrollableMotion = true;
    }
    void EndAttack09()
    {
        attacked09 = true;
    }

    // Tumbling
    void StartTumbling()
    {
        status.uncontrollableMotion = true;
    }
    void StartTumblingNoHit()
    {

    }
    void EndTumblingNoHit()
    {
        
    }
    void EndTumbling()
    {
        status.uncontrollableMotion = false;
        tumbled = true;
    }
    // ~animation event handling

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();
        
        status = GetComponent<CharacterStatus>();

        prePosition = transform.position;
	}

    // Update is called once per frame
    void Update() {
        // move
        Vector3 deltaPosition = transform.position - prePosition;
        animator.SetFloat("Speed", deltaPosition.magnitude / Time.deltaTime);


        
        // Basic attack
        if (attacked01 && !status.basicAttack1)
        {
            attacked01 = false;
        }
        if (attacked05 && !status.basicAttack2)
        {
            attacked05 = false;
        }
        if(attacked09 && !status.basicAttack3)
        {
            attacked09 = false;
        }
    
        animator.SetBool("BasicAttack1", (!attacked01 && status.basicAttack1));
        animator.SetBool("BasicAttack2", (!attacked05 && status.basicAttack2));
        animator.SetBool("BasicAttack3", (!attacked09 && status.basicAttack3));

     
        // tumbling
        if (tumbled && !status.tumbling)
        {
            tumbled = false;
        }
        animator.SetBool("Tumbling", (!tumbled && status.tumbling));


        // Died
        if (!isDown && status.died)
        {
            isDown = true;
            animator.SetTrigger("Down");
        }

        prePosition = transform.position;
	}
}
