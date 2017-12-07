using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleCtrl : MonoBehaviour {

    public float timeRemaining = 5.0f * 60.0f;
    public bool gameOver = false;
    public bool gameClear = false;

    public float sceneChangeTime = 3.0f;

    public GameObject player;
    public GameObject playerPrefab;
    public Transform startPoint;
    public FollowCamera followCamera;

    public int userCount = 0;

    NetworkView netView;

    [RPC]
    void IncreaceUserCount()
    {
        userCount++;
        Debug.Log(userCount);
    }

	// Use this for initialization
	void Start () {
        netView = GetComponent<NetworkView>();
	}

    // Update is called once per frame
    void Update() {
        // player character create
        if (player == null && (Network.isServer || Network.isClient))
        {
            // position divide
            Vector3 shiftVector = new Vector3(Network.connections.Length * 1.5f, 0, 0);
            player = Network.Instantiate(playerPrefab, startPoint.position + shiftVector, startPoint.rotation, 0) as GameObject;
            followCamera.SetTarget(player.transform);
            player.GetComponent<PlayerCtrl>().SetCamera(followCamera.GetComponent<Camera>());
            netView.RPC("IncreaceUserCount", RPCMode.Server);
            if (Network.isServer)
            {
                IncreaceUserCount();
            }
        }

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
