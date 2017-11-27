using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonAnimation : MonoBehaviour {

    Animator animator;
    TerrorDragonStatus status;
    Vector3 prePosition;

    bool shout = false;
    bool bite = false;
    bool breath = false;
    bool wingStrike = false;
    bool flightUp = false;
    bool flightDown = false;


    public GameObject fireEffect;

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

    // event
    //shout
    void StartShout()
    {
       
    }
    void EndShoutHit()
    {

    }
    void EndShout()
    {
        shout = true;
    }

    //bite
    void EndBite()
    {
        bite = true;
    }

    // breath
    void StartBreathFire()
    {
        if (fireEffect)
        {
            fireEffect.SetActive(true);
        }
    }
    void EndBreathFire()
    {
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
    void EndWingStrike()
    {
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


        prePosition = transform.position;
    }
}
