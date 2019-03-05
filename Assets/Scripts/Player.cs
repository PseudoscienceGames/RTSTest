using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	public float speed = 10;
	public float smooth = 0.1f;
	Vector3 smoothdamp;
	public Vector3 velocity;
	public float gravity;
	CharacterController pc;
	Vector3 input;

	void Start()
	{
		pc = GetComponent<CharacterController>();
	}
	void Update()
	{
		input = Camera.main.transform.root.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
		if (input.magnitude > 1f)
			input = input.normalized;
		input *= speed;
		if (pc.isGrounded)
			velocity.y = 0;
		velocity.y += gravity * Time.deltaTime;
		input.y = velocity.y;
		transform.LookAt(new Vector3(transform.position.x + velocity.x, transform.position.y, transform.position.z + velocity.z));
		velocity = Vector3.SmoothDamp(velocity, input, ref smoothdamp, smooth);
		pc.Move(velocity * Time.deltaTime);
	}
}