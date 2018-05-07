using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatorGeneric
{
    public class GenericFunctions : StateMachineBehaviour
    {
        AnimatorControllerParameter[] parameters;

        public List<AnimatorControllerParameter> GetAllActiveParameters(Animator animator)
        {
            parameters = animator.parameters;
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


    }
}

