using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCharacterCollision : MonoBehaviour {

    CharacterController charController;
    float speed = 10.0f;
	// Use this for initialization
	void Start () {
        charController = transform.root.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider hit)
    {
        Debug.Log("on trigger hit");
        Vector3 direction = transform.position - hit.transform.position;
        direction = direction.normalized;

        charController.Move(direction * speed * Time.deltaTime);
    }
}
