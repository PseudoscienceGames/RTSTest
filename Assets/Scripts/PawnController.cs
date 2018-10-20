using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour
{
	public static PawnController instance;
	void Awake(){ instance = this; }

	public List<PawnStats> pawns = new List<PawnStats>();

	public void AddPawn()
	{
		pawns.Add(new PawnStats());
	}

	public void Start()
	{
		InvokeRepeating("Digest", 10, 10);
	}

	public void Digest()
	{
		foreach(PawnStats p in pawns)
		{
			p.hunger--;
			p.thirst--;
		}
	}
}
