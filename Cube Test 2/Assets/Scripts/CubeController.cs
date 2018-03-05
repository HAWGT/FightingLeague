using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

	private CharacterController controller;

	private float moveSpeed = 5.0f;
	private float verticalVelocity;
	private float gravity = 14.0f;
	private float jumpForce = 10.0f;



	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (controller.isGrounded) {
			verticalVelocity = -gravity * Time.deltaTime;
			if (Input.GetButtonDown ("Jump")) {
				verticalVelocity = jumpForce;
			}
		} else {
			verticalVelocity -= gravity * Time.deltaTime;
		}

		Vector3 moveVector = Vector3.zero;
		moveVector.x = Input.GetAxis ("Horizontal")*moveSpeed;
		moveVector.y = verticalVelocity;
		moveVector.z = 0f;
		controller.Move (moveVector * Time.deltaTime);
		}
}