using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterStateController : MonoBehaviour
    {
        [SerializeField]
        private int playerID;

        [SerializeField]
        private SphereCollider groundCheck;

        [SerializeField]
        private AnimationController animControl;
        /// <summary>
        /// fazer disable a root motion ao saltar
        /// fazer enable ao aterrar no ontriggerenter no colider dos pés
        /// </summary>

        [SerializeField]
        private Animator motionControl;

        [SerializeField]
        private Enums.AttackState attackState;

        [SerializeField]
        private Enums.FacingSide facing;

        [SerializeField]
        private Enums.CharState charState;

        private GameObject game;
        private GameObject ui;

        private Rigidbody myRigidbody;

        private float healthPoints = 10000;

        private float superBar = 0;

        private Enums.Inputs lastInput;

        private Enums.Inputs latestDirection;

        private bool airborn;

        public Enums.CharState GetCharState()
        {
            return this.charState;
        }

        public Enums.AttackState GetAttackState()
        {
            return this.attackState;
        }

        public float GetHP()
        {
            return this.healthPoints;
        }

        public float GetSB()
        {
            return this.superBar;
        }

        public Enums.FacingSide GetFacingSide()
        {
            return this.facing;
        }

        // Use this for initialization
        private void Start()
        {
            charState = Enums.CharState.standing;
            attackState = Enums.AttackState.none;
            animControl = GetComponent<AnimationController>();
            motionControl = GetComponent<Animator>();
            latestDirection = Enums.Inputs.Neutral;
            airborn = false;
            groundCheck = GetComponent<SphereCollider>();
            myRigidbody = GetComponent<Rigidbody>();

            //update -> ui manager
            game = GameObject.Find("Game Manager");
            ui = GameObject.Find("UI Manager");
            if (playerID == 1) ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
            if (playerID == 2) ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
        }

        public void SetState(Enums.CharState state)
        {
            charState = state;
        }

        public void SetAttackState(Enums.AttackState state)
        {
            attackState = state;
        }

        public void TakeDamage(float dmg)
        {
            healthPoints -= dmg;
            if (playerID == 1) ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
            if (playerID == 2) ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);

            animControl.Hitstun();

            if (healthPoints <= 0)
            {
                game.GetComponent<GameManager>().Die(playerID);
            }
        }

        public void TranslateDirectionalInput(Enums.NumPad xAxis, Enums.NumPad yAxis)
        {

            if (facing == Enums.FacingSide.P1)
            {
                if (yAxis == Enums.NumPad.Down)
                {
                    if (xAxis == Enums.NumPad.Left)
                    {
                        lastInput = Enums.Inputs.DownBack;

                    }
                    else if (xAxis == Enums.NumPad.Right)
                    {
                        lastInput = Enums.Inputs.DownForward;

                    }
                    else if (xAxis == Enums.NumPad.Neutral)
                    {
                        lastInput = Enums.Inputs.Down;
                    }
                }
                else if (yAxis == Enums.NumPad.Neutral)
                {

                    if (xAxis == Enums.NumPad.Left)
                    {
                        lastInput = Enums.Inputs.Backward;

                    }
                    else if (xAxis == Enums.NumPad.Right)
                    {
                        lastInput = Enums.Inputs.Forward;

                    }
                    else if (xAxis == Enums.NumPad.Neutral)
                    {
                        lastInput = Enums.Inputs.Neutral;
                    }
                }
            }else if (facing == Enums.FacingSide.P2)
            {
                if (yAxis == Enums.NumPad.Down)
                {
                    if (xAxis == Enums.NumPad.Left)
                    {
                        lastInput = Enums.Inputs.DownForward;
                    }
                    else if (xAxis == Enums.NumPad.Right)
                    {
                        lastInput = Enums.Inputs.DownBack;
                    }
                    else if (xAxis == Enums.NumPad.Neutral)
                    {
                        lastInput = Enums.Inputs.Down;
                    }
                }
                else if (yAxis == Enums.NumPad.Neutral)
                {
                    if (xAxis == Enums.NumPad.Left)
                    {
                        lastInput = Enums.Inputs.Forward;
                    }
                    else if (xAxis == Enums.NumPad.Right)
                    {
                        lastInput = Enums.Inputs.Backward;
                    }
                    else if (xAxis == Enums.NumPad.Neutral)
                    {
                        lastInput = Enums.Inputs.Neutral;
                    }
                }
            }

            if(yAxis == Enums.NumPad.Up)
            {
                lastInput = Enums.Inputs.Up;
            }

            SetInputBool(lastInput);

            if (attackState != Enums.AttackState.none)
            {
                //TODO
                //diferenciar Special de Super
                //reset states to neutral
            }
            else
            {
                switch (lastInput)
                {
                    case Enums.Inputs.Backward:
                        animControl.WalkBwd();
                        break;

                    case Enums.Inputs.DownBack:
                        animControl.Crouch();
                        break;

                    case Enums.Inputs.Down:
                        animControl.Crouch();
                        break;

                    case Enums.Inputs.DownForward:
                        animControl.Crouch();
                        break;

                    case Enums.Inputs.Forward:
                        animControl.WalkFwd();
                        break;

                    case Enums.Inputs.Up:
                        animControl.Jump();
                        //TODO
                        //Verificação para aterrar
                        //>escrever no gameObject groundCheck
                        //saber se faço disable à root motion para adicionar força ao salto
                        break;

                    case Enums.Inputs.Neutral:
                        SetAllInputBoolFalse();
                        break;
                }
            }
        }

        private void SetInputBool(Enums.Inputs input)
        {
            if (input != latestDirection)
            {

                switch (input)
                {
                    case Enums.Inputs.Backward:
                        motionControl.SetBool("4", true);
                        break;

                    case Enums.Inputs.DownBack:
                        motionControl.SetBool("1", true);
                        break;

                    case Enums.Inputs.Down:
                        motionControl.SetBool("2", true);
                        break;

                    case Enums.Inputs.DownForward:
                        motionControl.SetBool("3", true);
                        break;

                    case Enums.Inputs.Forward:
                        motionControl.SetBool("6", true);
                        break;

                    case Enums.Inputs.Neutral:
                        SetAllInputBoolFalse();
                        break;
                }
            }
        }

        private void SetAllInputBoolFalse()
        {
            motionControl.SetBool("4", false);
            motionControl.SetBool("1", false);
            motionControl.SetBool("2", false);
            motionControl.SetBool("3", false);
            motionControl.SetBool("6", false);
            motionControl.SetBool("5", true);

            StopAllAnimations();
        }

        private void StopAllAnimations()
        {
            animControl.StopBlock();
            animControl.StopCrouch();
            animControl.StopCrouchBlock();
            animControl.StopHeavyAtk();
            animControl.StopHitstun();
            animControl.StopLightAtk();
            animControl.StopMediumAtk();
            animControl.StopSpecial1();
            animControl.StopSpecial2();
            animControl.StopSuper();
            animControl.StopWalkBwd();
            animControl.StopWalkFwd();
        }

        private void TranslateInputToState(Enums.Inputs input)
        {
            switch (input)
            {
                case Enums.Inputs.Backward:
                    animControl.WalkBwd();
                    break;

                case Enums.Inputs.Down:
                    animControl.Crouch();
                    break;

                case Enums.Inputs.DownBack:
                    animControl.CrouchBlock();
                    break;

                case Enums.Inputs.DownForward:
                    animControl.Crouch();
                    break;

                case Enums.Inputs.Forward:
                    animControl.WalkFwd();
                    break;

                case Enums.Inputs.Up:
                    animControl.Jump();
                    break;
            }
        }


        

    }

}
