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
                if(stateController.GetCharState() == CharacterStateController.CharState.standing){
                    if(stateController.GetFacingSide() == CharacterStateController.FacingSide.P1)
                    {
                        stateController.SetState(CharacterStateController.CharState.walkingF);
                    }
                    else
                    {
                        stateController.SetState(CharacterStateController.CharState.walkingB);
                    }
                }else if(stateController.GetCharState() == CharacterStateController.CharState.airborn)
                {
                    myRigidbody.velocity = new Vector3(airSpeed * moveHorizontal, myRigidbody.velocity.y, 0);
                }
            }else if (moveHorizontal < 0){
                //movimento esquerda
                if (stateController.GetCharState() == CharacterStateController.CharState.standing)
                {
                    if (stateController.GetFacingSide() == CharacterStateController.FacingSide.P1)
                    {
                        stateController.SetState(CharacterStateController.CharState.walkingB);
                    }
                    else
                    {
                        stateController.SetState(CharacterStateController.CharState.walkingF);
                    }
                }
                else if (stateController.GetCharState() == CharacterStateController.CharState.airborn)
                {
                    //movimento horizontal aéreo
                    myRigidbody.velocity = new Vector3(airSpeed * moveHorizontal, myRigidbody.velocity.y, 0);
                }

            } else if( moveHorizontal == 0 && stateController.GetCharState() != CharacterStateController.CharState.airborn && stateController.GetCharState() != CharacterStateController.CharState.crouching)
            {
                //default para standing quando parado no chão
                stateController.SetState(CharacterStateController.CharState.standing);
            }
            //Jump and attacks
            if(stateController.GetCharState() == CharacterStateController.CharState.standing || stateController.GetCharState() == CharacterStateController.CharState.walkingB || stateController.GetCharState() == CharacterStateController.CharState.walkingF)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    stateController.SetState(CharacterStateController.CharState.attacking);
                    stateController.SetAttackState(CharacterStateController.AttackState.light);
                }
                if (Input.GetButtonDown("Jump"))
                {
                    myRigidbody.AddForce(Vector3.up * jumpForce);
                    stateController.SetState(CharacterStateController.CharState.airborn);
                }
            }
        }
    }
}

