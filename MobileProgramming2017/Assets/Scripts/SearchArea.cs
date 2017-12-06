using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour {

    TerrorDragonCtrl enemyCtrl;

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
        enemyCtrl = transform.root.GetComponent<TerrorDragonCtrl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
