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

        private GameObject game;
        private GameObject ui;

        private Rigidbody myRigidbody;

        private float healthPoints = 10000;

        private float superBar = 0;

        private Enums.Inputs lastInput;

        private Enums.Inputs latestDirection;

        private bool airborn;

        private List<AnimatorControllerParameter> animatorParameters;

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
            latestDirection = Enums.Inputs.Neutral;
            airborn = false;
            groundCheck = GetComponent<SphereCollider>();
            myRigidbody = GetComponent<Rigidbody>();
            animControl.SetRigidBody(myRigidbody);
            animatorParameters = animControl.GetAllBoolAnimatorParameters();


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

            List<AnimatorControllerParameter> parameter = FindAnimatorParameter(new string[] { "hitstun" });

            animControl.TurnAnimatorParametersOn(parameter);

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
                switch(attackState)
                {
                    case Enums.AttackState.light:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] {"lightAttack"}));
                        break;
                    case Enums.AttackState.medium:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] {"mediumAttack"}));
                        break;
                    case Enums.AttackState.heavy:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] {"heavyAttack"}));
                        break;

                }
            }
            else
            {
                switch (lastInput)
                {
                    case Enums.Inputs.Backward:
                        animControl.WalkBwd();
                        break;

                    case Enums.Inputs.DownBack:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] {"crouch"}));
                        break;

                    case Enums.Inputs.Down:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "crouch" }));
                        break;

                    case Enums.Inputs.DownForward:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "crouch" }));
                        break;

                    case Enums.Inputs.Forward:
                        animControl.WalkFwd();
                        break;

                    case Enums.Inputs.Up:
                        animControl.Jump();
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
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "4" }));
                        break;

                    case Enums.Inputs.DownBack:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "4", "2" }));
                        break;

                    case Enums.Inputs.Down:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "2" }));
                        break;

                    case Enums.Inputs.DownForward:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "2", "6" }));
                        break;

                    case Enums.Inputs.Forward:
                        animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "6" }));
                        break;

                    case Enums.Inputs.Neutral:
                        SetAllInputBoolFalse();
                        break;
                }
            }
        }

        private void SetAllInputBoolFalse()
        {
            animControl.TurnAnimatorParametersOff(FindAnimatorParameter(new string[] { "2", "4", "6" }));

            animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "5" }));
        }

        private void TranslateInputToState(Enums.Inputs input)
        {
            switch (input)
            {
                case Enums.Inputs.Backward:
                    animControl.WalkBwd();
                    break;

                case Enums.Inputs.Down:
                    animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "crouch" }));
                    break;

                case Enums.Inputs.DownBack:
                    animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "crouch" })); break;

                case Enums.Inputs.DownForward:
                    animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "crouch" })); break;

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
