﻿using UnityEngine;
using System.Collections;

public class StageSelect : MonoBehaviour {

	public void OnClickLevel1()
	{
		//Application.LoadLevel("BasicLevel");
		Application.LoadLevel("Level1Cutscene");
	}
	public void OnClickLevel5()
	{
		//Application.LoadLevel("BasicLevel");
		Application.LoadLevel("Level5-Temple");
	}
}