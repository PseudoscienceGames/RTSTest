using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpawner : MonoBehaviour
{
	public void SpawnPlayerUnit()
	{
		Debug.Log("SpawnPlayerUnit");
		GameObject pawn = Instantiate(Resources.Load("PlayerUnit")) as GameObject;
		pawn.GetComponent<Unit>().stats = PawnController.instance.pawns.Count;
		PawnController.instance.AddPawn();
	}
	public void SpawnZombieUnit()
	{
		Debug.Log("SpawnZombieUnit");
		Instantiate(Resources.Load("ZombieUnit"));
	}
}
