using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Installable : MonoBehaviour
{
	public bool blueprint = true;
	public MeshRenderer mesh;
	public Vector2Int gridLoc;
	public int workDone;
	public int workRequired;
	public Material mat;

	public void Build(int amt)
	{
		if (blueprint)
		{
			workDone += amt;
			if (workDone >= workRequired)
				Finish();
		}
		else
			Debug.LogError("Why are we building if " + gameObject.name + " is already built?");
	}

	public void Finish()
	{
		blueprint = false;
		GetComponent<NavMeshObstacle>().enabled = true;
		mesh.material = mat;
	}
}
