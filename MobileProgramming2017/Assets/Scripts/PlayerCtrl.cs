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



    enum State { Walk, Tumble};
    State state = State.Walk;

    // Use this for initialization
    void Start () {
        inputManager = FindObjectOfType<InputManager>();
        characterMove = GetComponent<CharacterMove>();
        tumbleDestination = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

        

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

    public void StartTumble()
    {
        state = State.Tumble;
        tumbleDestination = transform.position + transform.forward * tumbleDistance * Time.deltaTime;
    }
}
