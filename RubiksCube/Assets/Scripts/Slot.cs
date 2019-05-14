using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * This class controls game saving/loading.
 * 
 **/
public class Slot : MonoBehaviour {
	public string currentSlot;
	public Menu menu;
	
	/**
	 * 
	 * This function is called by UI buttons.
	 * The options parameter is the save slot.
	 * 
	 **/
	public void UIUpdate(string option) {
		menu.UIUpdate("Game");
		currentSlot = option;
		Load();
	}
	
	/**
	 * 
	 * This function saves the game to a file.
	 * 
	 **/
	public void Save() {
		if(currentSlot == "") {
			return;
		}
		
		Transform cube = transform.parent.GetComponent<Menu>().rubiksCube.transform;
		string data = "";
		for(int i = 0; i < cube.childCount; i ++) {
			Vector3 v = cube.GetChild(i).position;
			Quaternion q = cube.GetChild(i).rotation;
			data += "" + v.x + " " + v.y + " " + v.z + " ";
			data += "" + q.x + " " + q.y + " " + q.z + " " + q.w;
			data += "\n";
		}
		data += transform.parent.GetComponent<Menu>().rubiksCube.GetComponent<RubiksCube>().time;
		
		PlayerPrefs.SetString(currentSlot, data);
		currentSlot = "";
	}
	
	/**
	 * 
	 * This function saves the game to a file.
	 * 
	 **/
	public void Erase() {
		if(currentSlot == "" || !PlayerPrefs.HasKey(currentSlot)) {
			return;
		}
		
		PlayerPrefs.DeleteKey(currentSlot);
		currentSlot = "";
	}
	
	/**
	 * 
	 * This function loads the game from a file.
	 * 
	 **/
	public void Load() {
		if(currentSlot == "" || !PlayerPrefs.HasKey(currentSlot)) {
			return;
		}
		
		Transform cube = transform.parent.GetComponent<Menu>().rubiksCube.transform;
		string[] data = PlayerPrefs.GetString(currentSlot).Split('\n');
		if(data.Length != 27) {
			Erase();
			return;
		}
		for(int i = 0; i < data.Length-1; i ++) {
			float[] values = new float[7];
			string[] subData = data[i].Split(' ');
			for(int j = 0; j < 7; j ++) {
				values[j] = float.Parse(subData[j]);
			}
			cube.GetChild(i).position = new Vector3(values[0], values[1], values[2]);
			cube.GetChild(i).rotation = new Quaternion(values[3], values[4], values[5], values[6]);
		}
		transform.parent.GetComponent<Menu>().rubiksCube.GetComponent<RubiksCube>().time = float.Parse(data[data.Length-1]);
	}
}
