using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
	const float locoAnimSmoothTime = 0.1f;
	Player player;
	Animator animator;

	void Start ()
	{
		player = GetComponent<Player>();
		animator = GetComponentInChildren<Animator>();
	}
	
	void Update ()
	{
		float mag = new Vector3(player.velocity.x, 0, player.velocity.z).magnitude;
		float speedPercent = mag / player.speed;
		animator.SetFloat("speedPercent", speedPercent, locoAnimSmoothTime, Time.deltaTime);
	}
}
