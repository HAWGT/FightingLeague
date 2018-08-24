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

        [SerializeField]
        private GameObject flarePrefab;

        [SerializeField]
        private GameObject smokePrefab;

        [SerializeField]
        private GameObject steamPrefab;

        [SerializeField]
        private GameObject rayPrefab;

        [SerializeField]
        private GameObject shockPrefab;

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

        public List<AnimatorControllerParameter> GetAllBoolTriggerAnimatorParameters()
        {
            List<AnimatorControllerParameter> list = new List<AnimatorControllerParameter>();
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool || parameter.type == AnimatorControllerParameterType.Trigger)
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
                GetComponent<CharacterStateController>().SetCharState(Enums.CharState.blocking);
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
            if (!animator.GetBool("airborn"))
            {
                animator.applyRootMotion = false;
				animator.SetBool("airborn", true);
				animator.SetTrigger("jump");
				if (rigidbody.velocity.y < 5f)
                    rigidbody.AddForce(new Vector3(0, 10f), ForceMode.VelocityChange);
            }
        }

        public void Knock(float dmg)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
            Vector3 pos = rigidbody.transform.position;
            float side = 0;
            if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
            {
                    rigidbody.transform.position += new Vector3(-0.13f * (dmg / 1000), 0);
                    side = 0.5f;
            } else if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                    rigidbody.transform.position += new Vector3(0.13f * (dmg / 1000), 0);
                    side = -0.5f;
            }
            if ((rigidbody.transform.position.x < -7 || rigidbody.transform.position.x > 7) && dmg > 100)
            {
                rigidbody.transform.position += new Vector3(side * 4, 3f);
                var smoke = (GameObject)Instantiate(smokePrefab, pos + new Vector3(side, 1.25f, 0), rot);
                Destroy(smoke, 2f);
            }
            var flare = (GameObject)Instantiate(flarePrefab, pos + new Vector3(side , 0.6f, 0), rot);
            Destroy(flare, 0.25f);
        }

        public void BlockFX()
        {
            float side = 0;
            if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
            {
                side = 0.5f;
            }
            else if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                side = -0.5f;
            }
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
            Vector3 pos = rigidbody.transform.position;
            var steam = (GameObject)Instantiate(steamPrefab, pos + new Vector3(side, 0), Quaternion.Euler(-90, 0, 0 ));
            Destroy(steam, 0.5f);
        }

        public void VerticalTeleportFX()
        {
            GetComponent<CharacterStateController>().TeleportSFX();
            float side = 0;
            if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
            {
                side = 0.5f;
            }
            else if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                side = -0.5f;
            }
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
            Vector3 pos = rigidbody.transform.position;
            var ray = (GameObject)Instantiate(rayPrefab, pos + new Vector3(side, 0), Quaternion.Euler(-90, 0, 0));
            Destroy(ray, 0.25f);
        }

        public void HorizontalTeleportFX()
        {
            GetComponent<CharacterStateController>().TeleportSFX();
            float side = 0;
            if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
            {
                side = 1f;
            }
            else if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                side = -1f;
            }
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
            Vector3 pos = rigidbody.transform.position;
            var ray = (GameObject)Instantiate(rayPrefab, pos + new Vector3(0, 0.7f), Quaternion.Euler(0, -90 * side, 0));
            Destroy(ray, 0.1f);
        }

        public void Shockwave()
        {
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
                Vector3 pos = rigidbody.transform.position;
                var shock = (GameObject)Instantiate(shockPrefab, pos , Quaternion.Euler(0, 0, 0));
                Destroy(shock, 0.35f);
        }

        public void Push(float dmg)
        {
            if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
            {
                rigidbody.transform.position += new Vector3(-0.13f * (dmg / 1000), 0);
            }
            else if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                rigidbody.transform.position += new Vector3(0.13f * (dmg / 1000), 0);
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
				if (parameter.name == "walkingForward" || parameter.name == "walkingBackward")
				{
					list.Add(parameter);
				}
			}
			TurnAnimatorParametersOff(list);
		}

        public void ResetAnim()
        {
            bool state1 = animator.GetBool("mirrorAnimation");
            bool state2 = animator.GetBool("airborn");
            GetAnimator().Rebind();
            animator.SetBool("mirrorAnimation", state1);
            animator.SetBool("airborn", state2);

        }
    }

}