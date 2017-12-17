using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonAnimation : MonoBehaviour {

    Animator animator;
    TerrorDragonStatus status;
    Vector3 prePosition;

    // damage system activator
    public EnemyAttackArea rightWing;
    public EnemyAttackArea leftWing;
    public EnemyAttackArea frontRightLeg;
    public EnemyAttackArea frontLeftLeg;
    public EnemyAttackArea rearRightLeg;
    public EnemyAttackArea rearLeftLeg;
    public EnemyAttackArea chest;
    public EnemyAttackArea fire;
    public EnemyAttackArea tail1;
    public EnemyAttackArea tail2;
    public EnemyAttackArea head;
    public EnemyAttackArea neck;

    public EnemyAttackArea shoutArea;
    public EnemyAttackArea flightRushArea;
  


    // animation state variable
    bool died = false;
    bool shout = false;
    bool bite = false;
    bool breath = false;
    bool wingStrike = false;
    bool flightUp = false;
    bool flightDown = false;
    bool flightFire = false;


    public GameObject fireEffect;
    public GameObject shoutEffect;
    public GameObject rightWingFireEffect;
    public GameObject leftWingFireEffect;


    // property

    public bool IsShoutEnd()
    {
        return shout;
    }
    public bool IsBited()
    {
        return bite;
    }
    public bool IsBreathed()
    {
        return breath;
    }
    public bool IsWingStriked()
    {
        return wingStrike;
    }
    public bool IsFlightUp()
    {
        return flightUp;
    }
    public bool IsFlightDown()
    {
        return flightDown;
    }
    public bool IsFlightFire()
    {
        return flightFire;
    }

    // event
    //shout
    void StartShout()
    {
        shoutArea.SetAttackPower(0);
        shoutArea.OnAttack();
        if (shoutEffect)
        {
            shoutEffect.SetActive(true);
        }
    }
    void EndShoutHit()
    {
        shoutArea.OnAttackTermination();
    }
    void EndShout()
    {
        shout = true;
        if (shoutEffect)
        {
            shoutEffect.SetActive(false);
        }
    }

    //bite
    void StartBite()
    {
        neck.SetAttackPower(200);
        neck.OnAttack();
        head.SetAttackPower(200);
        head.OnAttack();
    }
    void EndBite()
    {
        neck.OnAttackTermination();
        head.OnAttackTermination();
        bite = true;
    }

    // breath
    void StartBreathFire()
    {
        
        if (fireEffect)
        {
            fireEffect.SetActive(true);
        }
        fire.SetAttackPower(500);
        fire.OnAttack();
    }
    void EndBreathFire()
    {
        fire.OnAttackTermination();
        if (fireEffect)
        {
            fireEffect.SetActive(false);
        }
    }
    void EndBreath()
    {
        breath = true;
    }

    // wing strike
    void StartWingStrike()
    {
        rightWing.SetAttackPower(300);
        rightWing.OnAttack();
        leftWing.SetAttackPower(300);
        leftWing.OnAttack();
    }
    void EndWingStrike()
    {
        rightWing.OnAttackTermination();
        leftWing.OnAttackTermination();
        wingStrike = true;
    }

    // flight up
    void StartFlightUp()
    {
        status.flightDontMove = true;
    }
    void EndFlightUp()
    {
        flightUp = true;
        status.flightDontMove = false;
    }
    // flight down
    void StartFlightDown()
    {
        status.flightDontMove = true;
    }
    void EndFlightDown()
    {
        flightDown = true;
        status.flightDontMove = false;
    }

    // flight fire
    void StartFlightFireHit()
    {
        if (fireEffect)
        {
            fireEffect.SetActive(true);
        }
        fire.SetAttackPower(500);
        fire.OnAttack();
    }
    void EndFlightFireHit()
    {
        fire.OnAttackTermination();
        if (fireEffect)
        {
            fireEffect.SetActive(false);
        }
    }
    void EndFlightFire()
    {
        flightFire = true;
    }

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        status = GetComponent<TerrorDragonStatus>();
        prePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 deltaPosition = transform.position - prePosition;
        animator.SetFloat("Speed", deltaPosition.magnitude / Time.deltaTime);

        animator.SetBool("Running", status.running);

        // shout
        if(shout && !status.shouting)
        {
            shout = false;
        }
        animator.SetBool("Shouting", !shout && status.shouting);
        //bite
        if(bite && !status.biting)
        {
            bite = false;
        }
        animator.SetBool("Biting", !bite && status.biting);

        // breath
        if(breath && !status.breathing)
        {
            breath = false;
        }
        animator.SetBool("Breathing", !breath && status.breathing);

        // wing strike
        if(wingStrike && !status.wingStriking)
        {
            wingStrike = false;
        }
        animator.SetBool("WingStriking", !wingStrike && status.wingStriking);

        // flight up
        if(flightUp && !status.flighting)
        {
            flightUp = false;
        }
        if(flightDown && !status.flighting)
        {
            flightDown = false;
        }
        animator.SetBool("FlightUp", status.flighting);

        // flight rush
        animator.SetBool("FlightRush", status.flightRush);
        if (status.flightRush)
        {
            flightRushArea.SetAttackPower(400);
            flightRushArea.OnAttack();
            rightWingFireEffect.SetActive(true);
            leftWingFireEffect.SetActive(true);
        }
        else
        {
            flightRushArea.OnAttackTermination();
            rightWingFireEffect.SetActive(false);
            leftWingFireEffect.SetActive(false);
        }

        // flight fire
        if(flightFire && !status.flightFire)
        {
            flightFire = false;
        }
        animator.SetBool("FlightFire", !flightFire && status.flightFire);

        if(!died && status.died)
        {
            died = true;
            animator.SetTrigger("Died");
        }

        prePosition = transform.position;
    }
}
