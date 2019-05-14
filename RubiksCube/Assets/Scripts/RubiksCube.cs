using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * This class controls the Rubik's Cube.
 * 
 **/
public class RubiksCube : MonoBehaviour {
	public enum Face {Red, Green, Blue, Orange, Yellow, White, Null};
	
	// Remember that the YFace is negated.
	[System.Serializable]
	public struct Cube {
		public Face xFace;
		public Face yFace;
		public Face zFace;
		public Transform prefab;
		public Vector3 position;
		public Quaternion rotation;
		
		public Cube(Transform prefab, Face xFace, Face yFace, Face zFace) {
			this.prefab = prefab;
			this.position = Vector3.zero;
			this.rotation = Quaternion.identity;
			this.xFace = xFace;
			this.yFace = yFace;
			this.zFace = zFace;
		}
	}
	
	public Cube[] cubes;
	public float time;
	public int noise;
	
	// Increment timer while the cube is
	// active.
	void Update() {
		time += Time.deltaTime;
	}
	
	/**
	 * 
	 * This function checks if the cube is
	 * solved and returns a boolean.
	 * 
	 **/
	public bool IsComplete() {
		int[] colors = new int[7];
		foreach(Cube cube in cubes) {
			colors[(int)cube.xFace] ++;
			colors[(int)cube.yFace] ++;
			colors[(int)cube.zFace] ++;
		}
		
		// Ensure the cube is legal.
		if(colors[0] != 9) {
			print("Error: " + colors[0] + " red faces.");
			return false;
		}
		if(colors[1] != 9) {
			print("Error: " + colors[1] + " green faces.");
			return false;
		}
		if(colors[2] != 9) {
			print("Error: " + colors[2] + " blue faces.");
			return false;
		}
		if(colors[3] != 9) {
			print("Error: " + colors[3] + " orange faces.");
			return false;
		}
		if(colors[4] != 9) {
			print("Error: " + colors[4] + " yellow faces.");
			return false;
		}
		if(colors[5] != 9) {
			print("Error: " + colors[5] + " white faces.");
			return false;
		}
		if(colors[6] != 24) {
			print("Error: " + colors[6] + " black faces.");
			return false;
		}
		
		// Ensure each face is uniform.
		if(FaceComposition(-1, 0, 0).Max() != 9) {
			print("Error: -X contains more than one color.");
			return false;
		}
		if(FaceComposition(1, 0, 0).Max() != 9) {
			print("Error: X contains more than one color.");
			return false;
		}
		if(FaceComposition(0, -1, 0).Max() != 9) {
			print("Error: -Y contains more than one color.");
			return false;
		}
		if(FaceComposition(0, 1, 0).Max() != 9) {
			print("Error: Y contains more than one color.");
			return false;
		}
		if(FaceComposition(0, 0, -1).Max() != 9) {
			print("Error: -Z contains more than one color.");
			return false;
		}
		if(FaceComposition(0, 0, 1).Max() != 9) {
			print("Error: Z contains more than one color.");
			return false;
		}
		
		return true;
	}
	
	/**
	 * 
	 * This function returns cube progress.
	 * 
	 **/
	public int GetProgress() {
		int[] colors = new int[7];
		foreach(Cube cube in cubes) {
			colors[(int)cube.xFace] ++;
			colors[(int)cube.yFace] ++;
			colors[(int)cube.zFace] ++;
		}
		
		float progress = FaceComposition(-1, 0, 0).Max() / 9.0f;
		progress += FaceComposition(1, 0, 0).Max() / 9.0f;
		progress += FaceComposition(0, -1, 0).Max() / 9.0f;
		progress += FaceComposition(0, 1, 0).Max() / 9.0f;
		progress += FaceComposition(0, 0, -1).Max() / 9.0f;
		progress += FaceComposition(0, 0, 1).Max() / 9.0f;
		
		return (int)Mathf.Round(100 * (progress / 6));
	}
	
	/**
	 * 
	 * This function returns the colors
	 * that a face is composed of.
	 * 
	 **/
	int[] FaceComposition(int x, int y, int z) {
		if(Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z) != 1) {
			return new int[7];
		}
		
