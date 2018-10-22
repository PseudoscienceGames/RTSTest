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

	private void Start()
	{
		BuilderController.instance.blueprints.Add(this);
	}

	public bool Build(int amt)
	{
		if (blueprint)
		{
			workDone += amt;
			if (workDone >= workRequired)
				Finish();
		}
		else
			Debug.LogError("Why are we building if " + gameObject.name + " is already built?");
		return !blueprint;
	}

	public void Finish()
	{
		blueprint = false;
		GetComponent<NavMeshObstacle>().enabled = true;
		mesh.material = mat;
	}
}
