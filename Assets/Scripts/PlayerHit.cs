﻿using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {
	
	//get script from parent class (Player.cs)
	Player playerScript;
	GenericEnemy enemyScript;
	SpriteRenderer playerRender;
	public GameObject healthBar;
    public AudioSource hitAudio;
    public AudioClip[] hitClips;
    // Use this for initialization
    void Start () {
		GetComponent<Renderer>().enabled = false;//makes catch radius invisible
		playerScript = transform.parent.GetComponent<Player>();
		playerRender = transform.parent.GetComponent<SpriteRenderer> ();
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag.Equals("EnemyArrow")) 
		{
            if (!hitAudio.isPlaying)
            {
                hitAudio.clip = hitClips[Random.Range(0, 2)];
                hitAudio.Play();
            }
            //cause vibration on damage(only mobile)
            if (PlayerPrefs.GetFloat("Vibrate")==1)
			{
#if UNITY_IPHONE || UNITY_ANDROID
                Handheld.Vibrate();
#endif

            }
            /////////////////////////////////////

            Debug.Log("Damage Taken!");
			playerRender.color = new Color(255f, 0f, 0f, 255f); //set to red
			Invoke("ReturnColor" , 0.2f);
			playerScript.health--;
			ObjectPool.instance.PoolObject(col.gameObject);
		}
		else if(col.gameObject.tag.Equals("BallistaBolt")) 
		{
			//cause vibration on damage(only mobile)
			if(PlayerPrefs.GetFloat("Vibrate")==1)
			{
#if UNITY_IPHONE || UNITY_ANDROID
                Handheld.Vibrate();
#endif
            }
            //////////////////////////////////

            Debug.Log("5 Damage Taken!");
			playerRender.color = new Color(255f, 0f, 0f, 255f); //set to red
			Invoke("ReturnColor" , 0.2f);
			playerScript.health-=5;
			ObjectPool.instance.PoolObject(col.gameObject);
		}
	}
	void ReturnColor()
	{
		playerRender.color = new Color(255f, 255f, 255f, 255f); //set to clear
	}
}