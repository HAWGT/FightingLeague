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
        private int playerInputType = 0;

        public void SetPlayerInputType(int a)
        {
            if (a != 0 && a != 1 && a != 2) a = 0;
            playerInputType = a;
            UnFreezeControls();
        }

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

        private AudioSource audioSource;

        [SerializeField]
        private AudioClip hit;

        [SerializeField]
        private AudioClip teleport;

        [SerializeField]
        private AudioClip meter;

        [SerializeField]
        private AudioClip cancelSpecial;

        [SerializeField]
        private GameObject buffPrefab;

        [SerializeField]
        private GameObject debuffPrefab;

        public void ResetP()
        {
            healthPoints = 10000;
            superBar = 0;
            myRigidbody.transform.position = startPosition;
            UpdateUI(false);
            animControl.ResetAnim();
        }

        private float healthPoints = 10000;

        private float superBar;

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

        public void FreezeControls()
        {
            if (playerInputType == 0) GetComponent<CharacterInputProcessor>().enabled = false;
            if (playerInputType == 1) GetComponent<Panda.BehaviourTree>().enabled = false;
            if (playerInputType == 2) GetComponent<NetworkInterface>().enabled = false;
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

        public void UnFreezeControls()
        {
            if (playerInputType == 0) GetComponent<CharacterInputProcessor>().enabled = true;
            if (playerInputType == 1) GetComponent<Panda.BehaviourTree>().enabled = true;
            if (playerInputType == 2) GetComponent<NetworkInterface>().enabled = true;
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
            audioSource = GetComponent<AudioSource>();
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

        public void CancelSpecial()
        {
            if (superBar < 25f) return;
            ReduceSuperBarNoSnd(25f);
            audioSource.volume = 0.6f;
            audioSource.PlayOneShot(cancelSpecial);
            StartCoroutine(ResetVolume());
            animControl.ResetAnim();
            if (GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().TakeDamage(0, false))
            {
                animControl.FuryFire();
                var buff = (GameObject)Instantiate(buffPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                buff.GetComponent<BuffScript>().SetCreator(myRigidbody);
                Destroy(buff, 3f);
            }
        }

        public void FuryBuff(float a)
        {
            healthPoints += a;
            if (healthPoints > 10000) healthPoints = 10000;
            if (playerID == 1)
            {
                if (PlayerPrefs.GetInt("Player1") == 2)
                {
                    GetComponent<NetworkInterface>().ChangeHP(playerID, (int)healthPoints, (int)superBar);
                }
                if (PlayerPrefs.GetInt("Player2") == 2)
                {
                    matchManager.GetComponent<MatchManager>().ChangeAIValue(playerID, (int)healthPoints, (int)superBar);
                }
                ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
            }
            if (playerID == 2)
            {
                if (PlayerPrefs.GetInt("Player1") == 2)
                {
                    matchManager.GetComponent<MatchManager>().ChangeAIValue(playerID, (int)healthPoints, (int)superBar);
                }
                if (PlayerPrefs.GetInt("Player2") == 2)
                {
                    GetComponent<NetworkInterface>().ChangeHP(playerID, (int)healthPoints, (int)superBar);
                }
                ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
            }

        }

        public void TeleportSFX()
        {
            audioSource.PlayOneShot(teleport);
        }

        public float GetSuperBar()
		{
			return superBar;
		}

        public void ReduceSuperBarNoSnd(float reduction)
        {
            reduction = Math.Abs(reduction);
            superBar -= reduction;
            if (playerID == 1)
            {
                ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
            }
            if (playerID == 2)
            {
                ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
            }
        }

		public void ReduceSuperBar(float reduction)
		{
            reduction = Math.Abs(reduction);
			superBar -= reduction;
            audioSource.volume = 0.6f;
            audioSource.PlayOneShot(meter);
            StartCoroutine(ResetVolume());
            if (playerID == 1)
            {
                ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
            }
            if (playerID == 2)
            {
                ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
            }
        }

        IEnumerator ResetVolume()
        {
            yield return new WaitForSeconds(0.625f);
            audioSource.volume = 0.3f;
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

		public bool TakeDamage(float dmg, bool grab)
        {
            dmg = Math.Abs(dmg);
            if (StateHelper.GetState(myRigidbody) == Enums.AnimState.super || StateHelper.GetState(myRigidbody) == Enums.AnimState.reflect)
            {
                animControl.BlockFX();
                return false;
            }

            if (StateHelper.GetState(myRigidbody) == Enums.AnimState.walkingB)
            {
                if (!grab)
                {
                    animControl.BlockFX();
                    return false;
                }
            }

            if (grab)
            {
                if (StateHelper.GetState(myRigidbody) == Enums.AnimState.light || StateHelper.GetState(myRigidbody) == Enums.AnimState.medium || StateHelper.GetState(myRigidbody) == Enums.AnimState.heavy)
                {
                    GetComponent<CharacterColliderController>().TechGrab();
                    animControl.BlockFX();
                    return false;
                }
            }

            if (dmg == 0)
            {
                animControl.Fireworks();
                var debuff = (GameObject)Instantiate(debuffPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                debuff.GetComponent<DebuffScript>().SetReceiver(myRigidbody);
                Destroy(debuff, 3f);
            }

            audioSource.PlayOneShot(hit);
            animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "hitstun" }));
            animControl.Knock(dmg);

            healthPoints -= dmg;
			if (playerID == 1)
			{
				if (PlayerPrefs.GetInt("Player1") == 2)
				{
					GetComponent<NetworkInterface>().ChangeHP(playerID, (int) healthPoints, (int) superBar);
				}
				if(PlayerPrefs.GetInt("Player2") == 2)
				{
					matchManager.GetComponent<MatchManager>().ChangeAIValue(playerID, (int) healthPoints, (int) superBar);
				}
				ui.GetComponent<UIManager>().UpdateP1(healthPoints, superBar);
			}
			if (playerID == 2) {
				if (PlayerPrefs.GetInt("Player1") == 2)
				{
					matchManager.GetComponent<MatchManager>().ChangeAIValue(playerID, (int)healthPoints, (int) superBar);
				}
				if (PlayerPrefs.GetInt("Player2") == 2)
				{
					GetComponent<NetworkInterface>().ChangeHP(playerID, (int)healthPoints, (int) superBar);
				}
				ui.GetComponent<UIManager>().UpdateP2(healthPoints, superBar);
			}

            if (healthPoints <= 0 && game.GetComponent<MatchManager>().IsMatchOver() == false)
            {
                animControl.TriggerAnimatorParameters(FindAnimatorParameter(new string[] { "death" }));
                game.GetComponent<MatchManager>().MatchEnd(playerID);

            }

            return true;
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

        public void ForwardDash()
        {
            if (superBar < 5) return;
            ReduceSuperBar(5);
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "midDash" }));
        }

        public void Vanish()
        {
            if (superBar < 10) return;
            ReduceSuperBar(10);
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "vanish" }));
        }

        public void Super()
        {
            if (superBar < 50) return;
            ReduceSuperBar(50);
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
            bar = Math.Abs(bar);
            if (GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().GetHP() <= 0) return;
            superBar += bar;
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
