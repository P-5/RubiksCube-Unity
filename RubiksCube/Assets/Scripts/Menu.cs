using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * 
 * This class controls UI toggling.
 * 
 **/
public class Menu : MonoBehaviour {
	public GameObject message;
	public GameObject mainMenu;
	public GameObject slotMenu;
	public GameObject playMenu;
	public GameObject pauseMenu;
	public GameObject instructions;
	public GameObject scores;
	public GameObject rubiksCube;
	public GameObject multiplayer;
	public GameObject otherPlayer;
	
	public bool erase;
	
	/**
	 * 
	 * Only the main menu is active to begin.
	 * 
	 **/
	void Start() {
		message.SetActive(false);
		mainMenu.SetActive(true);
		slotMenu.SetActive(false);
		playMenu.SetActive(false);
		pauseMenu.SetActive(false);
		instructions.SetActive(false);
		scores.SetActive(false);
		rubiksCube.SetActive(false);
		otherPlayer.SetActive(false);
		multiplayer.GetComponent<Multiplayer>().Disconnect();
		
		if(erase) {
			PlayerPrefs.DeleteAll();
		}
	}
	
	/**
	 * 
	 * This function is called by UI buttons.
	 * The options parameter is the button id.
	 * 
	 **/
	public void UIUpdate(string option) {
		switch(option) {
			case "Single Player":
				mainMenu.SetActive(false);
				slotMenu.SetActive(true);
				break;
			case "Multiplayer":
				multiplayer.GetComponent<Multiplayer>().Connect();
				rubiksCube.GetComponent<RubiksCube>().noise = 2;
				otherPlayer.SetActive(true);
				mainMenu.SetActive(false);
				playMenu.SetActive(true);
				rubiksCube.GetComponent<RubiksCube>().Randomize();
				rubiksCube.GetComponent<TouchCamera>().enabled = true;
				break;
			case "Scores":
				mainMenu.SetActive(false);
				scores.SetActive(true);
				break;
			case "Finish":
				Application.Quit();
				break;
			case "Easy":
				rubiksCube.GetComponent<RubiksCube>().noise = 2;
				break;
			case "Medium":
				rubiksCube.GetComponent<RubiksCube>().noise = 10;
				break;
			case "Hard":
				rubiksCube.GetComponent<RubiksCube>().noise = 50;
				break;
			case "Game":
				slotMenu.SetActive(false);
				playMenu.SetActive(true);
				rubiksCube.SetActive(true);
				rubiksCube.GetComponent<RubiksCube>().Randomize();
				rubiksCube.GetComponent<TouchCamera>().enabled = true;
				break;
			case "Menu":
				playMenu.SetActive(false);
				pauseMenu.SetActive(true);
				rubiksCube.GetComponent<TouchCamera>().enabled = false;
				break;
			case "Continue A":
				pauseMenu.SetActive(false);
				playMenu.SetActive(true);
				rubiksCube.GetComponent<TouchCamera>().enabled = true;
				break;
			case "Continue B":
				instructions.SetActive(false);
				pauseMenu.SetActive(true);
				break;
			case "Continue C":
				message.SetActive(false);
				slotMenu.GetComponent<Slot>().Erase();
				scores.GetComponent<Scores>().Save(message.transform.Find("Name/Text").GetComponent<Text>().text, rubiksCube.GetComponent<RubiksCube>().time);
				rubiksCube.GetComponent<RubiksCube>().Randomize();
				Start();
				break;
			case "Instructions":
				pauseMenu.SetActive(false);
				instructions.SetActive(true);
				break;
			case "Main Menu":
				slotMenu.GetComponent<Slot>().Save();
				Start();
				break;
			case "Message":
				playMenu.SetActive(false);
				message.SetActive(true);
				rubiksCube.SetActive(false);
				break;
			case "Share":
				#if UNITY_ANDROID
					AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
					AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
					intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
					intentObject.Call<AndroidJavaObject>("setType", "text/plain");
					intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Rubik's Cube (CS 242)");
					intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "I just solved a cube in " + rubiksCube.GetComponent<RubiksCube>().time + " seconds!");
					AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
					AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
					currentActivity.Call ("startActivity", intentObject);
				#endif
				UIUpdate("Continue C");
				break;
		}
	}
}
