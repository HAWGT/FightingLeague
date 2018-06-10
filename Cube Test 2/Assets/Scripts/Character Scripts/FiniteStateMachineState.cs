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


        public Enums.Inputs PerformTransition(Enums.Inputs input)
        {
            switch (currentState)
            {
                case Enums.Inputs.Neutral:
                    switch (input)
                    {
                        case Enums.Inputs.Down:
                            map.Add(Enums.Transition.NeutralDown, input);
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
                            break;

                        case Enums.Inputs.DownFront:
                            map.Add(Enums.Transition.DownToDF, input);
                            break;

                        case Enums.Inputs.Down:
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
                            break;

                        case Enums.Inputs.DownBack:
                            break;

                        default:
                            ResetMachine();
                            break;
                    }
                    break;

                case Enums.Inputs.Back:
                    switch (input)
                    {
                        case Enums.Inputs.Medium:
                            map.Add(Enums.Transition.BackToMedium, input);
                            StopAllCoroutines();
                            break;

                        case Enums.Inputs.Back:
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
                            break;

                        case Enums.Inputs.DownFront:
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
                            StopAllCoroutines();
                            break;

                        case Enums.Inputs.Heavy:
                            map.Add(Enums.Transition.FrontToHeavy, input);
                            StopAllCoroutines();
                            break;

                        case Enums.Inputs.Front:
                            break;

                        default:
                            ResetMachine();
                            break;
                    }
                    break;
            }

            currentState = input;

            if (map.ContainsKey(Enums.Transition.FrontToHeavy))
            {
                ResetMachine();
                return Enums.Inputs.Super;
            }else if (map.ContainsKey(Enums.Transition.FrontToMedium))
            {
                ResetMachine();
                return Enums.Inputs.Special1;
            }else if (map.ContainsKey(Enums.Transition.BackToMedium))
            {
                ResetMachine();
                return Enums.Inputs.Special2;
            }
            else
            {
                return input;
            }
        }


        public Enums.Inputs GetOutputState(Enums.Transition trans)
        {
            //check if map has transition
            if (map.ContainsKey(trans))
            {
                return map[trans];
            }
            return Enums.Inputs.NullStateID;
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
 