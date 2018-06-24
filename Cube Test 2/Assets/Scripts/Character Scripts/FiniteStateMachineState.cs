using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{

    public class FiniteStateMachineState : MonoBehaviour
    {
        protected static Dictionary<Enums.Transition, Enums.Inputs> map = new Dictionary<Enums.Transition, Enums.Inputs>();
        protected Enums.Inputs currentState;
        public Enums.Inputs ID { get { return currentState; } }

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
            if (IsInvoking("CountTime"))
            {
                StopAllCoroutines();
            }
            StartCoroutine(CountTime());
        }


        private IEnumerator CountTime()
        {
            yield return new WaitForSeconds(1f);
            ResetMachine();
        }


        public Enums.Inputs PerformTransition(Enums.Inputs input, List<Enums.AttackState> attacks)
        {
			List<Enums.Inputs> localParse = new List<Enums.Inputs>();


			switch (currentState)
			{
				case Enums.Inputs.Neutral:
					switch (input)
					{
						case Enums.Inputs.Down:
								map.Add(Enums.Transition.NeutralDown, input);
								currentState = Enums.Inputs.Down;
								StartCoroutine(CountTime());
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

				case Enums.Inputs.Back:
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
								map.Add(Enums.Transition.BackToMedium, input);
								currentState = Enums.Inputs.Special2;
								StopAllCoroutines();
								break;

							case Enums.Inputs.Back:
								break;

							case Enums.Inputs.Neutral:
								break;

							default:
								ResetMachine();
								break;
						}
						break;

					case Enums.Inputs.Front:
						switch (input)
						{
							case Enums.Inputs.Medium:
								map.Add(Enums.Transition.FrontToMedium, input);
								currentState = Enums.Inputs.Special1;
								StopAllCoroutines();
								break;

							case Enums.Inputs.Heavy:
								map.Add(Enums.Transition.FrontToHeavy, input);
								currentState = Enums.Inputs.Super;
								StopAllCoroutines();
								break;

							case Enums.Inputs.Front:
								break;

							case Enums.Inputs.Neutral:
								break;

							default:
								ResetMachine();
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
				else
				{
					ResetMachine();
					return biggestestTack;
				}
			}
			else
			{
				ResetMachine();
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
 