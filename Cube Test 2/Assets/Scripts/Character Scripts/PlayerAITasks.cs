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

        private void Update()
        {
            enemyAttacking = false;
            enemyBlocking = false;
            enemyClose = false;
            enemyJumping = false;
            float dist = System.Math.Abs(GetComponent<Rigidbody>().position.x - GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.x);
            Enums.CharState enemyState = GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().GetCharState();
            if (GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.y > 0.26f) enemyJumping = true;
            if (dist < 1.5f) enemyClose = true;
            if (enemyState == Enums.CharState.attacking) enemyAttacking = true;
            if (enemyState == Enums.CharState.blocking) enemyBlocking = true;
            currentSuperBar = GetComponent<CharacterStateController>().GetSB();
        }

        [Task]
        public void WalkForward()
        {
            GetComponent<AnimationController>().WalkFwd();
            Task.current.Succeed();
        }
        [Task]
        public void WalkBackward()
        {
            GetComponent<AnimationController>().WalkBwd();
            Task.current.Succeed();
        }
        [Task]
        public void Jump()
        {

        }
        [Task]
        public void Crouch()
        {

        }
        [Task]
        public void Reflect()
        {

        }
        [Task]
        public void Grab()
        {
        }
        [Task]
        public void Special1()
        {

        }
        [Task]
        public void Special2()
        {

        }
        [Task]
        public void Super()
        {

        }
        [Task]
        public void Dash()
        {

        }
        [Task]
        public void Vanish()
        {

        }
        [Task]
        public void AttackL()
        {

        }
        [Task]
        public void AttackM()
        {

        }
        [Task]
        public void AttackH()
        {

        }
    }
}
