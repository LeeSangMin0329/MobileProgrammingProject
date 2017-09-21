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



    enum State { Walk, Tumble, Attack, Died,};
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
                if (inputManager.attackTrigger)
                {
                    ChangeState(State.Attack);
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
            case State.Attack:
                Attacking();
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
                case State.Attack:
                    AttackStart();
                    break;
                case State.Died:
                    Died();
                    break;
            }
        }
        /*
        switch (state)
        {
            case State.Walk:
                if (inputManager.tumbleTrigger)
                {
                    StartTumble();
                }
                Walking();
                break;
            case State.Tumble:
                if(Vector3.Distance(transform.position, tumbleDestination) < 0.1f)
                {
                    state = State.Walk;
                }
                else
                {
                    characterMove.SetTumbleDestination(tumbleDestination);
                }
                break;
        }
        */

        
	}

    void ChangeState(State nextState)
    {
        this.nextState = nextState;
    }

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

    void AttackStart()
    {
        StateStartCommon();
        status.attacking = true;

        characterMove.StopMove();
    }

    void Attacking()
    {
        if (charaAnimation.IsAttacked())
        {
            ChangeState(State.Walk);
        }
    }

    void Died()
    {
        status.died = true;
    }

    public void TumbleStart()
    {
        StateStartCommon();

        tumbleDestination = transform.position + transform.forward * tumbleDistance * Time.deltaTime;
    }

    void Tumbling()
    {
        if (Vector3.Distance(transform.position, tumbleDestination) < 0.1f)
        {
            ChangeState(State.Walk);
        }
        else
        {
            characterMove.SetTumbleDestination(tumbleDestination);
        }
    }

    void Damage(AttackArea.AttackInfo attackInfo)
    {
        status.HP -= attackInfo.attackPower;
        if (status.HP <= 0)
        {
            status.HP = 0;
            ChangeState(State.Died);
        }
    }

    void StateStartCommon()
    {
        status.attacking = false;
        status.died = false;
    }
}
