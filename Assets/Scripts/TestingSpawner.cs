using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpawner : MonoBehaviour
{
	public void SpawnPlayerUnit()
	{
		GameObject pawn = Instantiate(Resources.Load("PlayerUnit")) as GameObject;
	}
	public void SpawnBuilder()
	{
		Instantiate(Resources.Load("Builder"));
	}
}
