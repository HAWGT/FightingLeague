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

        private Vector3 airMovement = new Vector3 (3f, 0, 0);

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void SetRigidBody(Rigidbody rb)
        {
            rigidbody = rb;
        }

        public void Mirror()
        {
            bool state = animator.GetBool("mirrorAnimation");
            animator.SetBool("mirrorAnimation", !state);
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

        public void TriggerAnimatorParameters(List<AnimatorControllerParameter> list)
        {
            if (list.Count != 0)
            {
                foreach (AnimatorControllerParameter parameter in list)
                {
                    if (parameter != null)
                    {
                        animator.SetTrigger(parameter.name);
                    }

                }
            }
        }

        public void UpdateSuperBar(int ammount)
        {

            animator.SetInteger("superBar", ammount);
        }

        
        public void WalkFwd()
        {
            if (animator.GetBool("airborn"))
            {
                if (rigidbody.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
                {
                    AddAirSpeed(airMovement);
                } else
                {
                    AddAirSpeed(-airMovement);
                }
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
                if (rigidbody.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
                {
                    AddAirSpeed(airMovement);
                }
                else
                {
                    AddAirSpeed(-airMovement);
                }
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
                //animator.applyRootMotion = false;
                animator.SetTrigger("jump");
                if(rigidbody.velocity.y < 5f) {
                    //rigidbody.velocity = rigidbody.velocity + new Vector3(0, 2f);
                    rigidbody.AddForce(new Vector3(0, 2f));
                }
            }
        }

        public void Knock(float dmg)
        {
                animator.applyRootMotion = false;
                if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
                {
                rigidbody.AddForce( new Vector3(- dmg / 1000, dmg / 1000));
            } else if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                    rigidbody.AddForce(new Vector3(dmg / 1000, dmg / 1000));
                }
        }

        public void Push(float dmg)
        {
            animator.applyRootMotion = false;
            if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
            {
                rigidbody.AddForce(new Vector3(dmg / 1000, 0));
            }
            else if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                rigidbody.AddForce(new Vector3(-dmg / 1000, 0));
            }
        }



        private void AddAirSpeed(Vector3 speed)
        {
            /*if (rigidbody.velocity.x > -maxAirSpeed && speed.x < 0)
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
            }*/

            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y) + speed;

        }

        public Animator GetAnimator()
        {
            return GetComponent<Animator>();
        }

        private void ResetStateParameters()
        {
            List<AnimatorControllerParameter> list = new List<AnimatorControllerParameter>();

            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name != "airborn"
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