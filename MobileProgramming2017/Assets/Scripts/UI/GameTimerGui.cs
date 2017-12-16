using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimerGui : MonoBehaviour {

    GameRuleCtrl gameRuleCtrl;

    float baseWidth = 854f;
    float baseHeight = 480f;

    public Texture timerIcon;
    public GUIStyle timerLabelStyle;

	// Use this for initialization
	void Awake () {
        gameRuleCtrl = FindObjectOfType(typeof(GameRuleCtrl)) as GameRuleCtrl;
	}
	
	// Update is called once per frame
	void OnGUI () {
        GUI.matrix = Matrix4x4.TRS(
            Vector3.zero,
            Quaternion.identity,
            new Vector3(Screen.width / baseWidth, Screen.height / baseHeight, 1f));

        GUI.Label(
            new Rect(450f, 8f, 128f, 48f),
            new GUIContent(gameRuleCtrl.timeRemaining.ToString("0"), timerIcon),
            timerLabelStyle);
    }
}
