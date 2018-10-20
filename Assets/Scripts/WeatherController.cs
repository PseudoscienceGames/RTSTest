using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
	public void Rain()
	{
		EventManager.TriggerEvent("Rain");
	}
}
