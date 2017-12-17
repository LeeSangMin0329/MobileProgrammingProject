using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    InputManager inputManager;
    CharacterMove characterMove;

    NetworkView netView;
    
    public Vector3 movementHorizon;
    public Vector3 movementVertical;
    public Vector3 tumbleDestination;
    public float tumbleDistance = 150f;

    public Camera charactorCamera;
    FollowCamera charaCameraScript;

    public float speed = 200f;

    bool immortal = false;
    bool shoutDamageTrigger = false;
    
    enum State { Walk, Tumble, Attack1, Attack2, Attack3, Died, Hit, Skill,};
    State state = State.Walk;
    State nextState = State.Walk;

    // skill system
    int skillID;
    int skillCount = 0;
    bool skillEnable = false;
    int[] skill;
    
    //const float RayCastMaxDistance = 100.0f;
    CharacterStatus status;
    CharaAnimation charaAnimation;
    Transform attackTarget;


    //audio
    public AudioClip deathSeClip;
    AudioSource deathSeAudio;
    
    void Awake()
    {
        netView = GetComponent<NetworkView>();
    }

    // Use this for initialization
    void Start () {
        inputManager = FindObjectOfType<InputManager>();
        characterMove = GetComponent<CharacterMove>();
        
        
        tumbleDestination = Vector3.zero;

        status = GetComponent<CharacterStatus>();
        charaAnimation = GetComponent<CharaAnimation>();
        skill = new int[3];

        //audio
        deathSeAudio = gameObject.AddComponent<AudioSource>();

        deathSeAudio.loop = false;
        deathSeAudio.clip = deathSeClip;
	}
	
    
	// Update is called once per frame
	void Update () {
        if (netView.enabled && !netView.isMine)
        {
            return;
        }
       
        switch (state)
        {
            case State.Walk:
                // branch trigger # critical ordering
                if(skillCount == 3)
                {
                    skillEnable = false;
                    status.skillOn = false;
                    ChangeState(State.Skill);
                }

                if (inputManager.basicAttackTrigger1)
                {
                    if (skillEnable)
                    {
                        skill[skillCount++] = 1;
                    }
                    ChangeState(State.Attack1);
                }
                else if (inputManager.basicAttackTrigger2)
                {
                    if (skillEnable)
                    {
                        skill[skillCount++] = 2;
                    }
                    ChangeState(State.Attack2);
                }
                else if (inputManager.basicAttackTrigger3)
                {
                    if (skillEnable)
                    {
                        skill[skillCount++] = 3;
                    }
                    ChangeState(State.Attack3);
                }
                if (inputManager.tumbleTrigger)
                {
                    if (status.Stamina - status.tumblingStamina > 0)
                    {
                        status.Stamina -= status.tumblingStamina;
                        ChangeState(State.Tumble);
                    }
                }

                if (status.Stamina == 0 || (skillEnable && inputManager.skillTrigger))
                {
                    skillEnable = false;
                    status.skillOn = false;
                    ChangeState(State.Walk);
                }
                else if(inputManager.skillTrigger)
                {
                    skillEnable = true;
                    status.skillOn = true;
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
            case State.Skill:
                SkillFire();
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
                case State.Skill:
                    SkillStart();
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
        else if (!skillEnable && status.uncontrollableMotion == false && inputManager.tumbleTrigger)
        {
            if(status.Stamina - status.tumblingStamina > 0)
            {
                status.Stamina -= status.tumblingStamina;
                ChangeState(State.Tumble);
            }
                
        }
    }

    // died
    void Died()
    {
        deathSeAudio.Play();
        characterMove.enabled = false;
        status.died = true;
        Invoke("DelayedDestroy", 8.0f);
    }

    void DelayedDestroy()
    {
        Network.Destroy(gameObject);
        Network.RemoveRPCs(netView.viewID);
        netView = null;
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
            if (status.skillOn)
            {
                skillEnable = false;
                status.skillOn = false;
                skillID = 0;
            }
            ChangeState(State.Walk);
            
        }
    }

    // Skill
    void SkillStart()
    {
        StateStartCommon();
        characterMove.StopMove();
        skillID = (int)skill[0];
        skillID *= 10;
        skillID += (int)skill[1];
        skillID *= 10;
        skillID += (int)skill[2];
        
        if (skillCount != 3)
        {
            // skill fail
        }
        else
        {

            switch (skillID)
            {
                case 111:
                    status.skill111 = true;
                    break;
                case 123:
                    status.skill123 = true;
                    break;
                case 121:
                    status.skill121 = true;
                    break;
                default:
                    ChangeState(State.Walk);
                    break;
            }
        }
        skillCount = 0;
    }
    void SkillFire()
    {
        if (charaAnimation.IsSkillEnd())
        {
            skillID = 0;
            ChangeState(State.Walk);
        }
    }

    // damage 
    void HitDamage(EnemyAttackArea.AttackInfo attackInfo)
    {
        if (netView.isMine)
        {
            DamageMine(attackInfo.attackPower, attackInfo.hitDirection);
        }
        else
        {
            netView.RPC("DamageMine", netView.owner, attackInfo.attackPower, attackInfo.hitDirection);
        }
    }

    [RPC]
    void DamageMine(int attackPower, Vector3 hitDirection)
    {
        if (!immortal)
        {
            // shout
            if (attackPower != 0)
            {
                shoutDamageTrigger = false;
            }
            if (shoutDamageTrigger)
            {
                return;
            }
            if (attackPower == 0)
            {
                shoutDamageTrigger = true;
            }
            status.HP -= attackPower;
            if (status.HP <= 0)
            {
                status.HP = 0;
                ChangeState(State.Died);
            }
            else
            {
                if (attackPower > status.MaxHP * 0.2f)
                {
                    status.knockDown = true;
                }
                else
                {
                    status.hit = true;
                }
                immortal = true;

                transform.LookAt(hitDirection * -1f);
                ChangeState(State.Hit);
            }

            charaCameraScript.ShakeOn(1);
        }
    }

    void StateStartCommon()
    {
        status.basicAttack1 = false;
        status.basicAttack2 = false;
        status.basicAttack3 = false;
        status.died = false;
        status.tumbling = false;
        status.skill111 = false;
        status.skill123 = false;
        status.skill121 = false;
    }

    // external part, not game logic
    public void SetCamera(Camera mainCamera)
    {
        charactorCamera = mainCamera;
        charaCameraScript = charactorCamera.GetComponent<FollowCamera>();
    }

    // @override
    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        if (!netView.isMine)
        {
            Destroy(characterMove);

            AttackArea[] attackAreas = GetComponentsInChildren<AttackArea>();
            foreach(AttackArea attackArea in attackAreas)
            {
                Destroy(attackArea);
            }
            
        }
    }
}
