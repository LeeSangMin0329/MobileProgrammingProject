using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoySticksCtrl : MonoBehaviour {

    InputManager inputManager;

    public Transform Stick;

    private Vector3 StickFirstPos;
    private Vector3 JoyVec;
    private float Radius;

    // Use this for initialization
	void Start () {
        inputManager = FindObjectOfType<InputManager>();
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = Stick.transform.position;

        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

	}

    void Update()
    {
        inputManager.horizontalMove = JoyVec.x;
        inputManager.verticalMove = JoyVec.y;
        
    }
	
    public void Drag(BaseEventData _Data)
    {
        PointerEventData Data = _Data as PointerEventData;
        Vector3 Pos = Data.position;

        JoyVec = (Pos - StickFirstPos).normalized;

        float Dis = Vector3.Distance(Pos, StickFirstPos);

        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        else
            Stick.position = StickFirstPos + JoyVec * Radius;
    }

    public void DragEnd()
    {
        Stick.position = StickFirstPos;
        JoyVec = Vector3.zero;
    }
	
}
