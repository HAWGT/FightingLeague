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


        [SerializeField]
        private List<Enums.AttackState> attackStates;

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

        private float superBar = 0;

        private Enums.Inputs lastInput;

        private Enums.Inputs latestDirection;

        private Enums.FacingSide nextFace;

        private bool airborn;

        private bool lockRotate = false;

        private bool queueRotate = false;


        public Enums.CharState GetCharState()
        {
            return this.charState;
        }

        public Enums.AttackState GetTypeOfAtk()
        {
            if (this.attackStates.Count == 0) return Enums.AttackState.none;
            return this.attackStates[this.attackStates.Count - 1];
        }

        public List<Enums.AttackState> GetAttackState()
        {
            return this.attackStates;
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
            if (!lockRotate)
            {
                this.facing = face;
                GetComponent<AnimationController>().Mirror();
                Vector3 change = myRigidbody.rotation.eulerAngles;
                change.y = -change.y;
                Quaternion newQuart = new Quaternion();
                newQuart.eulerAngles = change;
                myRigidbody.transform.rotation = newQuart;
                queueRotate = false;
            } else
            {
                queueRotate = true;
                nextFace = face;
            }
        }

        public void SetRotLockState(bool state)
        {
            lockRotate = state;
        }

        public bool GetQueueRotate()
        {
            return queueRotate;
        }

        public Enums.FacingSide GetNextFace()
        {
            return nextFace;
        }

        // Use this for initialization
        private void Start()
        {
            charState = Enums.CharState.standing;
			attackStates = new List<Enums.AttackState>();
			animControl = GetComponent<AnimationController>();
            latestDirection = Enums.Inputs.Neutral;
            airborn = false;
            groundCheck = GetComponent<SphereCollider>();
            myRigidbody = GetComponent<Rigidbody>();
            animControl.SetRigidBody(myRigidbody);
            animatorParameters = animControl.GetAllBoolTriggerAnimatorParameters();
            motionStateMachine = GetComponent<FiniteStateMachineState>();
			superBar = 0;


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

        public void AddAttackState(Enums.AttackState state)
        {
			attackStates.Add(state);
		}

		public void TakeDamage(float dmg)
        {
            animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "hitstun" }));
            animControl.Knock(dmg);

            healthPoints -= dmg;
            superBar += dmg / 200;
            if (playerID == 1) ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
            if (playerID == 2) ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);

           // List<AnimatorControllerParameter> parameter = FindAnimatorParameter(new string[] { "hitstun" });

            //animControl.TriggerAnimatorParameters(parameter);
            //animControl.Knock(dmg);


            if (healthPoints <= 0)
            {
                animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "death" }));
                game.GetComponent<GameManager>().GameEnd(playerID);

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
			}
			else if (facing == Enums.FacingSide.P2)
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

			if (yAxis == Enums.NumPad.Up)
			{
				lastInput = Enums.Inputs.Up;
			}

			lastInput = motionStateMachine.PerformTransition(lastInput, attackStates);


			if (attackStates.Count == 0)
			{
				switch (lastInput)
				{
					case Enums.Inputs.Back:
						animControl.WalkBwd();
						break;

					case Enums.Inputs.DownBack:
						animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "crouch" }));
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
			else
			{
				switch (lastInput)
				{
					case Enums.Inputs.Light:
						animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "lightAttack" }));
						break;

					case Enums.Inputs.Medium:
						animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "mediumAttack" }));
						break;

					case Enums.Inputs.Heavy:
						animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "heavyAttack" }));
						break;

					case Enums.Inputs.Special1:
						animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "special1" }));
						break;

					case Enums.Inputs.Special2:
						animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "special2" }));
						break;

					case Enums.Inputs.Super:
						if (superBar > 49)
						{
							superBar = superBar - 50;
							animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "super" }));
						}
						else
						{
							animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "special1" }));
						}
						break;

					case Enums.Inputs.Vanish:
                        if (superBar > 9)
                        {
                            superBar = superBar - 10;
                            animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "vanish" }));
                        }
                        else
                        {
                            animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "midDash" }));
                        }
                        break;

					case Enums.Inputs.GuardBreak:
						animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "guardBreak" }));
						break;

					case Enums.Inputs.Dash:
						animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "midDash" }));
						break;
				}
				animControl.TurnAnimatorParametersOff(FindAnimatorParameter(new string[] { "walkingForward", "walkingBackward", "crouch" }));
			}
			ResetEnumState();
		}

        private void ResetEnumState()
        {
			attackStates = new List<Enums.AttackState>();
            SetCharState(Enums.CharState.standing);

        }

        public void AddSuperBar(float bar)
        {
            if (GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().GetHP() <= 0) return;
            superBar += bar;
            GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().AddPassiveSuperBar(bar);
            if (superBar > 50f) superBar = 50f;
        }

        public void AddPassiveSuperBar(float bar)
        {
            superBar += bar / 2;
            if (superBar > 50f) superBar = 50f;
        }
    }

}
