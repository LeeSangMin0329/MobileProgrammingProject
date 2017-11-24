using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonCtrl : MonoBehaviour {

    CharacterMove characterMove;

    enum State { Walk, BreathFire, Run, Bite, WingStrike, Shout,
        FlightUp, FlightTurning, FlightRush, FlightDown, Flighting, FlightFire}
    State state = State.Walk;
    State nextState = State.Run;

    TerrorDragonStatus status;
    TerrorDragonAnimation terrAnimation;
    Transform attackTarget;

    Vector3 basePosition;
    bool firstEngage = true;

    public float waitBaseTime = 2.0f; // motion to motion max delay
    float waitTime;
    public float walkRange = 5.0f;

    

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

            case State.BreathFire:
                break;
            case State.Bite:
                break;
            case State.WingStrike:
                break;

            case State.Shout:
                break;

            case State.FlightUp:
                break;
            case State.FlightDown:
                break;
            case State.FlightFire:
                break;
            case State.Flighting:
                break;
            case State.FlightRush:
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

                case State.BreathFire:
                    break;
                case State.Bite:
                    break;
                case State.WingStrike:
                    break;

                case State.Shout:
                    break;

                case State.FlightUp:
                    break;
                case State.FlightDown:
                    break;
                case State.FlightFire:
                    break;
                case State.Flighting:
                    break;
                case State.FlightRush:
                    break;
                case State.FlightTurning:
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
                    ChangeState(State.Shout);
                }
                else
                {
                    // random pattern
                }
                
            }
        }
    }

    void RunStart()
    {
        StateStartCommon();
        status.running = true;
    }
    void Running()
    {
        if(attackTarget == null)
        {
            if (characterMove.Arrived())
            {
                Vector2 randomValue = Random.insideUnitCircle * walkRange * walkRange;
                Vector3 destinationPosition = transform.position + new Vector3(randomValue.x, 0.0f, randomValue.y);
                characterMove.SetTumbleDestination(destinationPosition);
            }
        }
        else
        {
            // attack run
        }
    }

    void ChangeState(State nextState)
    {
        this.nextState = nextState;
    }
    void StateStartCommon()
    {

    }

}
