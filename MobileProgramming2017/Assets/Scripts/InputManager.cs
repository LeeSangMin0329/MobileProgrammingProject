using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    // slide variable
    Vector2 slideStartPosition;
    Vector2 prevPosition;
    Vector2 delta = Vector2.zero;
    bool moved = false;


    // charactor Ctrl
    public float horizontalMove;
    public float verticalMove;

    public bool tumbleTrigger = false;
    public bool attackTrigger = false;
	
	// Update is called once per frame
	void Update () {

        // wasd
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        // must change mobile touch / slide
        if(Input.GetButtonDown("Fire1"))
        {
            slideStartPosition = GetCursorPosition();
        }

        if(Input.GetButton("Fire1"))
        {
            if(Vector2.Distance(slideStartPosition, GetCursorPosition()) >= (Screen.width * 0.1f))
            {
                moved = true;
            }
        }

        if(!Input.GetButtonUp("Fire1") && !Input.GetButton("Fire1"))
        {
            moved = false;
        }
        
        if(moved)
        {
            delta = GetCursorPosition() - prevPosition;
        }
        else
        {
            delta = Vector2.zero;
        }

        prevPosition = GetCursorPosition();

        // tumble
        if (Input.GetButtonDown("Jump"))
        {
            tumbleTrigger = true;
        }
        else
        {
            tumbleTrigger = false;
        }

        // attack
        if (Input.GetButtonDown("Fire2"))
        {
            attackTrigger = true;
        }
        else
        {
            attackTrigger = false;
        }
	}

    

    public bool Clicked()
    {
        if(!moved && Input.GetButtonUp("Fire1"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Moved()
    {
        return moved;
    }

    public Vector2 GetDeltaPosition()
    {
        return delta;
    }

    public Vector2 GetCursorPosition()
    {
        return Input.mousePosition;
    }
}
