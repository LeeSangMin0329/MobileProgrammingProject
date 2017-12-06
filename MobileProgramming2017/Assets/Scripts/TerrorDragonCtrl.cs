using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonCtrl : MonoBehaviour {

    CharacterMove characterMove;

    enum State { Walk, BreathFire, Run, Chasing, Bite, WingStrike, Shout,
        FlightUp, FlightTurning, FlightRush, FlightDown, Flighting, FlightFire, Died}
    State state = State.Walk;
    State nextState = State.Walk;

    TerrorDragonStatus status;
    TerrorDragonAnimation terrAnimation;
    // -public
    public Transform attackTarget;

    Vector3 basePosition;
    bool firstEngage = true;

    public float waitBaseTime = 2.0f; // motion to motion max delay
    float waitTime;
    public float walkRange = 5.0f;
    float stopDistanceTargetToOwn = 6.0f;
    public float patternClassificationDistance = 30.0f;

    //effect
    public GameObject hitEffect;

	// Use this for initialization
	void Start () {
        characterMove = GetComponent<CharacterMove>();

        status = GetComponent<TerrorDragonStatus>();
        terrAnimation = GetComponent<TerrorDragonAnimation>();

        basePosition = transform.position;
        waitTime = waitBaseTime;
        attackTarget = null;
	}
	
	// Update is called once per frame
	void Update () {
		
        switch(state)
        {
            case State.Walk:
                Walking();
                break;
            case State.Run:
                Running();
                break;
            case State.Chasing:
                Chasing();
                break;

            case State.BreathFire:
                Breathing();
                break;
            case State.Bite:
                Biting();
                break;
            case State.WingStrike:
                WingStriking();
                break;

            case State.Shout:
                Shouting();
                break;

            case State.FlightUp:
                FlightUP();
                break;
            case State.FlightDown:
                FlightDown();
                break;
            case State.FlightFire:
                FlightFire();
                break;
            case State.Flighting:
                break;
            case State.FlightRush:
                Running();
                break;
            case State.FlightTurning:
                break;
        }

        if(state != nextState)
        {
            state = nextState;
            switch(state)
            {
                case State.Walk:
                    WalkStart();
                    break;
                case State.Run:
                    RunStart();
                    break;
                case State.Chasing:
                    ChaseStart();
                    break;

                case State.BreathFire:
                    BreathStart();
                    break;
                case State.Bite:
                    BiteStart();
                    break;
                case State.WingStrike:
                    WingStrikeStart();
                    break;

                case State.Shout:
                    ShoutStart();
                    break;

                case State.FlightUp:
                    FlightUpStart();
                    break;
                case State.FlightDown:
                    FlightDownStart();
                    break;
                case State.FlightFire:
                    FlightFireStart();
                    break;
                case State.Flighting:
                    break;
                case State.FlightRush:
                    RunStart();
                    break;
                case State.FlightTurning:
                    break;
                case State.Died:
                    Died();
                    break;
            }
        }
	}

    void WalkStart()
    {
        StateStartCommon();
    }
    void Walking()
    {
        if (waitTime > 0.0f)
        {
            waitTime -= Time.deltaTime;

            if (waitTime <= 0.0f)
            {
                Vector2 randomValue = Random.insideUnitCircle * walkRange;
                Vector3 destinationPosition = basePosition + new Vector3(randomValue.x, 0.0f, randomValue.y);
                characterMove.SetDestination(destinationPosition);
            }
        }
        else
        {
            
            if (characterMove.Arrived())
            {
                basePosition = transform.position;
                waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
            }
            // discover user
            if (attackTarget)
            {
                if (firstEngage)
                {
                    firstEngage = false;
                    ChangeState(State.Shout);
                }
                else
                {
                    float patternDistance = Vector3.Distance(attackTarget.position, transform.position);
                    // random pattern
                    int randomValue = Random.Range(0, 4);
                    if (randomValue == 0 && !status.flighting)
                    {
                        ChangeState(State.FlightUp);
                    }
                    else if (randomValue == 0 && status.flighting)
                    {
                        ChangeState(State.FlightDown);
                    }
                    else if (patternDistance > patternClassificationDistance && status.flighting)
                    {
                        ChangeState(State.FlightRush);
                    }
                    else if (!status.flighting && randomValue == 1)
                    {
                        ChangeState(State.Shout);
                    }
                    else
                    {
                        ChangeState(State.Chasing);
                    }
                }
                
            }
            else
            {
                ChangeState(State.Walk);
            }
        }
    }

    void RunStart()
    {
        StateStartCommon();
        if (attackTarget == null)
        {
            Vector2 randomValue = Random.insideUnitCircle * walkRange * walkRange;
            Vector3 destinationPosition = transform.position + new Vector3(randomValue.x, 0.0f, randomValue.y);
            characterMove.SetTumbleDestination(destinationPosition);
        }
        else
        {
            Vector2 randomValue = Random.insideUnitCircle * walkRange;
            Vector3 destinationPosition = attackTarget.position + new Vector3(randomValue.x, 0.0f, randomValue.y);
            characterMove.SetTumbleDestination(destinationPosition);
        }
        status.running = true;
        if (status.flighting)
        {
            status.flightRush = true;
        }
    }
    void Running()
    {
        if (characterMove.Arrived())
        {
            ChangeState(State.Chasing);
        }
    }

    void ChaseStart()
    {
        StateStartCommon();
        characterMove.SetTumbleDestination(attackTarget.position);
        status.running = true;
    }
    void Chasing()
    {
        Vector3 targetPos = attackTarget.position;
        Vector3 Pos = transform.position;
        targetPos.y = 0;
        Pos.y = 0;
        if (Vector3.Distance(targetPos, Pos) <= stopDistanceTargetToOwn)
        {
            characterMove.StopMove();
            if (status.flighting)
            {
                // random flight attack state
                int randomValue = Random.Range(0, 2);
                if(randomValue == 0)
                {
                    ChangeState(State.Walk);
                }
                else
                {
                    ChangeState(State.FlightFire);
                }
            }
            else
            {
                // random ground attack state
                int randomValue = Random.Range(0, 3);
                if (randomValue == 0)
                {
                    ChangeState(State.Bite);
                }
                else if (randomValue == 1)
                {
                    ChangeState(State.BreathFire);
                }
                else
                {
                    ChangeState(State.WingStrike);
                }
            }
        }
        else if (characterMove.Arrived())
        {
            ChangeState(State.Walk);
            
        }
    }

    void BiteStart()
    {
        StateStartCommon();
        status.biting = true;
        Vector3 targetDirection = (attackTarget.position - transform.position).normalized;
        characterMove.SetDirection(targetDirection);

        characterMove.StopMove();
    }
    void Biting()
    {
        if (terrAnimation.IsBited())
        {
            ChangeState(State.Walk);
        }
    }

    void BreathStart()
    {
        StateStartCommon();
        status.breathing = true;
        Vector3 targetDirection = (attackTarget.position - transform.position).normalized;
        characterMove.SetDirection(targetDirection);

        characterMove.StopMove();
    }
    void Breathing()
    {
        if (terrAnimation.IsBreathed())
        {
            ChangeState(State.Walk);
        }
    }

    void ShoutStart()
    {
        StateStartCommon();
        status.shouting = true;
        characterMove.StopMove();
        
    }
    void Shouting()
    {
        if(terrAnimation.IsShoutEnd())
        {
            // random state
            ChangeState(State.Walk);
        }

        waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);

    }

    void WingStrikeStart()
    {
        StateStartCommon();
        status.wingStriking = true;
        Vector3 targetDirection = (attackTarget.position - transform.position).normalized;
        characterMove.SetDirection(targetDirection);

        characterMove.StopMove();
    }
    void WingStriking()
    {
        if (terrAnimation.IsWingStriked())
        {
            ChangeState(State.Walk);
        }
    }

    void FlightUpStart()
    {
        StateStartCommon();
        status.flighting = true;
       

    }
    void FlightUP()
    {
        if (status.flightDontMove)
        {
            characterMove.SetControllerOffsetY(5.0f);
        }
        //characterMove.UseGravity(false);
        if (terrAnimation.IsFlightUp())
        {
            // 1 chasing 2 up 3 rush
            ChangeState(State.Walk);
        }
    }

    void FlightDownStart()
    {
        StateStartCommon();
        status.flighting = false;
        characterMove.StopMove();
    }
    void FlightDown()
    {
        if (status.flightDontMove)
        {
            characterMove.SetControllerOffsetY(1f);
        }
        
        if (terrAnimation.IsFlightDown())
        {
        //    characterMove.UseGravity(true);
            ChangeState(State.Walk);
        }
    }

    void FlightFireStart()
    {
        StateStartCommon();
        status.flightFire = true;
    }
    void FlightFire()
    {
        if (terrAnimation.IsFlightFire())
        {
            ChangeState(State.Walk);
        }
    }

    void Died()
    {
        status.died = true;
    }

    void Damage(AttackArea.AttackInfo attackInfo)
    {
        if (hitEffect)
        {
            GameObject effect = Instantiate(hitEffect, attackInfo.collisionPosition, Quaternion.identity) as GameObject;

            //effect.transform.position = attackInfo.collisionPosition;
            Destroy(effect, 0.3f);
        }
        status.lastAttacker = attackInfo.attacker;
        status.HP -= attackInfo.attackPower;
        if (status.HP <= 0)
        {
            status.HP = 0;
            ChangeState(State.Died);
        }
    }

    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    void ChangeState(State nextState)
    {
        Debug.Log("Change State " + nextState.ToString());
        this.nextState = nextState;
    }
    void StateStartCommon()
    {
        status.running = false;
        status.shouting = false;
        status.biting = false;
        status.breathing = false;
        status.flightRush = false;
        status.flightFire = false;
    }
}
