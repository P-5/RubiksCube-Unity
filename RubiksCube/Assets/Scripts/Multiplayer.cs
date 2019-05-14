using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/**
 * 
 * This class controls multiplayer.
 * 
 **/
[RequireComponent(typeof(NetworkManager))]
[RequireComponent(typeof(MultiplayerLAN))]
public class Multiplayer : MonoBehaviour {
	private NetworkManager networkManager;
	private MultiplayerLAN multiplayerLAN;
	
	public string address = "";
	
	/**
	 * 
	 * Start the LAN broadcast.
	 * 
	 **/
	void Start() {
		networkManager = transform.GetComponent<NetworkManager>();
		multiplayerLAN = transform.GetComponent<MultiplayerLAN>();
		
		multiplayerLAN.Initialize();
		multiplayerLAN.StartAsClient();
	}
	
	/**
	 * 
	 * Hosts if no one is found.
	 * 
	 **/
	public void Connect() {
		if(address != "") {
			Client();
		}
		else {
			Server();
		}
	}
	
	/**
	 * 
	 * This function starts a server.
	 * 
	 **/
	void Server() {
		networkManager.StartHost();
		multiplayerLAN.StopBroadcast();
		multiplayerLAN.StartAsServer();
	}
	
	/**
	 * 
	 * This function starts a client.
	 * 
	 **/
	void Client() {
		networkManager.networkAddress = address;
		networkManager.StartClient();
		multiplayerLAN.StopBroadcast();
	}
	
	/**
	 * 
	 * This function disconnects.
	 * 
	 **/
	public void Disconnect() {
		networkManager.StopHost();
		networkManager.StopClient();
	}
	
	/**
	 * 
	 * This function sets the LAN address.
	 * 
	 **/
	public void SetAddress(string address) {
		this.address = address;
	}
}