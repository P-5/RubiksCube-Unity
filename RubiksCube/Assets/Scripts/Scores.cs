using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/**
 * 
 * This class controls the scoreboard.
 * 
 **/
public class Scores : MonoBehaviour {
	/**
	 * 
	 * Close scores on touch.
	 * 
	 **/
	void Update() {
		if(Input.GetMouseButtonDown(0) || 0 < Input.touchCount) {
			transform.parent.GetComponent<Menu>().UIUpdate("Main Menu");
		}
	}
	
	/**
	 * 
	 * Refresh the scoreboard when the UI
	 * is opened.
	 * 
	 **/
	void OnEnable() {
		Load();
	}
	
	/**
	 * 
	 * This function loads the scoreboard.
	 * 
	 **/
	void Load() {
		transform.Find("1st/Text").GetComponent<Text>().text = Get("1st");
		transform.Find("2nd/Text").GetComponent<Text>().text = Get("2nd");
		transform.Find("3rd/Text").GetComponent<Text>().text = Get("3rd");
		transform.Find("4th/Text").GetComponent<Text>().text = Get("4th");
	}
	
	/**
	 * 
	 * This function loads a score.
	 * 
	 **/
	public string Get(string slot) {
		if(!PlayerPrefs.HasKey(slot + "Name")) {
			return "";
		}
		
		string name = PlayerPrefs.GetString(slot + "Name");
		string date = PlayerPrefs.GetString(slot + "Date");
		float time = PlayerPrefs.GetFloat(slot + "Time");
		int score = PlayerPrefs.GetInt(slot + "Score");
		
		return name + " (" + date + ") " + score + " pts in " + (int)time + " secs.";
	}
	
	public void Save(string name, float time) {
		string date = DateTime.Now.ToString("MM-dd-yy");
		int score = (int)(1000 / time);
		
		string[] names = new string[5];
		string[] dates = new string[5];
		float[] times = new float[5];
		int[] scores = new int[5];
		
		names[0] = name;
		dates[0] = date;
		times[0] = time;
		scores[0] = score;
		
		if(PlayerPrefs.HasKey("1stName")) {
			names[1] = PlayerPrefs.GetString("1stName");
			dates[1] = PlayerPrefs.GetString("1stDate");
			times[1] = PlayerPrefs.GetFloat("1stTime");
			scores[1] = PlayerPrefs.GetInt("1stScore");
		}
		else {
			times[1] = Mathf.Infinity;
		}
		
		if(PlayerPrefs.HasKey("2ndName")) {
			names[2] = PlayerPrefs.GetString("2ndName");
			dates[2] = PlayerPrefs.GetString("2ndDate");
			times[2] = PlayerPrefs.GetFloat("2ndTime");
			scores[2] = PlayerPrefs.GetInt("2ndScore");
		}
		else {
			times[2] = Mathf.Infinity;
		}
		
		if(PlayerPrefs.HasKey("3rdName")) {
			names[3] = PlayerPrefs.GetString("3rdName");
			dates[3] = PlayerPrefs.GetString("3rdDate");
			times[3] = PlayerPrefs.GetFloat("3rdTime");
			scores[3] = PlayerPrefs.GetInt("3rdScore");
		}
		else {
			times[3] = Mathf.Infinity;
		}
		
		if(PlayerPrefs.HasKey("4thName")) {
			names[4] = PlayerPrefs.GetString("4thName");
			dates[4] = PlayerPrefs.GetString("4thDate");
			times[4] = PlayerPrefs.GetFloat("4thTime");
			scores[4] = PlayerPrefs.GetInt("4thScore");
		}
		else {
			times[4] = Mathf.Infinity;
		}
		
		int min = Min(times);
		if(times[min] != Mathf.Infinity) {
			PlayerPrefs.SetString("1stName", names[min]);
			PlayerPrefs.SetString("1stDate", dates[min]);
			PlayerPrefs.SetFloat("1stTime", times[min]);
			PlayerPrefs.SetInt("1stScore", scores[min]);
			times[min] = Mathf.Infinity;
		}
		min = Min(times);
		if(times[min] != Mathf.Infinity) {
			PlayerPrefs.SetString("2ndName", names[min]);
			PlayerPrefs.SetString("2ndDate", dates[min]);
			PlayerPrefs.SetFloat("2ndTime", times[min]);
			PlayerPrefs.SetInt("2ndScore", scores[min]);
			times[min] = Mathf.Infinity;
		}
		min = Min(times);
		if(times[min] != Mathf.Infinity) {
			PlayerPrefs.SetString("3rdName", names[min]);
			PlayerPrefs.SetString("3rdDate", dates[min]);
			PlayerPrefs.SetFloat("3rdTime", times[min]);
			PlayerPrefs.SetInt("3rdScore", scores[min]);
			times[min] = Mathf.Infinity;
		}
		min = Min(times);
		if(times[min] != Mathf.Infinity) {
			PlayerPrefs.SetString("4thName", names[min]);
			PlayerPrefs.SetString("4thDate", dates[min]);
			PlayerPrefs.SetFloat("4thTime", times[min]);
			PlayerPrefs.SetInt("4thScore", scores[min]);
			times[min] = Mathf.Infinity;
		}
	}
	
	int Min(float[] values) {
		int min = 0;
		float minValue = values[0];
		for(int i = 1; i < values.Length; i ++) {
			if(values[i] < minValue) {
				minValue = values[i];
				min = i;
			}
		}
		
		return min;
	}
}
