using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Unit
{
	private void Start()
	{
		BuilderController.instance.builders.Add(this);
	}

	//public override void 
}
