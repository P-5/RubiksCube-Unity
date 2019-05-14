using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * 
 * This class controls camera movements.
 * 
 **/
public class TouchCamera : MonoBehaviour {
	public Transform rotationArrows;
	public float tapSensitivity;
	public float rotateSpeed;
	public float zoomSpeed;
	public Camera camera;
	
	private bool zooming;
	private float lastDelta;
	private Vector3 lastPosition;
	private Vector3 firstPosition;
	
	/**
	 * 
	 * Depending on platform we call different
	 * functions to process input.
	 * 
	 **/
	void Update() {
		// Note that if using a device with a
		// touch screen mouse input will not
		// be processed. Add a call to the
		// UpdateMouse function to the IF
		// statement to change/
		if(Input.touchSupported) {
			UpdateTouch();
		}
		else {
			UpdateMouse();
		}
	}
	
	/**
	 * 
	 * This function moves the camera based on
	 * mouse movement and scrolling.
	 * 
	 **/
	void UpdateMouse() {
		if(Input.GetMouseButtonDown(0)) {
			lastPosition = Input.mousePosition;
			firstPosition = lastPosition;
		}
		if(Input.GetMouseButton(0)) {
			Vector3 newPosition = Input.mousePosition;
			Rotate(newPosition.x - lastPosition.x, newPosition.y - lastPosition.y);
			lastPosition = newPosition;
		}
		if(Input.GetMouseButtonUp(0)) {
			if((lastPosition - firstPosition).magnitude < tapSensitivity) {
				Tap(lastPosition);
			}
		}
		Zoom(Input.GetAxis("Mouse ScrollWheel"));
	}
	
	/**
	 * 
	 * This function moves the camera based on
	 * swiping and pinching touches.
	 * 
	 **/
	void UpdateTouch() {
		if(Input.touchCount == 1) {
			zooming = false;
			
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Began) {
				lastPosition = touch.position;
				firstPosition = lastPosition;
			}
			if(touch.phase == TouchPhase.Moved) {
				Vector3 newPosition = touch.position;
				Rotate(newPosition.x - lastPosition.x, newPosition.y - lastPosition.y);
				lastPosition = newPosition;
			}
			if(touch.phase == TouchPhase.Ended) {
				if((lastPosition - firstPosition).magnitude < tapSensitivity) {
					Tap(lastPosition);
				}
			}
		}
		else if(Input.touchCount == 2) {
			Vector2 touchA = Input.GetTouch(0).position;
			Vector2 touchB = Input.GetTouch(1).position;
			if(zooming) {
				float delta = Vector2.Distance(touchA, touchB);
				Zoom(delta - lastDelta);
				lastDelta = delta;
			}
			else {
				zooming = true;
				lastDelta = Vector2.Distance(touchA, touchB);
			}
		}
		else {
			// More then two touches results in
			// no movement. Possible room for
			// change in future versions.
			zooming = false;
		}
	}
	
	/**
	 * 
	 * This function rotates the camera.
	 * 
	 **/
	void Rotate(float x, float y) {
		Vector3 axis = camera.transform.up * x - camera.transform.right * y;
		camera.transform.RotateAround(Vector3.zero, axis.normalized, axis.magnitude * rotateSpeed);
	}
	
	/**
	 * 
	 * This function zoom the camera.
	 * Note that this effect is done through FOV,
	 * not movement, so it doesn't affect Rotate.
	 * 
	 **/
	void Zoom(float zoom) {
		camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - zoom * zoomSpeed, 50, 100);
	}
	
	/**
	 * 
	 * This function rotates the cube's "slices".
	 * 
	 **/
	void Tap(Vector3 position) {
		Ray ray = camera.ScreenPointToRay(position);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit)) {
			if(hit.transform.root != rotationArrows) {
				rotationArrows.gameObject.SetActive(true);
				rotationArrows.position = hit.point;
				if(0.9f < Mathf.Abs(Vector3.Dot(hit.normal, Vector3.right))) {
					rotationArrows.rotation = Quaternion.Euler(0, 90, 0);
				}
				if(0.9f < Mathf.Abs(Vector3.Dot(hit.normal, Vector3.up))) {
					rotationArrows.rotation = Quaternion.Euler(90, 0, 0);
				}
				if(0.9f < Mathf.Abs(Vector3.Dot(hit.normal, Vector3.forward))) {
					rotationArrows.rotation = Quaternion.Euler(0, 0, 0);
				}
			}
			else {
				rotationArrows.gameObject.SetActive(false);
				if(0.9f < Mathf.Abs(Vector3.Dot(hit.transform.up, Vector3.right))) {
					if(0.9f < Mathf.Abs(hit.transform.forward.z)) {
						transform.GetComponent<RubiksCube>().RotateFaceY((int)Mathf.Round(hit.point.y), -(int)(Mathf.Sign(hit.transform.position.x)*Mathf.Sign(hit.transform.forward.z)));
					}
					if(0.9f < Mathf.Abs(hit.transform.forward.y)) {
						transform.GetComponent<RubiksCube>().RotateFaceZ((int)Mathf.Round(hit.point.z), (int)(Mathf.Sign(hit.transform.position.x)*Mathf.Sign(hit.transform.forward.y)));
					}
				}
				if(0.9f < Mathf.Abs(Vector3.Dot(hit.transform.up, Vector3.up))) {
					if(0.9f < Mathf.Abs(hit.transform.forward.x)) {
						transform.GetComponent<RubiksCube>().RotateFaceZ((int)Mathf.Round(hit.point.z), -(int)(Mathf.Sign(hit.transform.position.y)*Mathf.Sign(hit.transform.forward.x)));
					}
					if(0.9f < Mathf.Abs(hit.transform.forward.z)) {
						transform.GetComponent<RubiksCube>().RotateFaceX((int)Mathf.Round(hit.point.x), (int)(Mathf.Sign(hit.transform.position.y)*Mathf.Sign(hit.transform.forward.z)));
					}
				}
				if(0.9f < Mathf.Abs(Vector3.Dot(hit.transform.up, Vector3.forward))) {
					if(0.9f < Mathf.Abs(hit.transform.forward.x)) {
						transform.GetComponent<RubiksCube>().RotateFaceY((int)Mathf.Round(hit.point.y), (int)(Mathf.Sign(hit.transform.position.z)*Mathf.Sign(hit.transform.forward.x)));
					}
					if(0.9f < Mathf.Abs(hit.transform.forward.y)) {
						transform.GetComponent<RubiksCube>().RotateFaceX((int)Mathf.Round(hit.point.x), -(int)(Mathf.Sign(hit.transform.position.z)*Mathf.Sign(hit.transform.forward.y)));
					}
				}
				if(transform.GetComponent<RubiksCube>().IsComplete()) {
					float time = transform.GetComponent<RubiksCube>().time;
					Menu menu = GameObject.Find("Canvas").GetComponent<Menu>();
					menu.message.transform.Find("Message/Text").GetComponent<Text>().text = "New Message:\n> You win (with a time of " + time + " seconds).";
					menu.UIUpdate("Message");
				}
			}
		}
		else {
			rotationArrows.gameObject.SetActive(false);
		}
	}
}
