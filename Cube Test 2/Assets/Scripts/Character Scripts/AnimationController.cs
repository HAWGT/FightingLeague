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

        private Rigidbody rigidbody;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void SetRigidBody()
        {
            
        }
        
        public void WalkFwd()
        {
            animator.SetBool("walkingForward", true);
        }

        public void WalkBwd()
        {
            animator.SetBool("walkingBackward", true);
        }

        public void Crouch()
        {
            animator.SetBool("crouch", true);
        }

        public void Jump()
        {
            animator.applyRootMotion = false;
            animator.SetBool("airborn", true);
        }

        public void Hitstun()
        {
            animator.SetBool("hitstun", true);
        }

        public void Block()
        {
            animator.SetBool("blocking", true);
        }

        public void CrouchBlock()
        {
            animator.SetBool("crouchBlock", true);
        }

        public void LightAtk()
        {
            animator.SetBool("lightAttack", true);
        }

        public void MediumAtk()
        {
            animator.SetBool("mediumAttack", true);
        }

        public void HeavyAtk()
        {
            animator.SetBool("heavyAttack", true);
        }

        public void Special1()
        {
            animator.SetBool("special1", true);
        }

        public void Special2()
        {
            animator.SetBool("special2", true);
        }

        public void Super()
        {
            animator.SetBool("super", true);
        }

        public void StopWalkFwd()
        {
            animator.SetBool("walkingForward", false);
        }

        public void StopWalkBwd()
        {
            animator.SetBool("walkingBackward", false);
        }

        public void StopCrouch()
        {
            animator.SetBool("crouch", false);
        }

        public void StopAirborn()
        {
            animator.applyRootMotion = true;
            animator.SetBool("airborn", false);
        }

        public void StopHitstun()
        {
            animator.SetBool("hitstun", false);
        }

        public void StopBlock()
        {
            animator.SetBool("standblock", false);
        }

        public void StopCrouchBlock()
        {
            animator.SetBool("crouchBlock", false);
        }

        public void StopLightAtk()
        {
            animator.SetBool("lightAttack", false);
        }

        public void StopMediumAtk()
        {
            animator.SetBool("mediumAttack", false);
        }

        public void StopHeavyAtk()
        {
            animator.SetBool("heavyAttack", false);
        }

        public void StopSpecial1()
        {
            animator.SetBool("special1", false);
        }

        public void StopSpecial2()
        {
            animator.SetBool("special2", false);
        }

        public void StopSuper()
        {
            animator.SetBool("super", false);
        }
    }

}