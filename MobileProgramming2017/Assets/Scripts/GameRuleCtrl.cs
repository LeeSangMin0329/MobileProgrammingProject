using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleCtrl : MonoBehaviour {

    public float timeRemaining = 5.0f * 60.0f;
    public bool gameOver = false;
    public bool gameClear = false;

    public float sceneChangeTime = 3.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(gameOver || gameClear)
        {
            sceneChangeTime -= Time.deltaTime;
            if(sceneChangeTime <= 0.0f)
            {
                // time out game over
                UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
            }
            return;
        }

        timeRemaining -= Time.deltaTime;

        if(timeRemaining <= 0.0f)
        {
            GameOver();
        }
	}

    public void GameOver()
    {
        gameOver = true;
        Debug.Log("GameOver");
    }

    public void GameClear()
    {
        gameClear = true;
        Debug.Log("GameClear");
    }
}
