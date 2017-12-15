using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimation : MonoBehaviour {
    
    Animator animator;
    CharacterStatus status;
    AttackArea attackArea;
    Vector3 prePosition;
    bool isDown = false;
    bool attacked01 = false;
    bool attacked05 = false;
    bool attacked09 = false;
    bool knockDown = false;
    bool knockDownTrigger = false;
    bool hit = false;
    bool hitTrigger = false;

    bool tumbled = false;

    // skill
    bool skill111 = false;
    bool skill123 = false;
    bool skill121 = false;

    public GameObject swordEffect;

    // propertiy
    public bool IsAttacked()
    {
        return attacked01 || attacked05 || attacked09;
    }
   
    public bool IsTumbleEnd()
    {
        return tumbled;
    }
    public bool IsHitted()
    {
        return (hit || knockDown);
    }
    public bool IsSkillEnd()
    {
        return skill111 || skill123 || skill121;
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
        attackArea.OnAttack(status.attack1Power);
        if (status.skillOn && swordEffect)
        {
            swordEffect.SetActive(true);
        }
    }

    void EndAttackHit01()
    {
        Debug.Log("End Attack Hit 1");
        status.uncontrollableMotion = false;
        attackArea.OnAttackTermination();
    }
    void EndCancel01()
    {
        //status.uncontrollableMotion = true;
        if (status.skillOn && swordEffect)
        {
            swordEffect.SetActive(false);
        }
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
        attackArea.OnAttack(status.attack2Power);
        if (status.skillOn && swordEffect)
        {
            swordEffect.SetActive(true);
        }
    }

    void EndAttackHit05()
    {
        Debug.Log("End Attack Hit 5");
        status.uncontrollableMotion = false;
        attackArea.OnAttackTermination();
    }
    void EndCancel05()
    {
        //status.uncontrollableMotion = true;
        if (status.skillOn && swordEffect)
        {
            swordEffect.SetActive(false);
        }
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
        attackArea.OnAttack(status.attack3Power);
        if (status.skillOn && swordEffect)
        {
            swordEffect.SetActive(true);
        }
    }
    void EndAttackHit09()
    {
        status.uncontrollableMotion = false;
        attackArea.OnAttackTermination();
    }
    void EndCancel09()
    {
        //status.uncontrollableMotion = true;
        if (status.skillOn && swordEffect)
        {
            swordEffect.SetActive(false);
        }
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

    // hit animation
    void StartKnockDown()
    {
        knockDownTrigger = true;
    }
    void EndKnockDown()
    {

    }
    void EndGetUp()
    {
        knockDown = true;
    }

    void StartHit02()
    {
        hitTrigger = true;
    }
    void EndHit02()
    {
        hit = true;
    }

    // skill id 111
    void StartSkill111()
    {
        if (swordEffect)
        {
            swordEffect.SetActive(true);
        }
    }
    void EndSkill111()
    {
        skill111 = true;
        if (swordEffect)
        {
            swordEffect.SetActive(false);
        }
    }
    // skill id 123
    void StartSkill123()
    {
        if (swordEffect)
        {
            swordEffect.SetActive(true);
        }
    }
    void EndSkill123()
    {
        skill123 = true;
        if (swordEffect)
        {
            swordEffect.SetActive(false);
        }
    }
    // skill id 121
    void StartSkill121()
    {
        if (swordEffect)
        {
            swordEffect.SetActive(true);
        }
    }
    void EndSkill121()
    {
        skill121 = true;
        if (swordEffect)
        {
            swordEffect.SetActive(false);
        }
    }
    // ~animation event handling

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();
        
        status = GetComponent<CharacterStatus>();

        prePosition = transform.position;
        attackArea = GetComponentInChildren<AttackArea>();
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

        // hit animaiton
        if(knockDown && !status.knockDown)
        {
            knockDown = false;
        }
        if(knockDownTrigger && !status.knockDown)
        {
            knockDownTrigger = false;
        }
        animator.SetBool("KnockDown", (!knockDownTrigger && status.knockDown));

        if(hit && !status.hit)
        {
            hit = false;
        }
        if(hitTrigger && !status.hit)
        {
            hitTrigger = false;
        }
        animator.SetBool("Hit", (!hitTrigger && status.hit));

        // skill
        if(skill111 && !status.skill111)
        {
            skill111 = false;
        }
        animator.SetBool("Skill111", !skill111 && status.skill111);

        if(skill123 && !status.skill123)
        {
            skill123 = false;
        }
        animator.SetBool("Skill123", !skill123 && status.skill123);

        if (skill121 && !status.skill121)
        {
            skill121 = false;
        }
        animator.SetBool("Skill121", !skill121 && status.skill121);

        // Died
        if (!isDown && status.died)
        {
            isDown = true;
            animator.SetTrigger("Down");
        }

        prePosition = transform.position;
	}
}
