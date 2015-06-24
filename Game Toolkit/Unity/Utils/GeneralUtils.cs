using System;
using UnityEngine;

public class GeneralUtils {

	public static Vector3 NearestPointOnBounds(Bounds bounds, Vector3 pointToTest) {
		Vector3 nearest = new Vector3();
		Vector3 max = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z);
		Vector3 min = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z);

		
		if(pointToTest.x < max.x && pointToTest.x > min.x) {
			nearest.x = pointToTest.x;
		} else if(Mathf.Abs(min.x - pointToTest.x) < Mathf.Abs(max.x - pointToTest.x)) {
			nearest.x = min.x;
		} else {
			nearest.x = max.x;
		}
		
		if(pointToTest.y < max.y && pointToTest.y > min.y) {
			nearest.y = pointToTest.y;
		} else if(Mathf.Abs(min.y - pointToTest.y) < Mathf.Abs(max.y - pointToTest.y)) {
			nearest.y = min.y;
		} else {
			nearest.y = max.y;
		}

		if(pointToTest.z < max.z && pointToTest.z > min.z) {
			nearest.z = pointToTest.z;
		} else if(Mathf.Abs(min.z - pointToTest.z) < Mathf.Abs(max.z - pointToTest.z)) {
			nearest.z = min.z;
		} else {
			nearest.z = max.z;
		}
		
		return nearest;
	}

	public static double DistanceToBoundsEdge(Bounds bounds, Vector3 pointToTest) {
		return DistanceToBoundsEdge(bounds, pointToTest.x, pointToTest.y, pointToTest.z);
	}

	public static double DistanceToBoundsEdge(Bounds bounds, float x, float y, float z) {
		Vector3 nearest = new Vector3();
		Vector3 max = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z);
		Vector3 min = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z);
		
		
		if(x < max.x && x > min.x) {
			nearest.x = x;
		} else if(Mathf.Abs(min.x - x) < Mathf.Abs(max.x - x)) {
			nearest.x = min.x;
		} else {
			nearest.x = max.x;
		}
		
		if(y < max.y && y > min.y) {
			nearest.y = y;
		} else if(Mathf.Abs(min.y - y) < Mathf.Abs(max.y - y)) {
			nearest.y = min.y;
		} else {
			nearest.y = max.y;
		}
		
		if(z < max.z && z > min.z) {
			nearest.z = z;
		} else if(Mathf.Abs(min.z - z) < Mathf.Abs(max.z - z)) {
			nearest.z = min.z;
		} else {
			nearest.z = max.z;
		}
		
		return Mathf.Sqrt(
			Mathf.Pow(nearest.x - x, 2.0f) +
			Mathf.Pow(nearest.y - y, 2.0f) +
			Mathf.Pow(nearest.z - z, 2.0f));
	}

	public static double DistanceToRectEdge (Rect rect, Vector2 pointToTest) {
		return DistanceToRectEdge(rect, pointToTest.x, pointToTest.y);
	}

	public static double DistanceToRectEdge (Rect rect, float x, float y)
	{
		Vector2 nearest = new Vector2();
		
		if(x < rect.xMax && x > rect.x) {
			nearest.x = x;
		} else if(Mathf.Abs(rect.xMin - x) < Mathf.Abs(rect.xMax - x)) {
			nearest.x = rect.xMin;
		} else {
			nearest.x = rect.xMax;
		}
		
		if(y < rect.yMax && y > rect.y) {
			nearest.y = y;
		} else if(Mathf.Abs(rect.yMin - y) < Mathf.Abs(rect.yMax - y)) {
			nearest.y = rect.yMin;
		} else {
			nearest.y = rect.yMax;
		}
		
		return Mathf.Sqrt(
			Mathf.Pow(nearest.x - x, 2.0f) +
			Mathf.Pow(nearest.y - y, 2.0f));
	}
}

