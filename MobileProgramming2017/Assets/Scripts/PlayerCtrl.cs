using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    InputManager inputManager;
    CharacterMove characterMove;

    public Vector3 movementHorizon;
    public Vector3 movementVertical;
    public Vector3 tumbleDestination;
    public float tumbleDistance = 150f;

    public Camera charactorCamera;

    public float speed = 200f;

    bool immortal = false;
    bool shoutDamageTrigger = false;
    
    enum State { Walk, Tumble, Attack1, Attack2, Attack3, Died, Hit,};
    State state = State.Walk;
    State nextState = State.Walk;

    //const float RayCastMaxDistance = 100.0f;
    CharacterStatus status;
    CharaAnimation charaAnimation;
    Transform attackTarget;
    

    // Use this for initialization
    void Start () {
        inputManager = FindObjectOfType<InputManager>();
        characterMove = GetComponent<CharacterMove>();
        tumbleDestination = Vector3.zero;

        status = GetComponent<CharacterStatus>();
        charaAnimation = GetComponent<CharaAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
       
        switch (state)
        {
            case State.Walk:
                // branch trigger # critical ordering
                if (inputManager.basicAttackTrigger1)
                {
                    ChangeState(State.Attack1);
                }
                else if (inputManager.basicAttackTrigger2)
                {
                    ChangeState(State.Attack2);
                }
                else if (inputManager.basicAttackTrigger3)
                {
                    ChangeState(State.Attack3);
                }
                if (inputManager.tumbleTrigger)
                {
                    ChangeState(State.Tumble);
                }
                // ~trigger ctrl
                
                Walking();
                break;
            case State.Tumble:
                Tumbling();
                break;
            case State.Attack1:
                Attacking();
                break;
            case State.Attack2:
                Attacking();
                break;
            case State.Attack3:
                Attacking();
                break;
            case State.Hit:
                Hitting();
                break;
        }

        if(state != nextState)
        {
            state = nextState;
            switch (state)
            {
                case State.Walk:
                    WalkStart();
                    break;
                case State.Tumble:
                    TumbleStart();
                    break;
                case State.Attack1:
                    AttackStart(1);
                    break;
                case State.Attack2:
                    AttackStart(2);
                    break;
                case State.Attack3:
                    AttackStart(3);
                    break;
                case State.Died:
                    Died();
                    break;
                case State.Hit:
                    HitStart();
                    break;
            }
        }
	}

    void ChangeState(State nextState)
    {
        this.nextState = nextState;
    }

    // walk
    void WalkStart()
    {
        StateStartCommon();
    }

    void Walking()
    {
        movementHorizon.x = inputManager.horizontalMove * charactorCamera.transform.right.x;
        movementHorizon.y = 0;
        movementHorizon.z = inputManager.horizontalMove * charactorCamera.transform.right.z;

        movementVertical.x = inputManager.verticalMove * charactorCamera.transform.forward.x;
        movementVertical.y = 0;
        movementVertical.z = inputManager.verticalMove * charactorCamera.transform.forward.z;

        characterMove.SetDestination(transform.position + (movementHorizon + movementVertical) * speed * Time.deltaTime);
    }

    // basic attack
    void AttackStart(int type)
    {
        StateStartCommon();
        if(type == 1)
        {
            status.basicAttack1 = true;
            status.basicAttack2 = false;
            status.basicAttack3 = false;
        }
        else if(type == 2)
        {
            status.basicAttack1 = false;
            status.basicAttack2 = true;
            status.basicAttack3 = false;
        }
        else if(type == 3)
        {
            status.basicAttack1 = false;
            status.basicAttack2 = false;
            status.basicAttack3 = true;
        }
        
        characterMove.StopMove();
    }

    void Attacking()
    {
        if (charaAnimation.IsAttacked())
        {
            ChangeState(State.Walk);
        }
        else if (status.uncontrollableMotion == false && inputManager.tumbleTrigger)
        {
            ChangeState(State.Tumble);
        }
    }

    // died
    void Died()
    {
        characterMove.enabled = false;
        status.died = true;
    }

    // tumbling
    public void TumbleStart()
    {
        StateStartCommon();

        movementHorizon.x = inputManager.horizontalMove * charactorCamera.transform.right.x;
        movementHorizon.y = 0;
        movementHorizon.z = inputManager.horizontalMove * charactorCamera.transform.right.z;

        movementVertical.x = inputManager.verticalMove * charactorCamera.transform.forward.x;
        movementVertical.y = 0;
        movementVertical.z = inputManager.verticalMove * charactorCamera.transform.forward.z;

        if((movementHorizon + movementVertical) == Vector3.zero)
        {
            tumbleDestination = transform.position + transform.forward * tumbleDistance * Time.deltaTime;
        }
        else
        {
            //transform.LookAt(transform.position + (movementHorizon + movementVertical));
            characterMove.SetDirection((movementHorizon + movementVertical));
            tumbleDestination = transform.position + (movementHorizon + movementVertical) * tumbleDistance * Time.deltaTime;
        }
    }

    void Tumbling()
    {
        if (charaAnimation.IsTumbleEnd())
        {
            ChangeState(State.Walk);
        }
        else
        {
            status.tumbling = true;
            
            characterMove.SetTumbleDestination(tumbleDestination);
        }
    }

    // Hit
    void HitStart()
    {
        StateStartCommon();
        characterMove.enabled = false;
    }
    void Hitting()
    {
        
        if (charaAnimation.IsHitted())
        {
            characterMove.enabled = true;
            status.knockDown = false;
            status.hit = false;
            immortal = false;
            ChangeState(State.Walk);
            
        }
    }

    // damage 
    void HitDamage(EnemyAttackArea.AttackInfo attackInfo)
    {
        if (!immortal)
        {
            // shout
            if(attackInfo.attackPower != 0)
            {
                shoutDamageTrigger = false;
            }
            if (shoutDamageTrigger)
            {
                return;
            }
            if(attackInfo.attackPower == 0)
            {
                shoutDamageTrigger = true;
            }
            status.HP -= attackInfo.attackPower;
            if (status.HP <= 0)
            {
                status.HP = 0;
                ChangeState(State.Died);
            }
            else
            {
                if (attackInfo.attackPower > status.MaxHP * 0.2f)
                {
                    status.knockDown = true;
                }
                else
                {
                    status.hit = true;
                }
                immortal = true;
                
                transform.LookAt( attackInfo.hitDirection);
                ChangeState(State.Hit);
            }
        }
    }

    void StateStartCommon()
    {
        status.basicAttack1 = false;
        status.basicAttack2 = false;
        status.basicAttack3 = false;
        status.died = false;
        status.tumbling = false;
       
    }
}
