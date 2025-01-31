﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
* Scene:All
* Object:Canvas
* Description: Skripta zaduzena za hendlovanje(prikaz i sklanjanje svih Menu-ja,njihovo paljenje i gasenje, itd...)
**/
public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance;
	public Menu currentMenu;
	Menu currentPopUpMenu;
	public static string activeMenu = "";

	//	[HideInInspector]
	//	public Animator openObject;
	public GameObject[] disabledObjects;
	GameObject ratePopUp;

	public static bool bFirstLoadMainScene = true;

	DailyRewards dailyaReward = null;
	public static bool bPopUpVisible = false;
	CanvasGroup BlockAll;
	GameObject PopUpSpecialOffer;

	void Awake()
	{
		Instance = this;
	}

	//void Update()
	//{
	//    if (Input.GetKeyDown(KeyCode.Q))
	//    {
	//        AdsManager.bannerHeight = 0;
	//        AdsManager.Instance.RefreshUIBannerHeight();
	//    }
	//    if (Input.GetKeyDown(KeyCode.A))
	//    {
	//        AdsManager.bannerHeight = 130;
	//        AdsManager.Instance.RefreshUIBannerHeight();
	//    }
	//}

	void Start()
	{

		activeMenu = "";
		if (SceneManager.GetActiveScene().name == "HomeScene")
		{
			//Hendlujemo banner za cross promo

			ratePopUp = GameObject.Find("PopUps/PopUpRate");


			if (GameObject.Find("PopUps/DailyReward") != null) dailyaReward = GameObject.Find("PopUps/DailyReward").GetComponent<DailyRewards>();

			PopUpSpecialOffer = GameObject.Find("PopUps/PopUpSpecialOffer");
		}

		if (disabledObjects != null)
		{
			for (int i = 0; i < disabledObjects.Length; i++)
				disabledObjects[i].SetActive(false);
		}

		if (SceneManager.GetActiveScene().name == "HomeScene")
		{
			if (bFirstLoadMainScene)
				StartCoroutine(DelayStartMainScene());
			else
			{
			}
		}

	}





	IEnumerator DelayStartMainScene()
	{

		//if (BlockAll == null) BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		//BlockAll.blocksRaycasts = true;

		yield return new WaitForSeconds(0.5f);

        if (bFirstLoadMainScene)
        {
            //if (Rate.alreadyRated == 0 && Rate.appStartedNumber >= 6)
            //{
            //    Rate.appStartedNumber = 0;
            //    PlayerPrefs.SetInt("appStartedNumber", Rate.appStartedNumber);
            //    PlayerPrefs.Save();

            //    //ShowPopUpMenu(ratePopUp);
            //}
            //else
            //{
            //    if (bFirstLoadMainScene && dailyaReward != null && dailyaReward.TestDailyRewards())
            //    {
            //        dailyaReward.ShowDailyReward();
            //    }
            //    else
            //    {
            //        //if( bFirstLoadMainScene) ShowStartInterstitial();
            //        //if(dailyaReward !=null ) dailyaReward.gameObject.SetActive(false);
            //    }
            //}
        }
        else
        {
            //ShowPopUpMenu ( PopUpSpecialOffer);
        }

        bFirstLoadMainScene = false;

        yield return new WaitForSeconds(1.1f);
		//BlockAll.blocksRaycasts = false;
	}





	/// <summary>
	/// Funkcija koja pali(aktivira) objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se upali</param>
	public void EnableObject(GameObject gameObject)
	{

		if (gameObject != null)
		{
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
		}
	}

	/// <summary>
	/// Funkcija koja gasi objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se ugasi</param>
	public void DisableObject(GameObject gameObject)
	{

		if (gameObject != null)
		{
			if (gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
	/// F-ja koji poziva ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadScene(string levelName)
	{
		if (levelName != "")
		{
			try
			{
				Application.LoadLevel(levelName);
			}
			catch (System.Exception e)
			{
				Debug.Log("Can't load scene: " + e.Message);
			}
		}
		else
		{
			Debug.Log("Can't load scene: Level name to set");
		}
	}

	/// <summary>
	/// F-ja koji poziva asihrono ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadSceneAsync(string levelName)
	{
		if (levelName != "")
		{
			try
			{
				Application.LoadLevelAsync(levelName);
			}
			catch (System.Exception e)
			{
				Debug.Log("Can't load scene: " + e.Message);
			}
		}
		else
		{
			Debug.Log("Can't load scene: Level name to set");
		}
	}




	/// <summary>
	/// Funkcija za prikaz Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
	public void ShowPopUpMenu(GameObject menu)
	{
		menu.gameObject.SetActive(true);
		currentPopUpMenu = menu.GetComponent<Menu>();
		currentPopUpMenu.OpenMenu();
		activeMenu = menu.name; //PROBLEM AKO SE OTVORE VISE MENIJA
		bPopUpVisible = true;
		if (SoundManager.Instance != null)
		{
			//if(Time.timeScale>0 ) SoundManager.Instance.Play_PopUpShow(.05f);
			//else 
			SoundManager.Instance.Play_Sound(SoundManager.Instance.PopUpShow);
		}
		if (menu.name == "PopUpAreYouSure") EscapeButtonManager.AddEscapeButonFunction("ButtonHomeYesClicked");
		else if (menu.name != "PopUpPause") EscapeButtonManager.AddEscapeButonFunction("ClosePopUpMenuEsc", menu.name);


		//if(FBNativeAdsController.Instance!=null)	FBNativeAdsController.Instance.ShowPopUp( menu.name);
		FBShowPopUp(menu.gameObject);
	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao PopUp-a, poziva inace coroutine-u, ima delay zbog animacije odlaska Menu-ja
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ClosePopUpMenu(GameObject menu)
	{
		//StartCoroutine("HidePopUp",menu);
		menu.GetComponent<Menu>().CloseMenu();
		if (activeMenu == menu.name) activeMenu = "";
		if (SoundManager.Instance != null) SoundManager.Instance.Play_PopUpHide(.4f);
		bPopUpVisible = false;
		/*if(SceneManager.GetActiveScene().name != "Room") */
		if (EscapeButtonManager.EscapeButonFunctionStack.Count >= 1) EscapeButtonManager.EscapeButonFunctionStack.Pop();


		//if(FBNativeAdsController.Instance!=null)	FBNativeAdsController.Instance.HidePopUp( menu.name);
		FBHidePopUp(menu.gameObject);
	}

	public void ClosePopUpMenuEsc(string menuName)
	{
		if (GameObject.Find(menuName) != null)
			ClosePopUpMenu(GameObject.Find(menuName));
	}


	/// <summary>
	/// Couorutine-a za zatvaranje Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje, mora imati na sebi skriptu Menu.</param>
	IEnumerator HidePopUp(GameObject menu)
	{
		yield return null;
		//menu.GetComponent<Menu> ().IsOpen = false;
		menu.GetComponent<Menu>().CloseMenu();
		//yield return new WaitForSeconds(1.2f);
		//menu.SetActive (false);
	}

	/// <summary>
	/// Funkcija za prikaz poruke preko Log-a, prilikom klika na dugme
	/// </summary>
	/// /// <param name="message">poruka koju treba prikazati.</param>
	public void ShowMessage(string message)
	{
		Debug.Log(message);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	/// <param name="messageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpMessage(string messageTitleText, string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = messageTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = messageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);

	}

	/// <summary>
	/// Funkcija koja podesava naslov CustomMessage-a, i ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageCustomMessageText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpMessageTitleText(string messageTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = messageTitleText;
	}

	/// <summary>
	/// Funkcija koja podesava poruku CustomMessage, i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageTitleText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpMessageCustomMessageText(string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = messageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialog(string dialogTitleText, string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = dialogTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = dialogMessageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogCustomMessageText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpDialogTitleText(string dialogTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = dialogTitleText;
	}

	/// <summary>
	/// Funkcija koja podesava poruku dialoga i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogTitleText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
	/// </summary>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialogCustomMessageText(string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = dialogMessageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	public void btnClicked_PlaySound()
	{
		if (SoundManager.Instance != null) SoundManager.Instance.Play_ButtonClick();
	}



	public void OnDestroy()
	{
		Instance = null;
	}

	public void FBShowPopUp(GameObject popupGo)
	{
		if (Instance == null) return;
		StartCoroutine("WaitFBShowPopUp", popupGo);
	}

	IEnumerator WaitFBShowPopUp(GameObject popupGo)
	{
		//Debug.Log("WaitFBShowPopUp 0");
		//        yield return new WaitForSecondsRealtime(0.5f);
		yield return null;
		if (popupGo.name == "PopUpAreYouSure")
		{
            //popupGo.transform.Find("AnimationHolder/NativeAdWithCover").gameObject.SetActive(true);
            //popupGo.transform.Find("AnimationHolder/NativeAdWithCover/LoadingHolder").gameObject.SetActive(true);
           
            yield return null;
		}

	}

	public void FBHidePopUp(GameObject popupGo)
	{
		if (Instance == null) return;
        popupGo.SetActive(false);
        //StartCoroutine("WaitFBHidePopUp", popupGo);
    }



	public void LevelTransitionOn()
	{
	
	}

	public void LevelTransitionOff()
	{
		
	}

	public void OpenPrivacyPolicyLink()
	{
		//Application.OpenURL(AdsManager.Instance.privacyPolicyLink);
	}
}
