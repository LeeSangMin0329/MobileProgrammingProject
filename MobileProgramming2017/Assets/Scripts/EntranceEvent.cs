using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceEvent : MonoBehaviour {

    GameRuleCtrl gameRuleCtrl;
    int readyCount = 0;

    InputManager inputManager;
    CharacterStatus charaStatus;
    
	// Use this for initialization
	void Start () {
        inputManager = FindObjectOfType<InputManager>();
        gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();
        charaStatus = GetComponent<CharacterStatus>();
        
    }
	
    void ChangeScene()
    {
        Resources.UnloadUnusedAssets();
        LoadingSceneManager.LoadScene("Hunt00");
    }
     

    
    bool entranceTrigger = true;
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Entrance")
        {
            if (inputManager.basicAttackTrigger1)
            {
                ChangeScene();
            }
        }
    }
}
