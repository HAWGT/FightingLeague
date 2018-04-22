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

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
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
    }

}