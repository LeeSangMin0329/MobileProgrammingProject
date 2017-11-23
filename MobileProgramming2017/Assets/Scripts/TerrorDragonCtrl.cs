using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonCtrl : MonoBehaviour {

    CharacterMove characterMove;

    enum State { Walk, BreathFire, Idle, Bite, WingStrike, Shout,
        FlightUp, FlightTurning, FlightRush, FlightDown, Flighting, FlightFire}
    State state = State.Walk;
    State nextState = State.Walk;

    CharacterStatus status;
    TerrorDragonAnimation terrAnimation;
    Transform attackTarget;

    Vector3 basePosition;

    public float waitBaseTime = 2.0f; // motion to motion max delay
    float waitTime;
    public float walkRange = 5.0f;

    

	// Use this for initialization
	void Start () {
        characterMove = GetComponent<CharacterMove>();

        status = GetComponent<CharacterStatus>();
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
            case State.Idle:
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
                case State.Idle:
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
                SendMessage("SetDestination", destinationPosition);
            }
        }
        else
        {
            if (characterMove.Arrived())
            {
                waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
            }
            // discover user
            if (attackTarget)
            {
                ChangeState(State.Shout);
            }
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
