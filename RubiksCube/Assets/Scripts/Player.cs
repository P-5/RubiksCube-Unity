using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/**
 * 
 * This class controls progress updates.
 * 
 **/
[RequireComponent(typeof(NetworkManager))]
public class Player : NetworkBehaviour{
	private NetworkIdentity networkIdentity;
	private RubiksCube cube;
	private Text text;
	
	[SyncVar]
	public int progress;
	
	/**
	 * 
	 * This function get references.
	 * 
	 **/
	void Start() {
		networkIdentity = transform.GetComponent<NetworkIdentity>();
		cube = GameObject.Find("Canvas").GetComponent<Menu>().rubiksCube.GetComponent<RubiksCube>();
		text = GameObject.Find("Canvas").GetComponent<Menu>().otherPlayer.transform.GetChild(0).GetComponent<Text>();
		if(!networkIdentity.isLocalPlayer) {
			cube.gameObject.SetActive(true);
		}
	}
	
	/**
	 * 
	 * This function updates progress.
	 * 
	 **/
	void Update() {
		if(networkIdentity.isLocalPlayer) {
			CmdProgress(cube.GetProgress());
		}
		else {
			text.text = "The Challanger is " + progress + "% Complete";
			if(progress == 100) {
				float time = cube.time;
				Menu menu = GameObject.Find("Canvas").GetComponent<Menu>();
				menu.message.transform.Find("Message/Text").GetComponent<Text>().text = "New Message:\n> You lost (with a time of " + time + " seconds).";
				menu.UIUpdate("Message");
			}
		}
	}
	
	/**
	 * 
	 * Command to update progress.
	 * 
	 **/
	[Command]
	void CmdProgress(int progress) {
		this.progress = progress;
	}
}
