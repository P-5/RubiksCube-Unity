using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
 * 
 * This class controls LAN discovery.
 * 
 **/
public class MultiplayerLAN : NetworkDiscovery {
	private Multiplayer multiplayer;
	
	void Start() {
		multiplayer = transform.GetComponent<Multiplayer>();
	}
	
	public override void OnReceivedBroadcast(string fromAddress, string data) {
		multiplayer.SetAddress(fromAddress);
	}
}
