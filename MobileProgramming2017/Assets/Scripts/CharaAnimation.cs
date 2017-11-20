using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimation : MonoBehaviour {

    InputManager inputManager;

    Animator animator;
    CharacterStatus status;
    Vector3 prePosition;
    bool isDown = false;
    bool attacked01 = false;
    bool attacked05 = false;

    bool tumbled = false;

    bool uncontrollableMotion = true;


    // propertiy
    public bool IsAttacked()
    {
        return attacked01 || attacked05;
    }
   
    public bool IsTumbleEnd()
    {
        return tumbled;
    }

    // animation event handling

    // Basic Attack 1
    void StartAttack01()
    {
        uncontrollableMotion = true;
    }
    void StartAttackHit01()
    {
        Debug.Log("Start Attack Hit 1");
    }

    void EndAttackHit01()
    {
        Debug.Log("End Attack Hit 1");
        uncontrollableMotion = false;
    }

    void EndAttack01()
    {
        attacked01 = true;
        uncontrollableMotion = true;
    }

    // Basic Attack 2
    void StartAttack05()
    {
        uncontrollableMotion = true;
    }
    void StartAttackHit05()
    {
        Debug.Log("Start Attack Hit 5");
    }

    void EndAttackHit05()
    {
        Debug.Log("End Attack Hit 5");
        uncontrollableMotion = false;
    }

    void EndAttack05()
    {
        attacked05 = true;
        uncontrollableMotion = true;
    }

    // Tumbling
    void StartTumbling()
    {
       
    }
    void StartTumblingNoHit()
    {

    }
    void EndTumblingNoHit()
    {
        
    }
    void EndTumbling()
    {
        tumbled = true;
    }
    // ~animation event handling

    // Use this for initialization
    void Start () {

        inputManager = FindObjectOfType<InputManager>();

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
        else if (attacked05 && !status.basicAttack2)
        {
            attacked05 = false;
        }
    
        animator.SetBool("BasicAttack1", (!attacked01 && status.basicAttack1));
        animator.SetBool("BasicAttack2", (!attacked05 && status.basicAttack2));

     
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
