using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour {

    public GameObject gameOverUI;
	public AudioClip deathSound;
	private AudioSource source;
	public float RespawnTime;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
			//TODO desactivate player controls
			gameOverUI.SetActive(true);
			//MusicPlayer.instance.playSong(deathSound);
			//source.PlayOneShot(deathSound);
			//other.transform.position = CheckpointManagerScript.GetLastCheckpoint();
			StartCoroutine(RespawnPlayer(other.gameObject, other.GetComponent<CharacterSwitcher>().Unlink()));
		}

		if (other.tag == "Suku" && !other.gameObject.GetComponent<IAFollower>().isPlayer)
		{
			source.PlayOneShot(deathSound);
			StartCoroutine(RespawnSuku(other.gameObject));

		}
	}

	IEnumerator RespawnSuku(GameObject o)
	{
		yield return new WaitForSeconds(RespawnTime);
		o.transform.position = CheckpointManagerScript.GetLastCheckpoint();
	}

	IEnumerator RespawnPlayer(GameObject o, GameObject suku)
	{
		yield return new WaitForSeconds(RespawnTime);
		o.transform.position = CheckpointManagerScript.GetLastCheckpoint();
		suku.transform.position = CheckpointManagerScript.GetLastCheckpoint();
		o.GetComponent<CharacterSwitcher>().Link(suku);
	}

}
