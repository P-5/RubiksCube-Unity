using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
 * 
 * This class runs tests.
 * 
 **/
public class Tests : MonoBehaviour {
	public bool runTests;
	
	/**
	 * 
	 * If runTests is true, run tests.
	 * 
	 **/
	void Update() {
		if(runTests){
			runTests = false;
			
			print(TestMenu());
			print(TestRubiksCube());
			print(TestScores());
			print(TestSlot());
			print(TestTouchCamera());
		}
	}
	
	/**
	 * 
	 * Tests. Names should be self explain
	 * purpose.
	 * 
	 **/
	string TestMenu() {
		return "Menu Test - Success (No Tests)";
	}
	string TestRubiksCube() {
		RubiksCube cube = transform.GetComponent<Menu>().rubiksCube.GetComponent<RubiksCube>();
		
		if(cube.cubes.Length != 26) {
			return "Test Rubiks Cube - Failed (Broken Rubiks Cube)";
		}
		if(!cube.IsComplete()) {
			return "Test Rubiks Cube - Failed (Incorrect Init)";
		}
		
		int layer = (int)UnityEngine.Random.Range(-1,2);
		int direction = (int)Mathf.Sign(UnityEngine.Random.Range(-1,1));
		cube.RotateFaceX(layer, direction);
		if(cube.IsComplete()) {
			return "Test Rubiks Cube - Failed (X Failure)";
		}
		cube.RotateFaceX(layer, -direction);
		if(!cube.IsComplete()) {
			return "Test Rubiks Cube - Failed (X Failure)";
		}
		
		layer = (int)UnityEngine.Random.Range(-1,2);
		direction = (int)Mathf.Sign(UnityEngine.Random.Range(-1,1));
		cube.RotateFaceY(layer, direction);
		if(cube.IsComplete()) {
			return "Test Rubiks Cube - Failed (Y Failure)";
		}
		cube.RotateFaceY(layer, -direction);
		if(!cube.IsComplete()) {
			return "Test Rubiks Cube - Failed (Y Failure)";
		}
		
		layer = (int)UnityEngine.Random.Range(-1,2);
		direction = (int)Mathf.Sign(UnityEngine.Random.Range(-1,1));
		cube.RotateFaceZ(layer, direction);
		if(cube.IsComplete()) {
			return "Test Rubiks Cube - Failed (Z Failure)";
		}
		cube.RotateFaceZ(layer, -direction);
		if(!cube.IsComplete()) {
			return "Test Rubiks Cube - Failed (Z Failure)";
		}
		
		cube.Randomize();
		if(cube.IsComplete()) {
			return "Test Rubiks Cube - Failed (No Entropy)";
		}
		
		return "Test Rubiks Cube - Success (9 Tests)";
	}
	string TestScores() {
		PlayerPrefs.DeleteAll();
		Scores scores = transform.GetComponent<Menu>().scores.GetComponent<Scores>();
		
		if(scores.Get("1st") != "" || scores.Get("2nd") != "" || scores.Get("3rd") != "" || scores.Get("4th") != "" || scores.Get("5th") != "") {
			return "Test Scores - Failed (Incorrect Init)";
		}
		
		scores.Save("A", 1.0f);
		scores.Save("B", 1.5f);
		scores.Save("C", 1.25f);
		
		string date = DateTime.Now.ToString("MM-dd-yy");
		if(scores.Get("1st") != "A (" + date + ") 1000 pts in 1 secs.") {
			return "Test Scores - Failed (Incorrect 1st)";
		}
		if(scores.Get("2nd") != "C (" + date + ") 800 pts in 1 secs.") {
			return "Test Scores - Failed (Incorrect 2nd)";
		}
		if(scores.Get("3rd") != "B (" + date + ") 666 pts in 1 secs.") {
			return "Test Scores - Failed (Incorrect 3rd)";
		}
		if(scores.Get("4th") != "") {
			return "Test Scores - Failed (Incorrect 4th)";
		}
		
		return "Test Scores - Success (4 Tests)";
	}
	string TestSlot() {
		return "Test Slot - Success (No Tests)";
	}
	string TestTouchCamera() {
		return "Test Touch Camera - Success (No Tests)";
	}
}
