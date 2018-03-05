using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeControl : MonoBehaviour
{

    [SerializeField]
    private Rigidbody myRigidbody;

    [SerializeField]
    private bool isJumping = false;
    /*public bool isFalling = false;
    public float initialGround;// = transform.position.y;
    public float maxHeight;// = initialGround + 1;*/

    private bool jump = false;
    private bool left = false;
    private bool right = false;

    private float jumpForce = 500f;
    private float speed = 8f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

        jump = false;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        //transform.Translate(0.0f, 0.0f, 0.0f);
        /*  if (Input.GetKey("up")) transform.Translate(0, 0, 0.25f);
          if (Input.GetKey("down")) transform.Translate(0, 0, -0.25f);
          if (Input.GetKey("left")) transform.Translate(-0.25f, 0, 0);
          if (Input.GetKey("right")) transform.Translate(0.25f, 0, 0);
          if (Input.GetKey("space") && this.isJumping == false && this.isFalling == false) this.isJumping = true;

          if(this.isJumping)
          {
              transform.Translate(0, 0.5f, 0);
          }

          if(transform.position.y == this.maxHeight)
          {
              this.isJumping = false;
              this.isFalling = true;
          }

          if (this.isFalling)
          {
              transform.Translate(0, -0.5f, 0);
          }

          if (transform.position.y == this.initialGround)
          {
              this.isJumping = false;
              this.isFalling = false;
          }*/

    }

    private void FixedUpdate()
    {
        if (jump)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce);
            jump = false;
        }
        float moveHorizontal = Input.GetAxis("Horizontal");
        myRigidbody.velocity = new Vector3(speed * moveHorizontal, 0, 0);
    }
}
