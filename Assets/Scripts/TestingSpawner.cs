using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpawner : MonoBehaviour
{
	public void SpawnPlayerUnit()
	{
		Debug.Log("SpawnPlayerUnit");
		GameObject pawn = Instantiate(Resources.Load("PlayerUnit")) as GameObject;
	}
	public void SpawnZombieUnit()
	{
		Debug.Log("SpawnZombieUnit");
		Instantiate(Resources.Load("ZombieUnit"));
	}
}
