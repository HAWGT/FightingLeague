using NeuralNetwork;
using System.Collections;
using System.Collections.Generic;
using CharacterControl;
using UnityEngine;
using System;

namespace NeuralNetwork{

	public class NetworkInterface : MonoBehaviour
	{
		//general interface
		// self -> x,y, life, super, airborn, busy(bool), attack(bool), tempo
		private int numEntradas = 13;
		private int numSaidas = 1;
		private int mode;
		private AnimationController animationController;
		private AnimatorParameters animatorParameters;
		private CharacterStateController state;
		private FF2Layer rede;

		private List<Neuron> neurons;

		//dados a enviar para a rede
		private int selfHP = 10000;
		private bool selfBusy = false;
		private int selfSuper = 0;

		private int enemyHP = 10000;
		private int enemySuper = 0;
		private bool enemyBusy = false;
		private bool enemyAttacking = false;

		// Use this for initialization
		void Start()
		{
			state = GetComponent<CharacterStateController>();
			rede = new FF2Layer(numEntradas, 5, numSaidas, 1);
			mode = PlayerPrefs.GetInt("TrainNeurons");
			animationController = GetComponent<AnimationController>();
			animatorParameters = new AnimatorParameters(animationController.GetAllBoolTriggerAnimatorParameters());
			neurons = new List<Neuron>();
		}

		// Update is called once per frame
		void Update()
		{
			if (mode == 1)
			{
				
			}
			else
			{

			}
		}

		private void Act(int action)
		{
			switch (action)
			{
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

		public void ChangeHP(int characterID, int newHP, int newSuper)
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

		private int OptimalResult(int selfHP, int enemyHP, int selfBar, int enemyBar, float distanceX, int selfHeight, int enemyHeight, bool selfBusy, bool enemyBusy, bool enemyIsAttacking, int time)
		{

			if (selfBusy)
			{
				return 0;
			}
			else if (enemyIsAttacking && distanceX < 2)
			{
				return 2;
			}else if(enemyHeight > 1)
			{
				return 2;
			}else
			{
				//diferencial vida, timer grande

				//diferencial vida, timer pequeno
				//vida distancia
				//selfheight enemyHeight
			}
		}
	}

}