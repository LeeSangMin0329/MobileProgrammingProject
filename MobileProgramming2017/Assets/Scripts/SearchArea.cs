using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour {

    EnemyCtrl enemyCtrl;

    // @override collider
    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            enemyCtrl.SetAttackTarget(other.transform);
        }
    }

	// Use this for initialization
	void Start () {
        enemyCtrl = transform.root.GetComponent<EnemyCtrl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
