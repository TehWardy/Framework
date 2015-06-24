using UnityEngine;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

public class GUIUtils {

	public static Texture2D MakeTexture(int width, int height, Color col) {
		Color[] pix = new Color[width * height];
		
		for (int i = 0; i < pix.Length; i++) {
			pix[i] = col;
		}
		
		Texture2D result = new Texture2D(width, height);
		
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	public static bool MouseOverRect(Rect rectToTest) {
		return rectToTest.Contains(GetGUIMousePosition());
	}

	//convexPoints should be a list of vertices, in clockwise order without repeating the first vertex
	public static bool ConvexShapeContains(Vector2[] convexPoints, Vector2 pointToTest) {
		//For each edge, test if the point is to the left
		//If it's to the left of all edges, it's inside
		
		for(int p = 0; p < convexPoints.Length; p++) {
			//Set up the line equation for the current edge
			float A = -(convexPoints[(p+1)%convexPoints.Length].y - convexPoints[p].y);
			float B = convexPoints[(p+1)%convexPoints.Length].x - convexPoints[p].x;
			float C = -(A * convexPoints[p].x + B * convexPoints[p].y);
			
			//Do a cross product test to determine which side the point is on
			//If D == 0, point is on the line, if D < 0 point is on the right, if D > 0 point is on the left
			float D = A * pointToTest.x + B * pointToTest.y + C;

			if(D < 0) // the point is to the right of this edge, outside the shape
				return false;
		}


		return true;
	}

	//Tests if the mouse is inside a rectangle that's been rotated
	public static bool MouseOverRotatedRect(Rect rectToTest, Matrix4x4 rotationMatrix) {

		return rectToTest.Contains(GetGUIMousePosition());
	}

	//Get the mouse position in Screen coordinates (flips the Y axis)
	public static Vector2 GetGUIMousePosition() {
		return new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);	
	}


	
	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color lineColor, float lineWidth, Texture2D lineTexture) {
		//Backup the existing matrix and color
		Matrix4x4 matrixBackup = GUI.matrix;
		Color colorBackup = GUI.color;

		GUI.color = lineColor;


		//If the texture is null, create a new one
		if (lineTexture == null) {
			lineTexture = new Texture2D(1, 1);
		}


		Vector2 direction = pointB - pointA;
		
		float angle = Mathf.Rad2Deg * Mathf.Atan(direction.y / direction.x);


		if (direction.x < 0)
			angle += 180;

		//Set up our rotation to make the canvas level between the two points
		GUIUtility.RotateAroundPivot(angle, pointA);


		int halfWidth = (int) Mathf.Ceil(lineWidth / 2);
		//Draw the line
		GUI.DrawTexture(new Rect(pointA.x, pointA.y - halfWidth, direction.magnitude, lineWidth), lineTexture);

		//Restore the backups.
		GUI.matrix = matrixBackup;
		GUI.color = colorBackup;
	}


	public static Vector2 NearestPointOnPerimeter(Rect rect, Vector2 pointToTest) {

		Vector2 nearest = new Vector2();

		if(pointToTest.x < rect.xMax && pointToTest.x > rect.x) {
			nearest.x = pointToTest.x;
		} else if(Mathf.Abs(rect.xMin - pointToTest.x) < Mathf.Abs(rect.xMax - pointToTest.x)) {
			nearest.x = rect.xMin;
		} else {
			nearest.x = rect.xMax;
		}

		if(pointToTest.y < rect.yMax && pointToTest.y > rect.y) {
			nearest.y = pointToTest.y;
		} else if(Mathf.Abs(rect.yMin - pointToTest.y) < Mathf.Abs(rect.yMax - pointToTest.y)) {
			nearest.y = rect.yMin;
		} else {
			nearest.y = rect.yMax;
		}

		return nearest;
	}

	public static Rect PlaceRectangleOnScreen(Rect inputRect, float edgePadding) {
		Rect outputRect = new Rect(inputRect);

		if(outputRect.x + outputRect.width + edgePadding > Screen.width)
			outputRect.x = Screen.width - outputRect.width - edgePadding;
		if(outputRect.x < edgePadding)
			outputRect.x = edgePadding;
		
		if(outputRect.y + outputRect.height + edgePadding > Screen.height)
			outputRect.y = Screen.height - outputRect.height - edgePadding;
		if(outputRect.y < edgePadding)
			outputRect.y = edgePadding;

		return outputRect;
	}

	public static string MakeRandomString(int wordCount)
	{
		StringBuilder builder = new StringBuilder();
		char ch;
		for (int word = 0; word < wordCount; word++)
		{
			int wordSize = UnityEngine.Random.Range(2, 8);
			for(int letter = 0; letter < wordSize; letter++) {
				if(word == 0 && letter == 0) //First letter of the first word, capitalize!
					ch = (char)UnityEngine.Random.Range('A', 'Z');  
				else
					ch = (char)UnityEngine.Random.Range('a', 'z');  
				builder.Append(ch);
			}
			if(word != wordCount - 1)
				builder.Append(' ');
		}
		char[] punctuation = new char[]{'?', '.', '!'};
		builder.Append(punctuation[UnityEngine.Random.Range(0,3)]);
		
		return builder.ToString();
	}

	public static string PrettyPrintVariableName(string varName) {
		string prettyVariableName = Regex.Replace(varName, "(?<=[a-z])([A-Z])", " $1").Trim();
		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(prettyVariableName);
	}
}
