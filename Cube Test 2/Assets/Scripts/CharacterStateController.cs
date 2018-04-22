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
        private AnimationController animControl;

        [SerializeField]
        private Enums.AttackState attackState;

        [SerializeField]
        private Enums.FacingSide facing;

        [SerializeField]
        private Enums.CharState charState;

        private GameObject game;
        private GameObject ui;

        private float healthPoints = 10000;

        private float superBar = 0;
        
        private Stopwatch sw;

        private List<Enums.Inputs> inputList;

        private bool canAttack = true;

        public void ToggleAttack()
        {
            this.canAttack = !this.canAttack;
        }

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
            inputList = new List<Enums.Inputs>(20);
            sw = new Stopwatch();
            sw.Start();
            animControl = GetComponent<AnimationController>();

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

        // Update is called once per frame
        private void Update()
        {
            /*if (CheckMotion())
            {

            }
            else
            {*/

            if (attackState == Enums.AttackState.none)
            {
                TranslateInputToState(inputList[inputList.Count - 1]);
            }
            // }
        }

        public void TranslateDirectionalInput(Enums.NumPad xAxis, Enums.NumPad yAxis)
        {
            if (inputList.Count == 20)
            {
                inputList.Clear();
            }

            if (xAxis == Enums.NumPad.Neutral)
            {
                if (yAxis == Enums.NumPad.Up)
                {
                    inputList.Add(Enums.Inputs.Up);
                }

                if (yAxis == Enums.NumPad.Neutral)
                {
                    inputList.Add(Enums.Inputs.Neutral);
                }

                if (yAxis == Enums.NumPad.Down)
                {
                    inputList.Add(Enums.Inputs.Down);
                }

            }
            else if (xAxis == Enums.NumPad.Right)
            {
                if (facing == Enums.FacingSide.P1)
                {
                    if (yAxis == Enums.NumPad.Up)
                    {
                        inputList.Add(Enums.Inputs.Up);
                    }

                    if (yAxis == Enums.NumPad.Neutral)
                    {
                        inputList.Add(Enums.Inputs.Forward);
                    }

                    if (yAxis == Enums.NumPad.Down)
                    {
                        inputList.Add(Enums.Inputs.DownForward);
                    }
                }
                else
                {
                    if (yAxis == Enums.NumPad.Up)
                    {
                        inputList.Add(Enums.Inputs.Up);
                    }

                    if (yAxis == Enums.NumPad.Neutral)
                    {
                        inputList.Add(Enums.Inputs.Backward);
                    }

                    if (yAxis == Enums.NumPad.Down)
                    {
                        inputList.Add(Enums.Inputs.DownBack);
                    }
                }

            }
            else if (xAxis == Enums.NumPad.Left)
            {
                if (facing == Enums.FacingSide.P1)
                {
                    if (yAxis == Enums.NumPad.Up)
                    {
                        inputList.Add(Enums.Inputs.Up);
                    }

                    if (yAxis == Enums.NumPad.Neutral)
                    {
                        inputList.Add(Enums.Inputs.Backward);
                    }

                    if (yAxis == Enums.NumPad.Down)
                    {
                        inputList.Add(Enums.Inputs.DownBack);
                    }
                }
                else
                {
                    if (yAxis == Enums.NumPad.Up)
                    {
                        inputList.Add(Enums.Inputs.Up);
                    }

                    if (yAxis == Enums.NumPad.Neutral)
                    {
                        inputList.Add(Enums.Inputs.Forward);
                    }

                    if (yAxis == Enums.NumPad.Down)
                    {
                        inputList.Add(Enums.Inputs.DownForward);
                    }
                }
            }
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
        

        private bool CheckMotion()
        {
            throw new NotImplementedException();
        }

    }

}
