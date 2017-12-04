using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour {

    const float GravityPower = 9.8f;
    
    const float StoppingDistance = 0.01f;

    Vector3 velocity = Vector3.zero;

    CharacterController characterController;

    bool arrived = false;
    bool useGravity = true;
   
    // forced move
    bool forceRotate = false;
    Vector3 forceRotateDirection;
    float height;
    
    public Vector3 targetControllerOffset;

    public Vector3 destination;

    public float currentSpeed = 6.0f;
    public float walkSpeed = 6.0f;
    public float tumbleSpeed = 12.0f;
    public float rotationSpeed = 360.0f;

	// Use this for initialization
	void Start () {
        characterController = GetComponent<CharacterController>();
        destination = transform.position;
        height = transform.position.y;
        targetControllerOffset = characterController.center;
	}

    // Update is called once per frame
    void Update() {
        

        if (useGravity)
        {
            if (characterController.isGrounded)
            {
                Vector3 destinationXZ = destination;
                destinationXZ.y = transform.position.y;

                Vector3 direction = (destinationXZ - transform.position).normalized;

                float distance = Vector3.Distance(transform.position, destinationXZ);

                Vector3 currentVelocity = velocity;

                if (arrived || distance < StoppingDistance)
                    arrived = true;

                if (arrived)
                {
                    velocity = Vector3.zero;
                }
                else
                {
                    velocity = direction * currentSpeed;
                }

                // smoothing
                velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
                velocity.y = 0;

                if (!forceRotate)
                {
                    if (velocity.magnitude > 0.1f && !arrived)
                    {
                        Quaternion characterTargetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                            characterTargetRotation, rotationSpeed * Time.deltaTime);

                    }
                }
                else
                {
                    Quaternion characterTargetRotation = Quaternion.LookRotation(forceRotateDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,
                        characterTargetRotation, rotationSpeed * Time.deltaTime);
                }

            }
        }
        else // not use gravity
        {
            Vector3 destinationXZ = destination;
            destinationXZ.y = transform.position.y;
            Vector3 direction = (destinationXZ - transform.position).normalized;

            float distance = Vector3.Distance(transform.position, destinationXZ);

            Vector3 currentVelocity = velocity;

            if (arrived || distance < StoppingDistance)
                arrived = true;

            if (arrived)
            {
                velocity = Vector3.zero;
            }
            else
            {
                velocity = direction * currentSpeed;
            }

            
            // smoothing
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));

            

            if (!forceRotate)
            {
                if (velocity.magnitude > 0.1f && !arrived)
                {
                    Quaternion characterTargetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,
                        characterTargetRotation, rotationSpeed * Time.deltaTime);

                }
            }
            else
            {
                Quaternion characterTargetRotation = Quaternion.LookRotation(forceRotateDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    characterTargetRotation, rotationSpeed * Time.deltaTime);
            }
            
        }

        if (Vector3.Distance( characterController.center, targetControllerOffset) > 0.01f)
        {
            if(characterController.center.y > targetControllerOffset.y)
            {
                characterController.center = Vector3.Lerp(characterController.center, targetControllerOffset, Time.deltaTime);
            }
            else
            {
                characterController.center = Vector3.Lerp(characterController.center, targetControllerOffset, 0.4f * Time.deltaTime);
            }
        }

        Vector3 snapGround = Vector3.zero;
        if (useGravity)
        {
            //gravity
            velocity += Vector3.down * GravityPower * Time.deltaTime;

            
            if (characterController.isGrounded)
                snapGround = Vector3.down;

            characterController.Move(velocity * Time.deltaTime + snapGround);

        }
        else
        {
           
            // use up only not in ground position
            if (transform.position.y - height > 0.01f || transform.position.y - height < -0.01f)
            {
                Vector3 basePosition = transform.position;

                basePosition.y = height;
                transform.position = Vector3.Lerp(transform.position, basePosition, 0.3f * Time.deltaTime);
            }
            else
            {
                characterController.Move(velocity * Time.deltaTime + snapGround);
            }
        }
        
        if (characterController.velocity.magnitude < 0.1f)
            arrived = true;

        if (forceRotate && Vector3.Dot(transform.forward, forceRotateDirection) > 0.99f)
            forceRotate = false;

       
    }

    public void UseGravity(bool trigger)
    {
        useGravity = trigger;
    }

    public void SetDestination(Vector3 destination)
    {
        currentSpeed = walkSpeed;
        arrived = false;
        this.destination = destination;
    }

    public void SetTumbleDestination(Vector3 destination)
    {
        currentSpeed = tumbleSpeed;
        arrived = false;
        this.destination = destination;
    }

    public void SetDirection(Vector3 direction)
    {
        forceRotateDirection = direction;
        forceRotateDirection.y = 0;
        forceRotateDirection.Normalize();
        forceRotate = true;
    }

    public void StopMove()
    {
        destination = transform.position;
    }

    public bool Arrived()
    {
        return arrived;
    }

    public void SetHeight(float height)
    {
        this.height = height;
    }

    public void SetControllerOffsetY(float height)
    {
        targetControllerOffset = characterController.center;
        targetControllerOffset.y = height;
    }
}
