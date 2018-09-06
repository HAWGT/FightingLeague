using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{

    public class FiniteStateMachineState : MonoBehaviour
    {
        protected Dictionary<Enums.Transition, Enums.Inputs> map = new Dictionary<Enums.Transition, Enums.Inputs>();
        protected Enums.Inputs currentState;
        public Enums.Inputs ID { get { return currentState; } }
		private List<Enums.Inputs> localParse;

		protected static Dictionary<Enums.Transition, Enums.Inputs> refMap = new Dictionary<Enums.Transition, Enums.Inputs>()
        {
            {Enums.Transition.NeutralDown, Enums.Inputs.Down },
            {Enums.Transition.DownDown, Enums.Inputs.Down },
            {Enums.Transition.DownToDB, Enums.Inputs.DownBack },
            {Enums.Transition.DBToBack, Enums.Inputs.Back},
            {Enums.Transition.BackToMedium, Enums.Inputs.Medium },
            {Enums.Transition.DownToDF, Enums.Inputs.DownFront },
            {Enums.Transition.DFDF, Enums.Inputs.DownFront},
            {Enums.Transition.DFToFront, Enums.Inputs.Front},
            {Enums.Transition.FrontToMedium, Enums.Inputs.Medium },
            {Enums.Transition.FrontToHeavy, Enums.Inputs.Heavy }

        };

        public void Start()
        {
            currentState = Enums.Inputs.Neutral;
			StartCoroutine(CountTime());
        }

        public void ResetMachine()
        {
            currentState = Enums.Inputs.Neutral;
			map.Clear();
                StopAllCoroutines();
            StartCoroutine(CountTime());
        }


        private IEnumerator CountTime()
        {
			
			yield return new WaitForSeconds(1f);
            ResetMachine();
        }


        public Enums.Inputs PerformTransition(Enums.Inputs input, List<Enums.AttackState> attacks)
        {
			localParse = new List<Enums.Inputs>();
			switch (currentState)
			{
				case Enums.Inputs.Neutral:
					switch (input)
					{
						case Enums.Inputs.Down:
							if (map.ContainsKey(Enums.Transition.DownToNeutral) && !map.ContainsKey(Enums.Transition.DoubleToDown))
							{
								map.Add(Enums.Transition.DoubleToDown, input);
								currentState = Enums.Inputs.Down;
							}
							else if(map.ContainsKey(Enums.Transition.DownToNeutral) && map.ContainsKey(Enums.Transition.DoubleToDown))
							{
								break;
							}
							else
							{
								map.Add(Enums.Transition.NeutralDown, input);
								currentState = Enums.Inputs.Down;
							}
								
							break;

						case Enums.Inputs.Neutral:
							break;

						default:
							ResetMachine();
							break;
					}
					break;

				case Enums.Inputs.Down:
					switch (input)
					{
						case Enums.Inputs.DownBack:
							map.Add(Enums.Transition.DownToDB, input);
							currentState = Enums.Inputs.DownBack;

							break;

						case Enums.Inputs.DownFront:
							map.Add(Enums.Transition.DownToDF, input);
							currentState = Enums.Inputs.DownFront;
							break;

						case Enums.Inputs.Down:
							break;

						case Enums.Inputs.Neutral:
							if (!map.ContainsKey(Enums.Transition.DownToNeutral))
							{
								map.Add(Enums.Transition.DownToNeutral, input);
								currentState = Enums.Inputs.Neutral;
							}
							
							break;

						default:
							ResetMachine();
							break;
					}
					break;

				case Enums.Inputs.DownBack:
					switch (input)
					{
						case Enums.Inputs.Back:
							map.Add(Enums.Transition.DBToBack, input);
							currentState = Enums.Inputs.Back;
							break;

						case Enums.Inputs.DownBack:
							break;

						case Enums.Inputs.Neutral:
							break;

						default:
							ResetMachine();
							break;
					}
					break;

				case Enums.Inputs.DownFront:
					switch (input)
					{
						case Enums.Inputs.Front:
							map.Add(Enums.Transition.DFToFront, input);
							currentState = Enums.Inputs.Front;
							break;

						case Enums.Inputs.DownFront:
							break;

						case Enums.Inputs.Neutral:
							break;

						default:
							ResetMachine();
							break;
					}
					break;

				case Enums.Inputs.Front:
					break;

				case Enums.Inputs.Back:
					break;

				default:
					ResetMachine();
					break;
			}

			//cycle attacks for decision
			foreach (Enums.AttackState attack in attacks)
			{
				if (attack != Enums.AttackState.none)
				{
					switch (attack)
					{
						case Enums.AttackState.light:
							localParse.Add(Enums.Inputs.Light);
							break;
						case Enums.AttackState.medium:
							localParse.Add(Enums.Inputs.Medium);
							break;
						case Enums.AttackState.heavy:
							localParse.Add(Enums.Inputs.Heavy);
							break;
						default:
							break;
					}
				}
			}

			foreach (Enums.Inputs attack in localParse)
			{
                switch (currentState)
				{
					case Enums.Inputs.Back:
						switch (attack)
						{
							case Enums.Inputs.Medium:
								map.Add(Enums.Transition.BackToMedium, attack);
								currentState = Enums.Inputs.Special2;
								break;

							default:
								ResetMachine();
								break;
						}
						break;

					case Enums.Inputs.Front:
						switch (attack)
						{
							case Enums.Inputs.Medium:
								map.Add(Enums.Transition.FrontToMedium, attack);
								currentState = Enums.Inputs.Special1;
								break;

							case Enums.Inputs.Heavy:
								map.Add(Enums.Transition.FrontToHeavy, attack);
								currentState = Enums.Inputs.Super;
								break;

							default:
								ResetMachine();
								break;
						}
						break;

					case Enums.Inputs.Down:
						switch (attack)
						{
							case Enums.Inputs.Light:
								if (map.ContainsKey(Enums.Transition.DoubleToDown))
								{
									map.Add(Enums.Transition.DDownToLight, attack);
									currentState = Enums.Inputs.Vanish;
								}
								break;

							case Enums.Inputs.Medium:
								if (map.ContainsKey(Enums.Transition.DoubleToDown))
								{
									map.Add(Enums.Transition.DDownToMedium, attack);
									currentState = Enums.Inputs.Dash;
								}
								break;
						}
						break;

					default:
						ResetMachine();
						break;
				}
			}

			if (attacks.Count > 0)
			{
				Enums.Inputs biggestestTack = Enums.Inputs.NullStateID;

				foreach(Enums.Inputs tack in localParse)
				{
					if(biggestestTack == Enums.Inputs.NullStateID)
					{
						biggestestTack = tack;
					}
					else
					{
						switch (biggestestTack)
						{
							case Enums.Inputs.Medium:
								if (tack == Enums.Inputs.Heavy)
									biggestestTack = tack;
								break;
							case Enums.Inputs.Light:
								biggestestTack = tack;
								break;
							default:
								break;
						}
					}
				}

				if(attacks.Contains(Enums.AttackState.heavy) && attacks.Contains(Enums.AttackState.medium))
				{
					ResetMachine();
					return Enums.Inputs.GuardBreak;
				}

				if(attacks.Contains(Enums.AttackState.light) && attacks.Contains(Enums.AttackState.medium))
				{
					ResetMachine();
					return Enums.Inputs.Reflect;
				}

				if (map.ContainsKey(Enums.Transition.FrontToHeavy))
				{
					ResetMachine();
					return Enums.Inputs.Super;
				}
				else if (map.ContainsKey(Enums.Transition.FrontToMedium))
				{
					ResetMachine();
					return Enums.Inputs.Special1;
				}
				else if (map.ContainsKey(Enums.Transition.BackToMedium))
				{
					ResetMachine();
					return Enums.Inputs.Special2;
				}
				else if (map.ContainsKey(Enums.Transition.DDownToLight))
				{
					ResetMachine();
					return Enums.Inputs.Vanish;
				}
				else if (map.ContainsKey(Enums.Transition.DDownToMedium))
				{
					ResetMachine();
					return Enums.Inputs.Dash;
				}
				else
				{
					ResetMachine();
					return biggestestTack;
				}
			}
			else
			{
				return input;
			}


		}


        public Boolean GetOutputState(Enums.Transition trans)
        {
            //check if map has transition
            if (map.ContainsKey(trans))
            {
                return true;
            }
            return false;
        }

        ///<summary> A happy little tree
        ///                                                 .
        ///                                      .         ;  
        ///         .              .              ;%     ;;   
        ///           ,           ,                :;%  %;   
        ///            :         ;                   :;%;'     .,   
        ///   ,.        %;     %;            ;        %;'    ,;
        ///     ;       ;%;  %%;        ,     %;    ;%;    ,%'
        ///      %;       %;%;      ,  ;       %;  ;%;   ,%;' 
        ///       ;%;      %;        ;%;        % ;%;  ,%;'
        ///        `%;.     ;%;     %;'         `;%%;.%;'
        ///         `:;%.    ;%%. %@;        %; ;@%;%'
        ///            `:%;.  :;bd%;          %;@%;'
        ///              `@%:.  :;%.         ;@@%;'   
        ///                `@%.  `;@%.      ;@@%;         
        ///                  `@%%. `@%%    ;@@%;        
        ///                    ;@%. :@%%  %@@%;       
        ///                      %@bd%%%bd%%:;     
        ///                        #@%%%%%:;;
        ///                        %@@%%%::;
        ///                        %@@@%(o);  . '         
        ///                        %@@@o%;:(.,'         
        ///                    `.. %@@@o%::;         
        ///                       `)@@@o%::;         
        ///                        %@@(o)::;        
        ///                       .%@@@@%::;         
        ///                       ;%@@@@%::;.          
        ///                      ;%@@@@%%:;;;. 
        ///                  ...;%@@@@@%%:;;;;,.. 
        ///</summary>
    } // class FiniteStateMotion
}
 