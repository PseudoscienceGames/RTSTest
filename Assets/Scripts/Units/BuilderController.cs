using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
	public static BuilderController instance;
	private void Awake(){ instance = this; }

	public List<Builder> builders = new List<Builder>();
	public List<Builder> idleBuilders = new List<Builder>();
	public List<Installable> blueprints = new List<Installable>();

	private void Start()
	{
		idleBuilders = builders;
	}


}
