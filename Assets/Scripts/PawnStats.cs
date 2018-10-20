using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnStats
{
	public float name;

	public int blood = 100;
	public int sweat = 100;
	public int tears = 100;
	public int hunger = 100;
	public int thirst = 100;

	public int strength;
	public int endurance;
	public int dexterity;
	public int charisma;
	public int awareness;
	public int wisdom;

	public int fighting;
	public int building;
	public int crafting;
	public int cooking;
	public int farming;
	public int animals;

	public Dictionary<int, int> inventory = new Dictionary<int, int>();
}
