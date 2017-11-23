using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonAnimation : MonoBehaviour {

    Animator animator;
    TerrorDragonStatus status;
    Vector3 prePosition;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 deltaPosition = transform.position - prePosition;
        animator.SetFloat("Speed", deltaPosition.magnitude / Time.deltaTime);



        prePosition = transform.position;
    }
}
