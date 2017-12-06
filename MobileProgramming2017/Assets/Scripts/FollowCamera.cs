using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public float distance = 5.0f;
    public float horizontalAngle = 0.0f;
    public float rotAngle = 180.0f;
    public float verticalAngle = 10.0f;
    public float cameraFix = 1.0f;
    public Transform lookTarget = null;
    public Vector3 offset = Vector3.zero;

    Vector3 lookPosition;
    Vector3 relativePos;
    Vector3 dir;

    InputManager inputManager;

	// Use this for initialization
	void Start () {
        inputManager = FindObjectOfType<InputManager>();
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(inputManager.Moved())
        {
            float anglePerPixel = rotAngle / (float)Screen.width;
            Vector2 delta = inputManager.GetDeltaPosition();
            horizontalAngle += delta.x * anglePerPixel;
            horizontalAngle = Mathf.Repeat(horizontalAngle, 360.0f);
            verticalAngle -= delta.y * anglePerPixel;
            verticalAngle = Mathf.Clamp(verticalAngle, -60.0f, 60.0f);
        }

        if(lookTarget != null)
        {
            lookPosition = lookTarget.position + offset;
            relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0) *
                new Vector3(0, 0, -distance);

            transform.position = lookPosition + relativePos;

            transform.LookAt(lookPosition);

            RaycastHit hitInfo;
            if (Physics.Linecast(lookPosition, transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Ground")))
            {
                transform.position = hitInfo.point;
                if(cameraFix > 0)
                {
                    transform.Translate(dir * cameraFix);
                }
            }
        }
	}

    public void SetTarget(Transform target)
    {
        lookTarget = target;
        dir = (lookTarget.position + offset) - transform.position;
        dir.Normalize();
    }
}
