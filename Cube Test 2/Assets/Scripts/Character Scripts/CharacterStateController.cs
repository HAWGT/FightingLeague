using NeuralNetwork;
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

		[SerializeField]
		private GameObject matchManager;

        private List<AnimatorControllerParameter> animatorParameters;

        private GameObject game;
        private GameObject ui;

        private Rigidbody myRigidbody;

        private Vector3 startPosition;

        public void ResetP()
        {
            healthPoints = 10000;
            superBar = 0;
            myRigidbody.transform.position = startPosition;
            UpdateUI(false);
            animControl.ResetAnim();
        }

        private float healthPoints = 10000;

        private int superBar;

        private Enums.Inputs lastInput;

        private Enums.Inputs latestDirection;

        private Enums.FacingSide nextFace;

        private bool airborn;

        private bool lockRotate = false;

        private bool queueRotate = false;

        private Enums.AttackState lastAtk = Enums.AttackState.none;

        public Enums.CharState GetCharState()
        {
            return this.charState;
        }

        public Enums.AttackState GetTypeOfAtk()
        {
            return lastAtk;
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
				Quaternion newQuart = new Quaternion
				{
					eulerAngles = change
				};
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
            startPosition = myRigidbody.transform.position;
            animatorParameters = animControl.GetAllBoolTriggerAnimatorParameters();
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

        public int GetSuperBar()
		{
			return superBar;
		}

		public void ReduceSuperBar(int reduction)
		{
			superBar -= reduction;
		}

		public void UpdateUI(bool changeSides)
		{
			if (playerID == 1)
			{
				ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);

				if (changeSides)
				{
					animControl.TurnAnimatorParametersOff(FindAnimatorParameter(new string[] { "mirrorAnimation" }));
				}
			}
			if (playerID == 2)
			{
				ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
				if (changeSides)
				{
					animControl.TurnAnimatorParametersOn(FindAnimatorParameter(new string[] { "mirrorAnimation" }));
				}
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
            if (StateHelper.GetState(rigidbody) == Enums.AnimState.super) return;
            animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "hitstun" }));
            animControl.Knock(dmg);

            healthPoints -= dmg;
            superBar += (int) dmg / 200;
			if (playerID == 1)
			{
				if (PlayerPrefs.GetInt("Player1") == 2)
				{
					GetComponent<NetworkInterface>().ChangeHP(playerID, (int) healthPoints);
				}
				if(PlayerPrefs.GetInt("Player2") == 2)
				{
					matchManager.GetComponent<MatchManager>().ChangeAIValue(playerID, (int) healthPoints);
				}
				ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
			}
			if (playerID == 2) {
				if (PlayerPrefs.GetInt("Player1") == 2)
				{
					matchManager.GetComponent<MatchManager>().ChangeAIValue(playerID, (int)healthPoints);
				}
				if (PlayerPrefs.GetInt("Player2") == 2)
				{
					GetComponent<NetworkInterface>().ChangeHP(playerID, (int)healthPoints);
				}
				ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
			}

           // List<AnimatorControllerParameter> parameter = FindAnimatorParameter(new string[] { "hitstun" });

            //animControl.TriggerAnimatorParameters(parameter);
            //animControl.Knock(dmg);


            if (healthPoints <= 0)
            {
                animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "death" }));
                game.GetComponent<MatchManager>().MatchEnd(playerID);

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



        public void Vanish()
        {
            if (superBar < 10) return;
            superBar -= 10;
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "vanish" }));
        }

        public void Super()
        {
            if (superBar < 50) return;
            superBar -= 50;
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "super" }));
        }

        public void SetLastAtk(Enums.AttackState atk)
        {
            lastAtk = atk;
        }

        private void ResetEnumState()
        {
			attackStates = new List<Enums.AttackState>();
            SetCharState(Enums.CharState.standing);
            SetLastAtk(Enums.AttackState.none);

        }

        public void AddSuperBar(float bar)
        {
            if (GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().GetHP() <= 0) return;
            superBar += (int) bar;
            if (superBar > 100) superBar = 100;

			if (playerID == 1)
			{
				ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
			}
			if (playerID == 2)
			{
				ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
			}
		}
    public int GetPlayerID()
    {
      return playerID;
    }
  }

}
