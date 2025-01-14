﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
  * Scene:All
  * Object:SoundManager
  * Description: Skripta zaduzena za zvuke u apliakciji, njihovo pustanje, gasenje itd...
  **/
public class SoundManager : MonoBehaviour {

	public static int musicOn = 1;
	public static int soundOn = 1;
	public static bool forceTurnOff = false;


	 
	public AudioSource gameplayMusic;
	public AudioSource ButtonClick;
	public AudioSource ButtonClick2;
	public AudioSource PopUpShow;
	public AudioSource PopUpHide;

	public AudioSource ActionCompleted;
	public AudioSource ActionFailedSound;
	public AudioSource  Decoration;
	public AudioSource  CameraSound;
	public AudioSource  MoldSelected;
	public AudioSource  WaterSound;
	public AudioSource  ShowFreezer;
	public AudioSource  FreezerDoorOpen;
	public AudioSource  FreezerDoorClose;
	public AudioSource  MachineOnSound;
	public AudioSource  InsertSweets;
	public AudioSource  InsertFruit;
	public AudioSource  Locked;
	public AudioSource  MixerSound;
	public AudioSource  MixerSound2;
	public AudioSource Coins;

	public AudioSource EatSound;
	public AudioSource TimerSound;
	public AudioSource OutOfTimeSound;
	public AudioSource SugarSound;
	public AudioSource CutDoughSound;
	public AudioSource KnifeCutSound;
	public AudioSource KnifeCutSound2;
	public AudioSource OnOffSound;
	public AudioSource FryingSound;
	public AudioSource FryingSound2;

	public AudioSource PlugInSound;
	public AudioSource ShowItemSound;

	public AudioSource OilSound;
	public AudioSource RollingPinSound;
	public AudioSource RollInSugarSound;
	public AudioSource PickMeatSound;
	public AudioSource SauceSound;


	public float OriginalMusicVolume;

    public static SoundManager Instance;

	public List<AudioSource> listStopSoundOnExit = new  List<AudioSource>();
    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start () 
	{

		OriginalMusicVolume = gameplayMusic.volume;
		DontDestroyOnLoad(this.gameObject);

		if(PlayerPrefs.HasKey("SoundOn"))
		{
			soundOn = PlayerPrefs.GetInt("SoundOn",1);
			if(SoundManager.soundOn == 0) MuteAllSounds();
			else UnmuteAllSounds();
		}
		else
		{
			SetSound(true);
		}

		musicOn = PlayerPrefs.GetInt("MusicOn",1);
		if(musicOn == 1)Play_Music();
		else Stop_Music();

		Screen.sleepTimeout = SleepTimeout.NeverSleep; 

	}

	public void SetSound(bool bEnabled)
	{
		if(bEnabled)
		{
			PlayerPrefs.SetInt("SoundOn", 1);
			UnmuteAllSounds();
		}
		else
		{
			PlayerPrefs.SetInt("SoundOn", 0);
			MuteAllSounds();
		}

		soundOn = PlayerPrefs.GetInt("SoundOn");
	}

	public void Play_ButtonClick()
	{
		if(ButtonClick.clip != null && soundOn == 1)
			ButtonClick.Play();
	}

//	public void Play_MenuMusic()
//	{
//		if(menuMusic.clip != null && musicOn == 1)
//			menuMusic.Play();
//	}
//
//	public void Stop_MenuMusic()
//	{
//		if(menuMusic.clip != null && musicOn == 1)
//			menuMusic.Stop();
//	}

	public void Play_Music()
	{
		if(gameplayMusic.clip != null && musicOn == 1 && !gameplayMusic.isPlaying)
		{
			gameplayMusic.volume = OriginalMusicVolume;
			gameplayMusic.Play();
		}
	}

	public void Stop_Music()
	{
		if(gameplayMusic.clip != null && musicOn == 1)
		{
			StartCoroutine(FadeOut(gameplayMusic, 0.1f));
		}
	}

 
 
//	public void Play_TaskCompleted()
//	{
//		if(ElementCompleted.clip != null&& soundOn == 1)
//			ElementCompleted.Play();
//	}

 

