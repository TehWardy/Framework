using System;
using UnityEngine;

public class MouseUtils
{
	//Define the mouse buttons for easier reading
	public enum Button : int { Left = 0, Right = 1, Middle = 2, None = 3 }

	public static Vector3 GetMouseWorldPositionAtDepth(float depth) {
		Vector3 mouseScreenPosition = Input.mousePosition;
		mouseScreenPosition.z = depth;
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
		return new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, mouseWorldPosition.z);
	}
}

