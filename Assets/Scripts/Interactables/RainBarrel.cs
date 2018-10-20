using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RainBarrel : Interactable
{
	public int amt = 0;
	public int cap = 100;

	private UnityAction rain;

	public override void Start()
	{
		base.Start();
		possibleActions.Add("Drink");
		possibleActions.Add("Fill");
		rain = new UnityAction(Fill);
		EventManager.StartListening("Rain", rain);
	}

	private void Fill()
	{
		amt = cap;
	}

	public override void Interact(Unit unit, int index)
	{
		base.Interact(unit, index);
		if(index == 0)
		{
			Drink(unit);
		}
	}
	private void Drink(Unit unit)
	{
		amt--;
		PawnController.instance.pawns[unit.stats].thirst = 100;
	}
}
