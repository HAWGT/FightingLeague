using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

namespace CharacterControl
{
    public class PlayerAITasks : MonoBehaviour
    {
        [Task]
        private bool enemyAttacking;
        [Task]
        private bool enemyBlocking;
        [Task]
        private bool enemyClose;
        [Task]
        private bool enemyJumping;
        [Task]
        private float currentSuperBar;
        [Task]
        private bool isJumping;

        private AnimationController animControl;

        private void Start()
        {
            animControl = GetComponent<AnimationController>();
        }

        private void Update()
        {
            enemyAttacking = false;
            enemyBlocking = false;
            enemyClose = false;
            enemyJumping = false;
            isJumping = false;
            float dist = System.Math.Abs(GetComponent<Rigidbody>().position.x - GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.x);
            Enums.CharState enemyState = GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().GetCharState();
            if (GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.y > 0.26f) enemyJumping = true;
            if (GetComponent<Rigidbody>().position.y > 0.26f) isJumping = true;
            if (dist < 1.5f) enemyClose = true;
            if (enemyState == Enums.CharState.attacking) enemyAttacking = true;
            if (enemyState == Enums.CharState.blocking) enemyBlocking = true;
            currentSuperBar = GetComponent<CharacterStateController>().GetSB();
        }

        [Task]
        public void WalkForward()
        {
            animControl.WalkFwd();
            Task.current.Succeed();
        }
        [Task]
        public void WalkBackward()
        {
            animControl.WalkBwd();
            Task.current.Succeed();
        }
        [Task]
        public void Jump()
        {
            if (isJumping)
            {
                Task.current.Fail();
            }
            else
            {
                animControl.Jump();
                Task.current.Succeed();
            }
            
        }
        [Task]
        public void Crouch()
        {
            animControl.TurnAnimatorParametersOn(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "crouch" }));
            Task.current.Succeed();
        }
        [Task]
        public void Reflect()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "reflect" }));
            Task.current.Succeed();
        }
        [Task]
        public void Grab()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "guardBreak" }));
            Task.current.Succeed();
        }
        [Task]
        public void Special1()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "special1" }));
            Task.current.Succeed();
        }
        [Task]
        public void Special2()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "special2" }));
            Task.current.Succeed();
        }
        [Task]
        public void Super()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "super" }));
            Task.current.Succeed();
        }
        [Task]
        public void Dash()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "midDash" }));
            Task.current.Succeed();
        }
        [Task]
        public void Vanish()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "vanish" }));
            Task.current.Succeed();
        }
        [Task]
        public void AttackL()
        {
            if (isJumping)
            {
                Task.current.Fail();
            }
            else
            {
                animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "lightAttack" }));
                Task.current.Succeed();
            }
        }
        [Task]
        public void AttackM()
        {
            if (isJumping)
            {
                Task.current.Fail();
            }
            else
            {
                animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "mediumAttack" }));
                Task.current.Succeed();
            }
        }
        [Task]
        public void AttackH()
        {
            if (isJumping)
            {
                Task.current.Fail();
            }
            else
            {
                animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "heavyAttack" }));
                Task.current.Succeed();
            }
        }
    }
}
