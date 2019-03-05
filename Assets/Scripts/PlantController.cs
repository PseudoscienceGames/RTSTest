using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
	public string plantType;
	public int spawnCount;
	public GameObject prefab;

	public Dictionary<Vector2Int, int> gridLocs = new Dictionary<Vector2Int, int>();
	public Dictionary<Vector2Int, GameObject> meshes = new Dictionary<Vector2Int, GameObject>();
	public void Awake()
	{
		Island.instance.plantControllers.Add(this);
	}
	public void AddPlants()
	{
		List<Vector2Int> land = new List<Vector2Int>(Island.instance.land);
		for (int j = 0; j < spawnCount; j++)
		{
			Vector2Int toAdd = land[Random.Range(0, land.Count)];
			gridLocs.Add(toAdd, Random.Range(1, 101));
			land.Remove(toAdd);
		}
		DrawPlants();
	}
	public void DrawPlants()
	{
		foreach(Vector2Int loc in gridLocs.Keys)
		{
			Vector3 worldLoc = HexGrid.GridToWorld(loc, Island.instance.GetHeight(loc));
			GameObject currentPlant = Instantiate(prefab, worldLoc, Quaternion.identity, transform) as GameObject;
			currentPlant.transform.localScale = Vector3.one * gridLocs[loc];
			meshes.Add(loc, currentPlant);
		}
	}

	public void Grow()
	{
		foreach(Vector2Int loc in gridLocs.Keys)
		{
			if (gridLocs[loc] < 100)
			{
				gridLocs[loc]++;

			}
			else
			{
				Vector2Int loc2 = HexGrid.MoveTo(loc, Random.Range(0, 6));
				if(!gridLocs.ContainsKey(loc2))
					
				if (gridLocs[loc2] < 100)
					gridLocs[loc2]++;
			}
		}
	}
}