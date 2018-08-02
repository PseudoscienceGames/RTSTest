using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	static Texture2D white;
	public static Texture2D White
	{
		get
		{
			if (white == null)
			{
				white = new Texture2D(1, 1);
				white.SetPixel(0, 0, Color.white);
				white.Apply();
			}
			return white;
		}
	}

	public static void DrawScreenRect(Rect rect, Color color)
	{
		GUI.color = color;
		GUI.DrawTexture(rect, White);
		GUI.color = Color.white;
	}

	public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
	{
		// Top
		Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
		// Left
		Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
		// Right
		Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
		// Bottom
		Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
	}

	public static Rect GetScreenRect(Vector2 screenPos1, Vector2 screenPos2)
	{
		screenPos1.y = Screen.height - screenPos1.y;
		screenPos2.y = Screen.height - screenPos2.y;

		Vector2 topLeft = Vector2.Min(screenPos1, screenPos2);
		Vector2 bottomRight = Vector2.Max(screenPos1, screenPos2);

		return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
	}

	public static Bounds GetViewportBounds(Camera cam, Vector2 screenPos1, Vector2 screenPos2)
	{
		Vector2 v1 = Camera.main.ScreenToViewportPoint(screenPos1);
		Vector2 v2 = Camera.main.ScreenToViewportPoint(screenPos2);

		Vector3 min = Vector2.Min(v1, v2);
		Vector3 max = Vector2.Max(v1, v2);
		min.z = cam.nearClipPlane;
		max.z = cam.farClipPlane;

		Bounds bounds = new Bounds();
		bounds.SetMinMax(min, max);
		return bounds;
	}
}
