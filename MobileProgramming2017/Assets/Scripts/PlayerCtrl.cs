using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    InputManager inputManager;

    public Vector3 movementHorizon;
    public Vector3 movementVertical;
    public Camera charactorCamera;

    public float speed = 200f;

    // Use this for initialization
    void Start () {
        inputManager = FindObjectOfType<InputManager>();
	}
	
	// Update is called once per frame
	void Update () {
        Walking();
	}

    void Walking()
    {
        movementHorizon.x = inputManager.horizontalMove * charactorCamera.transform.right.x;
        movementHorizon.y = 0;
        movementHorizon.z = inputManager.horizontalMove * charactorCamera.transform.right.z;

        movementVertical.x = inputManager.verticalMove * charactorCamera.transform.forward.x;
        movementVertical.y = 0;
        movementVertical.z = inputManager.verticalMove * charactorCamera.transform.forward.z;



        SendMessage("SetDestination", transform.position + (movementHorizon + movementVertical) * speed * Time.deltaTime);
    }
}
