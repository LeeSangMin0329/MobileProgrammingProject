using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamePlate : MonoBehaviour {

    public Vector3 offset = new Vector3(0, 2.5f, 0);
    public CharacterStatus status;
    TextMesh textMesh;

	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if(textMesh.text != status.characterName)
        {
            textMesh.text = status.characterName;
        }

        transform.position = status.transform.position + offset;
        transform.rotation = Camera.main.transform.rotation;

        float scale = Camera.main.transform.InverseTransformPoint(transform.position).z / 30.0f;
        transform.localScale = Vector3.one * scale;
	}
}
