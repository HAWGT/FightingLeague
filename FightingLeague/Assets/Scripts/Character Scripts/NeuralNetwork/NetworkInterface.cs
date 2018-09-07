using System;
using System.Collections.Generic;
using CharacterControl;
using UnityEngine;

namespace NeuralNetwork{

	public class NetworkInterface : MonoBehaviour
	{
		//general interface
		// self -> x,y, life, super, airborn, busy(bool), attack(bool), tempo
		private int numEntradas = 13;
		private int numSaidas = 1;
		
		private AnimationController animationController;
		private AnimatorParameters animatorParameters;
		private CharacterStateController state;
		private FF2Layer rede;
		private NeuronExamples situationExamples;

		private List<Neuron> examples;

		private bool trainingMode;

		private Neuron neuron;
		private Neuron exampleSent;

		//dados a enviar para a rede
		private int selfHP = 10000;
		private int selfBusy;
		private int selfSuper = 0;
		private double selfHeight;

		private int enemyHP = 10000;
		private int enemySuper = 0;
		private int enemyBusy;
		private double enemyHeight;

		private double distanceX;

		private int recomendedAction;

		private int acaoRealizada;

		// Use this for initialization
		void Start()
		{
			state = GetComponent<CharacterStateController>();
			rede = new FF2Layer(numEntradas, 14, numSaidas, 1);

			

			if (PlayerPrefs.GetInt("TrainingMode") == 1)
			{
				trainingMode = true;
			}
			else
			{
				trainingMode = false;
			}

			animationController = GetComponent<AnimationController>();
			animatorParameters = new AnimatorParameters(animationController.GetAllBoolTriggerAnimatorParameters());
			neuron = new Neuron();
		}

		// Update is called once per frame
		void Update()
		{
			if(StateHelper.GetState(GetComponent<Rigidbody>()) == Enums.AnimState.standing || StateHelper.GetState(GetComponent<Rigidbody>()) == Enums.AnimState.airborn ||
				StateHelper.GetState(GetComponent<Rigidbody>()) == Enums.AnimState.walkingF || StateHelper.GetState(GetComponent<Rigidbody>()) == Enums.AnimState.walkingB)
			{

				selfBusy = RevertAction(StateHelper.GetState(GetComponent<Rigidbody>()));
				enemyBusy = RevertAction(StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()));

				distanceX = System.Math.Abs(GetComponent<Rigidbody>().position.x - GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.x);
				selfHeight = GetComponent<Rigidbody>().position.y;
				enemyHeight = GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.y;

				recomendedAction = OptimalResult(selfHP, enemyHP, selfSuper, enemySuper, distanceX, selfHeight, enemyHeight, selfBusy, enemyBusy);

				neuron = new Neuron(new double[] { selfHP, enemyHP, selfSuper, enemySuper, distanceX, selfHeight, enemyHeight, selfBusy, enemyBusy }, recomendedAction);
				//Construtor Neuron(new double[] { selfHP, enemyHP, selfBar, enemyBar, distanceX, selfHeight, enemyHeight, meBusy, themBusy, themAttack}, answer);

				exampleSent = situationExamples.SearchExample(selfHP - enemyHP, selfSuper, distanceX, Math.Abs(selfHeight - enemyHeight), enemyBusy);

				if (trainingMode)
				{
					acaoRealizada = rede.TreinoRede(neuron, exampleSent, 0.1, 0.01);
				}
				else
				{
					acaoRealizada = rede.CalculaResultadoRede(neuron.GetInstancia());
				}
			}
			Act(acaoRealizada);
			
		}

		private int RevertAction(Enums.AnimState action)
		{
			switch (action)
			{
				case Enums.AnimState.walkingF:
					return 1;
				case Enums.AnimState.walkingB:
					return 2;
				case Enums.AnimState.airborn:
					return 3;
				case Enums.AnimState.light:
					return 4;
				case Enums.AnimState.medium:
					return 5;
				case Enums.AnimState.heavy:
					return 6;
				case Enums.AnimState.special1:
					return 7;
				case Enums.AnimState.special2:
					return 8;
				case Enums.AnimState.super:
					return 9;
				case Enums.AnimState.grab:
					return 10;
				case Enums.AnimState.dash:
					return 11;
				case Enums.AnimState.reflect:
					return 12;
				case Enums.AnimState.vanish:
					return 13;
				default:
					return 0;
			}
		}

		private void Act(int action)
		{
			switch (action)
			{
				case 0:
					break;

				case 1:
					animationController.WalkFwd();
					break;
				case 2:
					animationController.WalkBwd();
					break;
				case 3:
					animationController.Jump();
					break;
				case 4:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] {"lightAttack"}));
					break;
				case 5:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "mediumAttack" }));
					break;
				case 6:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "heavyAttack" }));
					break;
				case 7:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "special1" }));
					break;
				case 8:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "special2" }));
					break;
				case 9:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "super" }));
					break;
				case 10:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "guardBreak" }));
					break;
				case 11:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "midDash" }));
					break;
				case 12:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "reflect" }));
					break;
				case 13:
					animationController.TriggerAnimatorParameters(animatorParameters.FindAnimatorParameter(new string[] { "vanish" }));
					break;

				default:
					break;
			}
		}

		public void ChangeHPSuper(int characterID, int newHP, int newSuper)
		{
			if(characterID == state.GetPlayerID())
			{
				selfHP = newHP;
				selfSuper = newSuper;
			}
			else
			{
				enemyHP = newHP;
				enemySuper = newSuper;
			}
			
		}

		private int OptimalResult(int selfHP, int enemyHP, int selfBar, int enemyBar, double distanceX, double selfHeight, double enemyHeight, int selfBusy, int enemyBusy)
		{

			if (selfBusy != 0 || selfBusy != 3)
			{
				return 0;
			}
			else if (enemyBusy != 0 && distanceX < 2 || enemyBusy != 3 && distanceX < 2)
			{
				return 2;
			}
			else if (enemyHeight > 1)
			{
				return 2;
			}
			else
			{
				//diferencial vida, timer grande
				if (selfHP > enemyHP)
				{
					//defensivo
					return 2;
				}
				else
				{
					//ofensivo
					if (distanceX < 1.5)
					{
						if (selfSuper < 50)
						{
							return 5;
						}
						else
						{
							return 9;
						}
					}
					
				}
			}
			return 0;
		}

		public FF2Layer GetRede()
		{
			return rede;
		}

		public void SaveLearningSession()
		{
			rede.SaveWeights();
		}
	}

}