	public void Play_PopUpShow(float time = 0)
	{
		if(PopUpShow.clip != null && soundOn == 1)
			StartCoroutine(PlayClip(PopUpShow,time));
			 
	}

	public void Play_PopUpHide(float time = 0)
	{
		if(PopUpHide.clip != null && soundOn == 1)
			StartCoroutine(PlayClip(PopUpHide,time));
		
	}

	IEnumerator PlayClip(AudioSource Clip, float time)
	{
		yield return new WaitForSeconds(time);
		Clip.Play();
	}



	/// <summary>
	/// Corutine-a koja za odredjeni AudioSource, kroz prosledjeno vreme, utisava AudioSource do 0, gasi taj AudioSource, a zatim vraca pocetni Volume na pocetan kako bi AudioSource mogao opet da se koristi
	/// </summary>
	/// <param name="sound">AudioSource koji treba smanjiti/param>
	/// <param name="time">Vreme za koje treba smanjiti Volume/param>
	IEnumerator FadeOut(AudioSource sound, float time)
	{
		float originalVolume = sound.volume;

		if(sound.name == gameplayMusic.name) originalVolume = OriginalMusicVolume;


		while(sound.volume > 0.05f)
		{
			sound.volume = Mathf.MoveTowards(sound.volume, 0, time);
			yield return null;
		}
		sound.Stop();
		sound.volume = originalVolume;
	}

	/// <summary>
	/// F-ja koja Mute-uje sve zvuke koja su deca SoundManager-a
	/// </summary>
	public void MuteAllSounds()
	{
		foreach (Transform t in transform)
		{
			t.GetComponent<AudioSource>().mute = true;
		}
	}

	/// <summary>
	/// F-ja koja Unmute-uje sve zvuke koja su deca SoundManager-a
	/// </summary>
	public void UnmuteAllSounds()
	{
		foreach (Transform t in transform)
		{
			t.GetComponent<AudioSource>().mute = false;
		}
	}

	public void	Play_Sound(AudioSource sound)
	{
		if(!sound.isPlaying  && soundOn == 1) 
			sound.Play();
	}

	public void	StopAndPlay_Sound(AudioSource sound)
	{
		if(sound.isPlaying)
			sound.Stop();

		if( soundOn == 1) 
			sound.Play();
	}
	
	public void	Stop_Sound(AudioSource sound)
	{
		
		if(sound.isPlaying)
			sound.Stop();
	}


	public void ChangeSoundVolume(AudioSource sound, float time, float value)
	{
		if(value>1) value = 1;
		if(value<0) value = 0;
		if( (musicOn == 1 && sound.name == gameplayMusic.name ) || (soundOn == 1 &&  sound.name != gameplayMusic.name )) 
		{
			StartCoroutine( _ChangeVolume( sound, time, value));
		}
		 
	}

    IEnumerator _ChangeVolume(AudioSource sound, float time,float value)
	{
		float _time = 0;
		yield return new WaitForFixedUpdate();
		while( _time <1)
		{
			_time+= Time.fixedDeltaTime/time;
			sound.volume = Mathf.Lerp(sound.volume, value, _time );
			yield return new WaitForFixedUpdate();
		}
		 
	}
	 
	public void  StopActiveSoundsOnExitAndClearList()
	{
		foreach (AudioSource a in listStopSoundOnExit)
		{
			a.Stop();
		}
		listStopSoundOnExit.Clear();
	}



	public void	StopAndPlay_Sound(AudioSource sound, float time)
	{
		if(  soundOn == 1) StartCoroutine(StopAndPlay_SoundDly(sound,time));
	}

	IEnumerator StopAndPlay_SoundDly(AudioSource sound, float time)
	{
		yield return new WaitForSeconds(time);
		if(sound.isPlaying)
			sound.Stop();
		
		if( soundOn == 1) 
			sound.Play();
	}

	public void	 Play_Sound(AudioSource sound, float time)
	{
		if(  soundOn == 1) StartCoroutine(Play_SoundDly(sound,time));
	}

   IEnumerator Play_SoundDly(AudioSource sound, float time)
   {
		yield return new WaitForSeconds(time);
		if(!sound.isPlaying  && soundOn == 1) 
			sound.Play();
	}

}
