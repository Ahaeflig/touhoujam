using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public float timeUntilLoad = 1;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start () {
		if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Invoke("LoadLevelNext", timeUntilLoad);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel (string levelName)
    {
        Debug.Log("Level loaded : " + levelName);
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevelNext ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
