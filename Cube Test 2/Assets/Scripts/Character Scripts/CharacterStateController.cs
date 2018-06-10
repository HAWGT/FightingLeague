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
        private Enums.AttackState attackState;

        [SerializeField]
        private Enums.FacingSide facing;

        [SerializeField]
        private Enums.CharState charState;

        private List<AnimatorControllerParameter> animatorParameters;

        [SerializeField]
        private FiniteStateMachineState motionStateMachine;

        private GameObject game;
        private GameObject ui;
        
        private Rigidbody myRigidbody;

        private float healthPoints = 10000;

        private int superBar = 0;

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

        public void SetFacingSide(Enums.FacingSide face)
        {
            this.facing = face;
            //GetComponent<AnimationController>().Mirror();
            myRigidbody.transform.Rotate(new Vector3(0, 0, 0), 180);
        }

        // Use this for initialization
        private void Start()
        {
            charState = Enums.CharState.standing;
            attackState = Enums.AttackState.none;
            animControl = GetComponent<AnimationController>();
            latestDirection = Enums.Inputs.Neutral;
            airborn = false;
            groundCheck = GetComponent<SphereCollider>();
            myRigidbody = GetComponent<Rigidbody>();
            animControl.SetRigidBody(myRigidbody);
            animatorParameters = animControl.GetAllBoolAnimatorParameters();
            motionStateMachine = GetComponent<FiniteStateMachineState>();


            //update -> ui manager
            game = GameObject.Find("Game Manager");
            ui = GameObject.Find("UI Manager");
            if (playerID == 1)
            {
                ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
                animControl.TurnAnimatorParametersOff(FindAnimatorParameter(new string[] { "mirrorAnimation" }));
            }
            if (playerID == 2)
            {
                ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
                animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "mirrorAnimation" }));
            }
        }

        

        public void SetCharState(Enums.CharState state)
        {
            charState = state;
        }

        public void SetAttackState(Enums.AttackState state)
        {
            attackState = state;
        }

        public void TakeDamage(float dmg)
        {
            motionStateMachine.ResetMachine();


            animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "hitstun" }));
            animControl.Knock(dmg);

            healthPoints -= dmg;
            if (playerID == 1) ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
            if (playerID == 2) ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);

            if (healthPoints <= 0)
            {
                animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "KO" }));
                game.GetComponent<GameManager>().Die(playerID);

            }
        }

        public List<AnimatorControllerParameter> FindAnimatorParameter(String[] names)
        {
            List<AnimatorControllerParameter> list = new List<AnimatorControllerParameter>();
            AnimatorControllerParameter parametro;

            foreach (String name in names)
            {
                parametro = animatorParameters.Find(parameter => parameter.name == name);
                list.Add(parametro);
            }

            return list;
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
                        lastInput = Enums.Inputs.DownFront;

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
                        lastInput = Enums.Inputs.Back;

                    }
                    else if (xAxis == Enums.NumPad.Right)
                    {
                        lastInput = Enums.Inputs.Front;

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
                        lastInput = Enums.Inputs.DownFront;
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
                        lastInput = Enums.Inputs.Front;
                    }
                    else if (xAxis == Enums.NumPad.Right)
                    {
                        lastInput = Enums.Inputs.Back;
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

            lastInput = motionStateMachine.PerformTransition(lastInput);
            //não está a devolver attackstates
            Console.WriteLine(lastInput.ToString());

            if (attackState != Enums.AttackState.none)
            {
                switch(lastInput)
                {
                    case Enums.Inputs.Light:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] {"lightAttack"}));
                        break;

                    case Enums.Inputs.Medium:
                            animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "mediumAttack" }));
                        break;

                    case Enums.Inputs.Heavy:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] {"heavyAttack"}));
                        break;

                    case Enums.Inputs.Special1:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "special1" }));
                        break;

                    case Enums.Inputs.Special2:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "special2" }));
                        break;

                    case Enums.Inputs.Super:
                        if(superBar > 49)
                        {
                            superBar = superBar - 50;
                            animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "super" }));
                        }
                        else
                        {
                            animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "special1" }));
                        }
                        break;
                }
            }
            else
            {
                switch (lastInput)
                {
                    case Enums.Inputs.Back:
                        animControl.WalkBwd();
                        break;

                    case Enums.Inputs.DownBack:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] {"crouch"}));
                        break;

                    case Enums.Inputs.Down:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "crouch" }));
                        break;

                    case Enums.Inputs.DownFront:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "crouch" }));
                        break;

                    case Enums.Inputs.Front:
                        animControl.WalkFwd();
                        break;

                    case Enums.Inputs.Up:
                        animControl.Jump();
                        break;

                    case Enums.Inputs.Neutral:

                        break;
                }
            }
            ResetEnumState();
        }

        private void ResetEnumState()
        {
            SetAttackState(Enums.AttackState.none);
            SetCharState(Enums.CharState.standing);

        }

    }

}
