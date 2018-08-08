using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public SelectionState sState;
}

public enum SelectionState { NotSelected, Selected, Highlighted};