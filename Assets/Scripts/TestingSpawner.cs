using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpawner : MonoBehaviour
{
	public void SpawnPlayerUnit()
	{
		Debug.Log("SpawnPlayerUnit");
		Instantiate(Resources.Load("PlayerUnit"));
	}
}
