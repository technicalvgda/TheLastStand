﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : GenericCharacter
{

	public bool isdead = false;
	public float moveSpeed, ammo, ammoLimit, kills, killcap;
	public float acceleration = 35, currentSpeed = 0;
    //public Text[] livesUI, ammosUI, killsUI;
    //Text liveUI, ammoUI, killUI;
    public GameObject controls;
    Controls script;
    Vector3 mousePosition, diff, translate, temp;
	Vector3 aimVector; // for mobile only
	public bool killedBoss;
	public bool pause = false;
	//flag used to signal level end
	public bool levelFinish = false;
	private bool tutorial = true;
    public bool deathScene = false;
	public AudioSource fireSound;


    public float minX; //left boundary 
    public float maxX; //right boundary 
    public float minY; //up boundary 
    public float maxY; //down boundary
	
	//cursor texture code
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	//public AudioClip arrowCatch;
	//AudioSource audio;

    // Use this for initialization
    void Start()
    {
		//audio = GetComponent<AudioSource>();


		killedBoss = false;
		isdead = false;
		acceleration *= Time.deltaTime;

        if (!Application.isMobilePlatform)
        {
			//set cursor texture
			Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

            //liveUI = livesUI[0];
            //ammoUI = ammosUI[0];
            //killUI = killsUI[0];
        }
        else // if this is a mobile platform
        {
			//keep phone from sleeping
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			//disable mouse image 
			Cursor.visible = false;
            //liveUI = livesUI[1];
            //ammoUI = ammosUI[1];
            //killUI = killsUI[1];
            kills = 0;
            
        }
        script = controls.transform.GetComponent<Controls>();
		pause = (PauseMenu)GameObject.Find("PauseMenu").GetComponent("PauseMenu");
    }

    // Update is called once per frame
    void Update()
    {
		////tutorial section
		if(Application.loadedLevelName == "Level1-Village" && tutorial == true)
		{

			if(ammo == 1)
			{
				Camera.main.GetComponent<Lvl1Tutorial>().tutArrowCaught= true;
			}
			if(kills == 1)
			{
				Camera.main.GetComponent<Lvl1Tutorial>().tutEnemyDead = true;
				tutorial = false;
			}
		}
	
		/////
		if (pause == false) 
		{
				Move ();
				RotateToMouse ();
				BoundaryCheck ();
				Wincondition ();
		
        if (health <= 0)
        {
			
			if(Application.loadedLevelName == "Level3-Temple")
			{
				float highScore = PlayerPrefs.GetFloat("High Score");
				if(kills > highScore)
				{
					PlayerPrefs.SetFloat("High Score",kills);
				}
			}
			if(killedBoss == false)
			{
				isdead = true;
	            //Replace with an actual trigger i.e. Death
	            //change code to jump to game over
	            //Application.LoadLevel("GameOver");
	            //resetPlayer();
			}
        }


        //"Fire1" is the left mouse button, left ctrl, or gamepad button 0 (A button on xbox360 remote)
		if(!Application.isMobilePlatform)
		{
	        if (Input.GetButtonDown("Fire1"))//(Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Space))
	        {
	            Fire();
	        }
		}
        //ammoUI.text = "Ammo: " + ammo;
        //liveUI.text = "Lives: " + health;
        //killUI.text = "Kills: " + kills;
        ////////////////////////////////////////////cheat codes!
        if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.H))
        {
            health = 1000;
        }
        if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.B))
        {
            ammo = 1000;
        }
		if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.M))
		{
			health = 1;
		}
        /////////////////////////////////////////////////////////
		}
    }

    private void RotateToMouse()
    {
        float rotation;
        translate = script.getGamePadTranslate();
        if (translate.x != 0.0 || translate.y != 0.0)
        {
            rotation = Mathf.Atan2(translate.y, translate.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(90.0 - angle, Vector3.up);
			if(!Application.isMobilePlatform)
			{
            	transform.rotation = Quaternion.Euler(0f, 0f, rotation);
			}
            arrowDir = transform.rotation.eulerAngles;
        }
		if(Application.isMobilePlatform)
		{
			//rotate to second stick
			aimVector = script.getAimPadTranslate();
			rotation = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rotation);
			arrowDir = transform.rotation.eulerAngles;
		}
		else if (!Application.isMobilePlatform)
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            diff = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
            diff.Normalize();

	        rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
	        arrowDir = transform.rotation.eulerAngles;

        }
    }

    private void Move()
    {
        ///works with both keyboard and gamepad
		translate = script.getTranslate ();
		if (translate != Vector3.zero) {
	
			currentSpeed += acceleration;
			if (currentSpeed >= moveSpeed)
				currentSpeed = moveSpeed;
			transform.position += translate * currentSpeed * Time.deltaTime;
			temp = translate;
		}
		else {
			currentSpeed += -acceleration;
			if (currentSpeed <= 0)
				currentSpeed = 0;
			transform.position += temp * currentSpeed * Time.deltaTime;
		}
    }


    private void resetPlayer()
    {
        var spawnpoint = GameObject.FindWithTag("Respawn").transform;
        transform.position = spawnpoint.position;
        //set initial health
        health = 10;
    }
	/*
	 * no longer necessary
    ///cause player damage (collision with box collider)
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("EnemyArrow"))
        {
            health--;
            RePool(col.gameObject);
        }
    }
	*/
    public void BoundaryCheck()
    {
        float xboundary = Mathf.Clamp(transform.position.x, minX, maxX);
        float yboundary = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(xboundary, yboundary, 0);
    }

    public void Fire()
    {
        if (ammo > 0)
        {
			fireSound.Play ();
            fireArrow("PlayerArrow");
            ammo--;
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

    public void addAmmo(int difference)
    {
		if (ammo > 0)
            ammo += difference;
    }
	/*cursor code
	void OnMouseEnter() {
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	}
	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}
	*/
	public void Wincondition(){
		//Level1 win condition
			if(Application.loadedLevelName == "Level1-Village" && kills>=killcap){
				Debug.Log("You beat level 1!");
				//Application.LoadLevel ("Level2Cutscene");
				//Application.LoadLevel ("Level2-Forest");
				//Application.LoadLevel ("NarrationLvl2");
				levelFinish = true;
			}
		//Level2 win condition	
		else if(Application.loadedLevelName == "Level2-Forest" && kills>=killcap){// && killedBoss == true){
				Debug.Log("You beat level 2!");
				//Application.LoadLevel ("Level3Cutscene");
			    //Application.LoadLevel ("Level3-Temple");
			    //Application.LoadLevel ("NarrationLvl3");
				levelFinish = true;
		}
		//Level3 win condition
		else if(Application.loadedLevelName == "Level3-Temple" && killedBoss ==true && health < 1){
			//checks if endless mode is active. If not, jump to end cutscene
			//if so, display game over screen
			if(PlayerPrefs.GetInt ("Endless")==0)
			{
                deathScene = true;
				Debug.Log("You beat level 3! Congrats!");
				//Application.LoadLevel ("EndingCutscene"); can't load this scene for some reason
				//Application.LoadLevel ("TempEnding");//placeholder destination
				levelFinish = true;
			}
			else
			{
				isdead = true;
			}

		}

	}

}