		// Find the face's cubes.
		List<Cube> face = new List<Cube>();
		foreach(Cube cube in cubes) {
			if(x != 0 && Mathf.Round(cube.prefab.position.x) == x) {
				face.Add(cube);
				continue;
			}
			if(y != 0 && Mathf.Round(cube.prefab.position.y) == y) {
				face.Add(cube);
				continue;
			}
			if(z != 0 && Mathf.Round(cube.prefab.position.z) == z) {
				face.Add(cube);
				continue;
			}
		}
		
		// Count the face's colors.
		int[] colors = new int[7];
		foreach(Cube cube in face) {
			Vector3 side = cube.prefab.InverseTransformDirection(new Vector3(x, y, z));
			side.x = Mathf.Round(side.x);
			side.y = Mathf.Round(side.y);
			side.z = Mathf.Round(side.z);
			if(side == Vector3.right) {
				colors[(int)cube.xFace] ++;
				continue;
			}
			if(side == -Vector3.up) {
				colors[(int)cube.yFace] ++;
				continue;
			}
			if(side == Vector3.forward) {
				colors[(int)cube.zFace] ++;
				continue;
			}
		}
		
		return colors;
	}
	
	/**
	 * 
	 * These functions rotate the given
	 * layer around the X, Y, or Z axis
	 * in either the -1 or +1 direction.
	 * 
	 **/
	public bool RotateFaceX(int layer, int direction) {
		if(1 < Mathf.Abs(layer) || Mathf.Abs(direction) != 1) {
			return false;
		}
		
		List<Cube> face = new List<Cube>();
		foreach(Cube cube in cubes) {
			if(Mathf.Round(cube.prefab.position.x) == layer) {
				face.Add(cube);
				continue;
			}
		}
		
		foreach(Cube cube in face) {
			cube.prefab.RotateAround(Vector3.zero, Vector3.right, 90 * direction);
		}
		
		return true;
	}
	public bool RotateFaceY(int layer, int direction) {
		if(1 < Mathf.Abs(layer) || Mathf.Abs(direction) != 1) {
			return false;
		}
		
		List<Cube> face = new List<Cube>();
		foreach(Cube cube in cubes) {
			if(Mathf.Round(cube.prefab.position.y) == layer) {
				face.Add(cube);
				continue;
			}
		}
		
		foreach(Cube cube in face) {
			cube.prefab.RotateAround(Vector3.zero, Vector3.up, 90 * direction);
		}
		
		return true;
	}
	public bool RotateFaceZ(int layer, int direction) {
		if(1 < Mathf.Abs(layer) || Mathf.Abs(direction) != 1) {
			return false;
		}
		
		List<Cube> face = new List<Cube>();
		foreach(Cube cube in cubes) {
			if(Mathf.Round(cube.prefab.position.z) == layer) {
				face.Add(cube);
				continue;
			}
		}
		
		foreach(Cube cube in face) {
			cube.prefab.RotateAround(Vector3.zero, Vector3.forward, 90 * direction);
		}
		
		return true;
	}
	
	/**
	 * 
	 * This function randomizes the cube.
	 * 
	 **/
	public void Randomize() {
		for(int i = 0; i < cubes.Length; i ++) {
			Cube cube = cubes[i];
			cube.prefab.position = cube.position;
			cube.prefab.rotation = cube.rotation;
			cubes[i] = cube;
		}
		
		Transform camera = transform.GetComponent<TouchCamera>().camera.transform;
		camera.position = Vector3.forward * -5;
		camera.rotation = Quaternion.identity;
		
		time = 0;
		
		for(int i = 0; i < noise; i ++) {
			switch(Random.Range(0,3)) {
				case 0:
					RotateFaceX(Random.Range(-1,2), (int)Mathf.Sign(Random.value));
					break;
				case 1:
					RotateFaceY(Random.Range(-1,2), (int)Mathf.Sign(Random.value));
					break;
				case 2:
					RotateFaceZ(Random.Range(-1,2), (int)Mathf.Sign(Random.value));
					break;
			}
		}
		time = 0;
	}
}
