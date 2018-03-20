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

        private CharacterStateController stateController;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            stateController = GetComponent<CharacterStateController>();
        }

        // Update is called once per frame
        void Update()
        {
            // standing, walkingF, walkingB, crouching, airborn, blocking, attacking, hitstun
            animator.SetBool("walkingForward", false);
            animator.SetBool("walkingBackward", false);
            animator.SetBool("crouch", false);
            animator.SetBool("lightattack", false);
            animator.SetBool("mediumattack", false);
            animator.SetBool("heavyattack", false);
            animator.SetBool("airborn", false);
            switch (stateController.charState)
            {
                case CharacterStateController.CharState.standing:
                    break;
                case CharacterStateController.CharState.walkingF:
                    animator.SetBool("walkingForward", true);
                    break;
                case CharacterStateController.CharState.walkingB:
                    animator.SetBool("walkingBackward", true);
                    break;
                case CharacterStateController.CharState.crouching:
                    animator.SetBool("crouch", true);
                    break;
                case CharacterStateController.CharState.airborn:
                    animator.SetBool("airborn", true);
                    break;
                case CharacterStateController.CharState.blocking:
                    break;
                case CharacterStateController.CharState.attacking:
                    attackTrigger();
                    break;
                case CharacterStateController.CharState.hitstun:
                    break;
            }

        }

        private void attackTrigger()
        {
            switch (stateController.attackState)
            {
                case CharacterStateController.AttackState.none:
                    break;
                case CharacterStateController.AttackState.light:
                    animator.SetBool("lightattack", true);
                    break;
                case CharacterStateController.AttackState.medium:
                    animator.SetBool("mediumattack", false);
                    break;
                case CharacterStateController.AttackState.heavy:
                    animator.SetBool("heavyattack", false);
                    break;
            }
        }
    }

}