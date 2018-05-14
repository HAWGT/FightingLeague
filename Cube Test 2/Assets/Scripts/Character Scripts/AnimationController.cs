using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class AnimationController : MonoBehaviour
    {

        [SerializeField]
        private Animator animator;

        private new Rigidbody rigidbody;

        private float maxAirSpeed = 5f;

        private Vector3 airMovement = new Vector3 (1f, 0, 0);

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void SetRigidBody(Rigidbody rb)
        {
            rigidbody = rb;
        }

        public List<AnimatorControllerParameter> GetAllBoolAnimatorParameters()
        {
            List<AnimatorControllerParameter> list = new List<AnimatorControllerParameter>();
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    list.Add(parameter);
                }
            }
            return list;
        }

        public void TurnAnimatorParametersOff(List<AnimatorControllerParameter> list)
        {
            if(list.Count != 0)
            {
                foreach (AnimatorControllerParameter parameter in list)
                {
                    if(parameter!= null)
                    {
                        animator.SetBool(parameter.name, false);
                    }

                }
            }
        }

        public void TurnAnimatorParametersOn(List<AnimatorControllerParameter> list)
        {
            if (list.Count != 0)
            {
                foreach (AnimatorControllerParameter parameter in list)
                {
                    if (parameter != null)
                    {
                        animator.SetBool(parameter.name, true);
                    }

                }
            }
        }

        
        public void WalkFwd()
        {
            if (animator.GetBool("airborn"))
            {
                AddAirSpeed(airMovement);
            }
            else
            {
                animator.SetBool("walkingForward", true);
            }
        }

        public void WalkBwd()
        {
            if (animator.GetBool("airborn"))
            {
                AddAirSpeed(-airMovement);
            }
            else
            {
                animator.SetBool("walkingBackward", true);
            }
            
        }

        private void TurnOnRootMotion()
        {
            animator.applyRootMotion = true;
        }

        private void TurnOffRootMotion()
        {
            animator.applyRootMotion = false;
        }

        public void Jump()
        {
            if (!animator.GetBool("airborn") && !animator.GetBool("crouch"))
            {
                animator.applyRootMotion = false;
                animator.SetBool("jump", true);
                rigidbody.velocity = rigidbody.velocity + new Vector3(0, 2f);
            }
        }

        private void AddAirSpeed(Vector3 speed)
        {
            if (rigidbody.velocity.x > -maxAirSpeed && speed.x < 0)
            {
                rigidbody.velocity = rigidbody.velocity + speed;
            }
            else if (rigidbody.velocity.x < maxAirSpeed && speed.x > 0)
            {
                rigidbody.velocity = rigidbody.velocity + speed;
            }
            else if (rigidbody.velocity.x > 5f)
            {
                rigidbody.velocity = new Vector3(5f, rigidbody.velocity.y);
            }
            else if (rigidbody.velocity.x < -5f)
            {
                rigidbody.velocity = new Vector3(-5f, rigidbody.velocity.y);
            }
            else
            {
                rigidbody.velocity = rigidbody.velocity + speed;
            }

        }

        private void ResetStateParameters()
        {
            List<AnimatorControllerParameter> list = new List<AnimatorControllerParameter>();

            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name != "airborn" && parameter.name != "2"
                    && parameter.name != "4" && parameter.name != "5" && parameter.name != "6" 
                    && parameter.name != "mirrorAnimation" && parameter.name != "grounded"
                    && parameter.name != "hitstun" && parameter.name != "KO"
                    && parameter.name != "blockStun" && parameter.name != "standBlock"
                    && parameter.name != "crouchBlock")
                {
                    list.Add(parameter);
                }
            }
            TurnAnimatorParametersOff(list);
        }
    }

}