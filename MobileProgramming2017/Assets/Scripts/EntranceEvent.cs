using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceEvent : MonoBehaviour {

    GameRuleCtrl gameRuleCtrl;
    int readyCount = 0;

    InputManager inputManager;
    CharacterStatus charaStatus;
    NetworkView netView;

	// Use this for initialization
	void Start () {
        inputManager = FindObjectOfType<InputManager>();
        gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();
        charaStatus = GetComponent<CharacterStatus>();
        netView = GetComponent<NetworkView>();
	}
	
    [RPC]
    void ReadyOn()
    {
        
        readyCount++;
        Debug.Log(readyCount + " " + gameRuleCtrl.userCount);
        if (readyCount == gameRuleCtrl.userCount)
        {
            netView.RPC("ChangeScene", RPCMode.All);
        }
    }
    [RPC]
    void ReadyOut()
    {
        readyCount--;
    }
    [RPC]
    void ChangeScene()
    {
        Destroy(gameRuleCtrl.gameObject);
        Destroy(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Hunt00");
    }
     

    
    bool entranceTrigger = true;
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Entrance")
        {
            if (inputManager.basicAttackTrigger1)
            {
                if (entranceTrigger)
                {
                    
                    netView.RPC("ReadyOn", RPCMode.Server);
                    if (Network.isServer)
                    {
                        ReadyOn();
                    }
                    entranceTrigger = false;
                }
                else
                {
                    netView.RPC("ReadyOut", RPCMode.Server);
                    if (Network.isServer)
                    {
                        ReadyOut();
                    }
                    entranceTrigger = true;
                }
            }
        }
    }
}
