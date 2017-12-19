using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleCtrl : MonoBehaviour {

    public float timeRemaining = 5.0f * 60.0f;
    public bool gameOver = false;
    public bool gameClear = false;

    public float sceneChangeTime = 10.0f;

    public GameObject player;
    public GameObject playerPrefab;
    public Transform startPoint;
    public FollowCamera followCamera;

    NetworkView netView;

    public GameObject gameDefeatSprite;
    public GameObject gameClearSprite;

    
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

            NetworkManager networkManager = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
            player.GetComponent<NetworkView>().RPC("SetName", RPCMode.AllBuffered, networkManager.GetPlayerName());

        }

        if(gameOver || gameClear)
        {
            sceneChangeTime -= Time.deltaTime;
            if(sceneChangeTime <= 0.0f)
            {
                // time out game over
                Resources.UnloadUnusedAssets();
                LoadingSceneManager.LoadScene("BaseCamp");
            }
            return;
        }

        if((Network.isServer || Network.isClient))
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0.0f)
            {
                GameOver();
            }
        }
	}

    public void GameOver()
    {
        if(!gameOver && Network.isServer)
        {
            netView.RPC("GameOverOnNetwork", RPCMode.AllBuffered);
        }
        
    }

    [RPC]
    void GameOverOnNetwork()
    {
        gameOver = true;
        if (gameDefeatSprite)
        {
            gameDefeatSprite.SetActive(true);
        }
        Debug.Log("GameOver");
    }

    public void GameClear()
    {
        if(!gameClear && Network.isServer)
        {
            netView.RPC("GameClearOnNetwork", RPCMode.AllBuffered);
        }
    }

    [RPC]
    void GameClearOnNetwork()
    {
        gameClear = true;
        if (gameClearSprite)
        {
            gameClearSprite.SetActive(true);
        }
        Debug.Log("GameClear");
    }

    [RPC]
    void SetRemainTime(float time)
    {
        timeRemaining = time;
    }


    // on Server method
    // when other client access to server
    void OnPlayerConnected(NetworkPlayer player)
    {
        netView.RPC("SetRemainTime", player, timeRemaining);
    }
}
