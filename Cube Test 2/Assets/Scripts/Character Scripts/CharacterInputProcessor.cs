using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterInputProcessor : MonoBehaviour
    {
        private CharacterStateController stateController;

        private Rigidbody myRigidbody;

        private float horizontalInput;

        private float verticalInput;

        /*[SerializeField]
        private float jumpForce = 50f;

        [SerializeField]
        private float airSpeed = 2f;*/

        // Use this for initialization
        void Start()
        {
            myRigidbody = GetComponent<Rigidbody>();
            stateController = GetComponent<CharacterStateController>();
        }

        // Update is called once per frame
        void Update()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            switch ((int)horizontalInput)
            {
                case 1:
                    if (verticalInput > 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Right, Enums.NumPad.Up);
                    }
                    else if (verticalInput == 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Right, Enums.NumPad.Neutral);
                    }
                    else if (verticalInput < 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Right, Enums.NumPad.Down);
                    }
                    break;

                case 0:
                    if (verticalInput > 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Neutral, Enums.NumPad.Up);
                    }
                    else if (verticalInput == 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Neutral, Enums.NumPad.Neutral);
                    }
                    else if (verticalInput < 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Neutral, Enums.NumPad.Down);
                    }
                    break;

                case -1:
                    if (verticalInput > 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Left, Enums.NumPad.Up);
                    }
                    else if (verticalInput == 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Left, Enums.NumPad.Neutral);
                    }
                    else if (verticalInput < 0)
                    {
                        stateController.TranslateDirectionalInput(Enums.NumPad.Left, Enums.NumPad.Down);
                    }
                    break;

            }

            //Attacks
            if (Input.GetButtonDown("Fire1"))
            {
                stateController.SetCharState(Enums.CharState.attacking);
                stateController.AddAttackState(Enums.AttackState.light);
            }
            if (Input.GetButtonDown("Fire2"))
            {
                stateController.SetCharState(Enums.CharState.attacking);
                stateController.AddAttackState(Enums.AttackState.medium);
            }
            if (Input.GetButtonDown("Fire3"))
            {
                stateController.SetCharState(Enums.CharState.attacking);
                stateController.AddAttackState(Enums.AttackState.heavy);
            }
        }
    }
}

