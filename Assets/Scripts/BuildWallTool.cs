using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWallTool : MonoBehaviour
{
	public GameObject wall;

	public void DrawWall(Vector3Int start, Vector3Int end)
	{
		if (wall.activeSelf == false)
			wall.SetActive(true);
		Vector3 pos = start + end;
		pos /= 2;
		wall.transform.position = pos;
		Vector3 scale = new Vector3(Mathf.Abs(start.x - end.x), 0, Mathf.Abs(start.z - end.z));
		scale += Vector3.one;
		wall.transform.localScale = scale;
	}
	public void TurnOffWall()
	{
		wall.SetActive(false);
	}
}
