using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGui : MonoBehaviour {

    float baseWidth = 854f;
    float baseHeight = 480f;
    

    Vector2 playerStatusOffset = new Vector2(8f, 80f);

    Rect buttonRect = new Rect(0f, 0f, 50f, 50f);
    public Texture attack1;
    public Texture attack2;
    public Texture attack3;
    public Texture skill;

    void DrawPlayerButton()
    {
        float x = baseWidth - buttonRect.width - playerStatusOffset.x;
        float y = playerStatusOffset.y;

        
    }
    
    void DrawButtonRect(float x, float y, Rect buttonRect, Texture texture)
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
