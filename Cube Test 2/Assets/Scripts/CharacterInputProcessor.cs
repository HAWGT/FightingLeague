using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterInputProcessor : MonoBehaviour
    {
        private CharacterStateController stateController;

        private Rigidbody myRigidbody;

        [SerializeField]
        private float jumpForce = 50f;

        [SerializeField]
        private float airSpeed = 2f;

        // Use this for initialization
        void Start()
        {
            myRigidbody = GetComponent<Rigidbody>();
            stateController = GetComponent<CharacterStateController>();
        }

        // Update is called once per frame
        void Update()
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            //movimento direita.
            if (moveHorizontal > 0)
            {
                if(stateController.charState == CharacterStateController.CharState.standing){
                    if(stateController.facing == CharacterStateController.FacingSide.P1)
                    {
                        stateController.charState = CharacterStateController.CharState.walkingF;
                    }
                    else
                    {
                        stateController.charState = CharacterStateController.CharState.walkingB;
                    }
                }else if(stateController.charState == CharacterStateController.CharState.airborn)
                {
                    myRigidbody.velocity = new Vector3(airSpeed * moveHorizontal, myRigidbody.velocity.y, 0);
                }
            }else if (moveHorizontal < 0){
                //movimento esquerda
                if (stateController.charState == CharacterStateController.CharState.standing)
                {
                    if (stateController.facing == CharacterStateController.FacingSide.P1)
                    {
                        stateController.charState = CharacterStateController.CharState.walkingB;
                    }
                    else
                    {
                        stateController.charState = CharacterStateController.CharState.walkingF;
                    }
                }
                else if (stateController.charState == CharacterStateController.CharState.airborn)
                {
                    //movimento horizontal aéreo
                    myRigidbody.velocity = new Vector3(airSpeed * moveHorizontal, myRigidbody.velocity.y, 0);
                }

            } else if( moveHorizontal == 0 && stateController.charState != CharacterStateController.CharState.airborn && stateController.charState != CharacterStateController.CharState.crouching)
            {
                //default para standing quando parado no chão
                stateController.charState = CharacterStateController.CharState.standing;
            }
            //Jump and attacks
            if(stateController.charState == CharacterStateController.CharState.standing || stateController.charState == CharacterStateController.CharState.walkingB || stateController.charState == CharacterStateController.CharState.walkingF)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    stateController.charState = CharacterStateController.CharState.attacking;
                    stateController.attackState = CharacterStateController.AttackState.light;
                }
                if (Input.GetButtonDown("Jump"))
                {
                    myRigidbody.AddForce(Vector3.up * jumpForce);
                    stateController.charState = CharacterStateController.CharState.airborn;
                }
            }
        }
    }
}

