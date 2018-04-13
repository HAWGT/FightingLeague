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
            switch (stateController.GetCharState())
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
                    AttackTrigger();
                    break;
                case CharacterStateController.CharState.hitstun:
                    break;
            }

        }

        private void LateUpdate()
        {
            // standing, walkingF, walkingB, crouching, airborn, blocking, attacking, hitstun
            animator.SetBool("walkingForward", false);
            animator.SetBool("walkingBackward", false);
            animator.SetBool("crouch", false);
        }

        private void AttackTrigger()
        {
            switch (stateController.GetAttackState())
            {
                case CharacterStateController.AttackState.none:
                    break;
                case CharacterStateController.AttackState.light:
                    animator.SetBool("lightAttack", true);
                    break;
                case CharacterStateController.AttackState.medium:
                    animator.SetBool("mediumAttack", false);
                    break;
                case CharacterStateController.AttackState.heavy:
                    animator.SetBool("heavyAttack", false);
                    break;
            }
        }
    }

}