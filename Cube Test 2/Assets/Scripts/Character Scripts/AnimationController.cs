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
                    animator.SetBool(parameter.name, false);
                }
            }
        }

        public void TurnAnimatorParametersOn(List<AnimatorControllerParameter> list)
        {
            if (list.Count != 0)
            {
                foreach (AnimatorControllerParameter parameter in list)
                {
                    animator.SetBool(parameter.name, true);
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

        public void TurnOnRootMotion()
        {
            animator.applyRootMotion = true;
        }

        public void TurnOffRootMotion()
        {
            animator.applyRootMotion = false;
        }
        public void Jump()
        {
            animator.applyRootMotion = false;
            animator.SetBool("airborn", true);
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
    }

}