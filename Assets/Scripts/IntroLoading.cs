﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class IntroLoading : MonoBehaviour {
 
    IEnumerator Start()
	{
        yield return new WaitForSeconds(3f);
		Application.LoadLevel("Splash");
	}
}
