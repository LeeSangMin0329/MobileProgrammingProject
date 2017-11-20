using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour {

    public float waitBaseTime = 2.0f; // motion to motion
    float waitTime;
    public float walkRange = 5.0f;
    public Vector3 basePosition;

    public float stopDistanceTargetToOwn = 5.0f;

    CharacterStatus status;
    CharaAnimation charaAnimation;
    CharacterMove characterMove;

    GameRuleCtrl gameRuleCtrl;

    enum State{
        Walking,
        Chasing,
        Attacking,
        Died,
    }
    State state = State.Walking;
    State nextState = State.Walking;

    Transform attackTarget;


    // effect
    public GameObject hitEffect;

    // Use this for initialization
    void Start () {
        status = GetComponent<CharacterStatus>();
        charaAnimation = GetComponent<CharaAnimation>();
        characterMove = GetComponent<CharacterMove>();

        basePosition = transform.position;
        waitTime = waitBaseTime;

        gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();
	}
	
	// Update is called once per frame
	void Update () {

        switch(state)
        {
            case State.Walking:
                Walking();
                break;
            case State.Chasing:
                Chasing();
                break;
            case State.Attacking:
                Attacking();
                break;
        }
        
        if(state != nextState)
        {
            state = nextState;
            switch (state)
            {
                case State.Walking:
                    WalkStart();
                    break;
                case State.Chasing:
                    ChaseStart();
                    break;
                case State.Attacking:
                    AttackStart();
                    break;
                case State.Died:
                    Died();
                    break;
            }
        }
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

            if (attackTarget)
            {
                ChangeState(State.Chasing);
            }
        }
    }

    void WalkStart()
    {
        StateStartCommon();
    }

    void ChaseStart()
    {
        StateStartCommon();
    }

    void Chasing()
    {
        SendMessage("SetDestination", attackTarget.position);

        if(Vector3.Distance(attackTarget.position, transform.position) <= stopDistanceTargetToOwn)
        {
            ChangeState(State.Attacking);
        }
    }

    void Attacking()
    {
        if (charaAnimation.IsAttacked())
        {
            ChangeState(State.Walking);
        }

        waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);

        attackTarget = null;
    }

    void AttackStart()
    {
        StateStartCommon();
        status.basicAttack1 = true;

        Vector3 targetDirection = (attackTarget.position - transform.position).normalized;
        SendMessage("SetDirection", targetDirection);

        SendMessage("StopMove");
    }

    void Damage(AttackArea.AttackInfo attackInfo)
    {
        GameObject effect = Instantiate(hitEffect, attackInfo.collisionPosition, Quaternion.identity) as GameObject;

        //effect.transform.position = attackInfo.collisionPosition;
        Destroy(effect, 0.3f);

        status.HP -= attackInfo.attackPower;
        if(status.HP <= 0)
        {
            status.HP = 0;
            ChangeState(State.Died);
        }

    }

    void Died()
    {
        status.died = true;

        if(gameObject.tag == "Boss")
        {
            gameRuleCtrl.GameClear();
        }
    }

    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    void StateStartCommon()
    {
        status.basicAttack1 = false;
        status.died = false;
    }

    void ChangeState(State nextState)
    {
        this.nextState = nextState;
    }
}
