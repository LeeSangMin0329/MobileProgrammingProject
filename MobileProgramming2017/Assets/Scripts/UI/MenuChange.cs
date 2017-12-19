using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChange : MonoBehaviour {
    public AudioClip startSeClip;
    AudioSource startSeAudio;
    
    void Start()
    {
        startSeAudio = gameObject.AddComponent<AudioSource>();

        startSeAudio.loop = false;
        startSeAudio.clip = startSeClip;

    }

	public void SelectGameStart()
    {
        startSeAudio.Play();
        Resources.UnloadUnusedAssets();
        LoadingSceneManager.LoadScene("BaseCamp");
    }

    public void SelectExit()
    {
        startSeAudio.Play();
        Application.Quit();
    }
}
