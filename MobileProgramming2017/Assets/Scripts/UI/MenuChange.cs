using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChange : MonoBehaviour {

	public void SelectGameStart()
    {
        Resources.UnloadUnusedAssets();
        UnityEngine.SceneManagement.SceneManager.LoadScene("BaseCamp");
    }

    public void SelectExit()
    {
        Application.Quit();
    }
}
