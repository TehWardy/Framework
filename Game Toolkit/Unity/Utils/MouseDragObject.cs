using UnityEngine;
using System.Collections;

public class MouseDragObject : MonoBehaviour {
	
	float depthIntoScene = 10;
	MouseUtils.Button respondToMouseButton = MouseUtils.Button.Left;

	bool selected = false;

	void MoveToMouseAtSpecifiedDepth(float depth) {
		this.transform.position = MouseUtils.GetMouseWorldPositionAtDepth(depth);
	}

	void Update() {
		if(selected) {
			if(Input.GetMouseButton((int)respondToMouseButton)) {
				//when the mouse button is held, move the object along with it
				//this provides simple click and drag functionality
				MoveToMouseAtSpecifiedDepth(depthIntoScene);
			} else {
				selected = false;
			}
		}
	}
	
	//OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
	public void OnMouseOver() {
		if(Input.GetMouseButtonDown((int)respondToMouseButton)) {
			//Get the vector from the camera to the object
			Vector3 headingToObject = this.transform.position - Camera.main.transform.position;
			//find the projection on the forward vector of the camera
			depthIntoScene = Vector3.Dot(headingToObject, Camera.main.transform.forward);
			selected = true;
		}
	}
}
