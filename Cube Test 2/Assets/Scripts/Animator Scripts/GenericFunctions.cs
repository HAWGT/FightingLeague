using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AnimatorGeneric
{
    public class GenericFunctions : StateMachineBehaviour
    {
        List<AnimatorControllerParameter> parameters = null;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(parameters == null)
            {
                parameters = animator.parameters.ToList();
            }
        }

        public List<AnimatorControllerParameter> GetAllActiveParameters(Animator animator)
        {
            parameters = animator.parameters.ToList();
            List<AnimatorControllerParameter> activeParameters = new List<AnimatorControllerParameter>();

            foreach (AnimatorControllerParameter parameter in parameters)
            {
                if(parameter.type == AnimatorControllerParameterType.Bool)
                {
                    if (animator.GetBool(parameter.name))
                        activeParameters.Add(parameter);
                }
            }
            
            
            return activeParameters;
        }

        public AnimatorControllerParameter GetSpecificParameter(string name)
        {
            AnimatorControllerParameter askedParameter = parameters.Find(param => param.name == name);

            return parameters[0];
        }
    }
}

