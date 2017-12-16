using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UnityEngine.EventSystems;

public class InputManager : NetworkBehaviour {

    // slide variable
    Vector2 slideStartPosition;
    Vector2 prevPosition;
    Vector2 delta = Vector2.zero;
    bool moved = false;


    // charactor Ctrl
    public float horizontalMove;
    public float verticalMove;

    public bool tumbleTrigger = false;
    public bool basicAttackTrigger1 = false;
    public bool basicAttackTrigger2 = false;
    public bool basicAttackTrigger3 = false;

    public bool skillTrigger = false;


    Touch tempTouchs;
    Vector3 touchedPos;
    
    // Update is called once per frame
    void Update () {
        
        if(Input.touchCount > 0)
        {
            for(int i=0; i<Input.touchCount; i++)
            {
                if(EventSystem.current.IsPointerOverGameObject(i) == false)
                {
                    tempTouchs = Input.GetTouch(i);
                    if (tempTouchs.phase == TouchPhase.Began)
                    {
                        slideStartPosition = GetCursorPosition();
                    }

                    if (tempTouchs.phase == TouchPhase.Moved)
                    {
                        if (Vector2.Distance(slideStartPosition, GetCursorPosition()) >= (Screen.width * 0.05f))
                        {
                            moved = true;
                        }
                    }

                    if (tempTouchs.phase == TouchPhase.Ended)
                    {
                        moved = false;
                    }
                    if (moved)
                    {
                        delta = GetCursorPosition() - prevPosition;
                    }
                    else
                    {
                        delta = Vector2.zero;
                    }

                    prevPosition = GetCursorPosition();
                }

            }
        }


        

    }


    public void OnAttack1Down()
    {
        basicAttackTrigger1 = true;
    }
    public void OnAttack1Up()
    {
        basicAttackTrigger1 = false;
    }
    public void OnAttack2Down()
    {
        basicAttackTrigger2 = true;
    }
    public void OnAttack2Up()
    {
        basicAttackTrigger2 = false;
    }
    public void OnAttack3Down()
    {
        basicAttackTrigger3 = true;
    }
    public void OnAttack3Up()
    {
        basicAttackTrigger3 = false;
    }

    
    public void OnSkillDown()
    {
        skillTrigger = true;
    }
    public void OnSkillUp()
    {
        skillTrigger = false;
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
