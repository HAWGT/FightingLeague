using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using BackGame;

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
        private bool isJumping;
        [Task]
        private bool grab;
        [Task]
        private bool defend;
        [Task]
        private bool counteredProjectile = true;
        [Task]
        private bool counteredBeam = true;
        [Task]
        private bool canChallenge = false;
        [Task]
        private bool gameEnded = false;
        [Task]
        private bool canAtk = false;
        [Task]
        private bool canApproach = false;

        private GameObject game;

        private Enums.AttackState lastAttack = Enums.AttackState.none;

        private float lastHP = 10000;

        private AnimationController animControl;

        private CharacterStateController stateController;

        private void Start()
        {
            animControl = GetComponent<AnimationController>();
            stateController = GetComponent<CharacterStateController>();
            game = GameObject.Find("Game Manager");
        }

        private void Update()
        {
            gameEnded = game.GetComponent<GameManager>().IsGameOver();
            canApproach = false;
            canAtk = false;
            canChallenge = false;
            enemyAttacking = false;
            enemyBlocking = false;
            enemyClose = false;
            enemyJumping = false;
            isJumping = false;
            grab = false;
            defend = false;
            if (!isJumping)
            {
                stateController.SetLastAtk(Enums.AttackState.none);
                stateController.SetCharState(Enums.CharState.standing);
            }
            float currentSuperBar = GetComponent<CharacterStateController>().GetSB();

            if (lastHP != GetComponent<CharacterStateController>().GetHP())
            {
                counteredProjectile = true;
                counteredBeam = true;
                lastHP = GetComponent<CharacterStateController>().GetHP();
            }

            float dist = System.Math.Abs(GetComponent<Rigidbody>().position.x - GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.x);
            Enums.CharState enemyState = GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().GetCharState();
            Enums.AttackState attackState = GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().GetTypeOfAtk();

            if (attackState == Enums.AttackState.special1 && attackState == lastAttack)
            {
                counteredProjectile = false;
            } else if (attackState == Enums.AttackState.super && attackState == lastAttack)
            {
                counteredBeam = false;
                if (currentSuperBar >= 50f && lastHP > GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<CharacterStateController>().GetHP() && attackState == Enums.AttackState.super) canChallenge = true;
            } else
            {
                lastAttack = Enums.AttackState.none;
                counteredBeam = true;
                counteredProjectile = true;
            }

            if (GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.y > 0.26f) enemyJumping = true;
            if (GetComponent<Rigidbody>().position.y > 0.26f) isJumping = true;
            if (dist < 1.5f) enemyClose = true;
            if (enemyState == Enums.CharState.attacking) enemyAttacking = true;
            if (enemyState == Enums.CharState.blocking) enemyBlocking = true;
            if (enemyClose && enemyState == Enums.CharState.blocking) grab = true;
            if (enemyClose && enemyState == Enums.CharState.attacking) defend = true;
            if (enemyClose && !defend) canAtk = true;
            canApproach = counteredBeam && counteredProjectile;
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
            counteredProjectile = true;
            Task.current.Succeed();
        }
        [Task]
        public void Grab()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "guardBreak" }));
            counteredProjectile = true;
            Task.current.Succeed();
        }
        [Task]
        public void Special1()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "special1" }));
            stateController.SetLastAtk(Enums.AttackState.special1);
            stateController.SetCharState(Enums.CharState.attacking);
            counteredProjectile = true;
            Task.current.Succeed();
        }
        [Task]
        public void Special2()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "special2" }));
            stateController.SetLastAtk(Enums.AttackState.special2);
            stateController.SetCharState(Enums.CharState.attacking);
            counteredProjectile = true;
            Task.current.Succeed();
        }
        [Task]
        public void Super()
        {
            if (GetComponent<CharacterStateController>().GetSB() >= 50f)
            {
                animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "super" }));
                stateController.SetLastAtk(Enums.AttackState.super);
                stateController.SetCharState(Enums.CharState.attacking);
                counteredProjectile = true;
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }
        [Task]
        public void Dash()
        {
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "midDash" }));
            counteredBeam = true;
            Task.current.Succeed();
        }
        [Task]
        public void Vanish()
        {
            if (GetComponent<CharacterStateController>().GetSB() >= 10f)
            {
                animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "vanish" }));
                counteredBeam = true;
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
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
                stateController.SetLastAtk(Enums.AttackState.light);
                stateController.SetCharState(Enums.CharState.attacking);
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
                stateController.SetLastAtk(Enums.AttackState.medium);
                stateController.SetCharState(Enums.CharState.attacking);
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
                stateController.SetLastAtk(Enums.AttackState.heavy);
                stateController.SetCharState(Enums.CharState.attacking);
                animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "heavyAttack" }));
                Task.current.Succeed();
            }
        }
    }
}
