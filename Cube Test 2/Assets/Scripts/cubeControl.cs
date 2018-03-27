using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeControl : MonoBehaviour
{

	private Rigidbody myRigidbody;

		

	[SerializeField]
	private Animator animator;

	[SerializeField]
    private float jumpForce = 50f;

	[SerializeField]
	private float airSpeed = 2f;

    // Use this for initialization
    private void Start()
    {
		myRigidbody = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    private void Update()
    {
		float moveHorizontal = Input.GetAxis("Horizontal");
		if(animator.GetBool("airborn") && moveHorizontal != 0.0f){
			myRigidbody.velocity = new Vector3(airSpeed * moveHorizontal, myRigidbody.velocity.y, 0);
		}
		if(moveHorizontal != 0.0f && !(animator.GetBool("airborn"))){
			animator.SetBool ("walking", true);
			myRigidbody.velocity = new Vector3(moveHorizontal, myRigidbody.velocity.y, 0);
		}else{
			animator.SetBool ("walking", false);
		}

		if (Input.GetButtonDown("Jump") && !(animator.GetBool ("airborn")))
		{
			myRigidbody.AddForce(Vector3.up*jumpForce);
			animator.SetBool ("airborn", true);
		}
    }

    private void FixedUpdate()
    {
		if(Input.GetButtonDown ("Fire1")){
			animator.SetBool ("a_press", true);

		}

		if(myRigidbody.position.y <= 0.07){
			animator.SetBool ("airborn", false);
		}
    }
}
