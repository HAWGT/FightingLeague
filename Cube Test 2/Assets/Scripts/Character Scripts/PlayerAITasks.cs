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
        private bool canAtk = false;
        [Task]
        private bool canApproach = false;

        private GameObject game;

        private Enums.AnimState lastState = Enums.AnimState.none;

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
            Enums.AnimState enemyState = StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>());

            if (StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()) == Enums.AnimState.special1 && enemyState == lastState)
            {
                counteredProjectile = false;
            } else if (enemyState == Enums.AnimState.super && enemyState == lastState)
            {
                counteredBeam = false;
                if (currentSuperBar >= 50f && enemyState == Enums.AnimState.super) canChallenge = true;
            } else
            {
                lastState = Enums.AnimState.none;
                counteredBeam = true;
                counteredProjectile = true;
            }

            if (GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>().position.y > 0.26f) enemyJumping = true;
            if (GetComponent<Rigidbody>().position.y > 0.26f) isJumping = true;
            if (dist < 1f) enemyClose = true;
            if (StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()) == Enums.AnimState.light || StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()) == Enums.AnimState.medium || StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()) == Enums.AnimState.heavy || StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()) == Enums.AnimState.special1 || StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()) == Enums.AnimState.special2 || StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()) == Enums.AnimState.super) enemyAttacking = true;
            if (StateHelper.GetState(GetComponent<CharacterColliderController>().GetOtherPlayer().GetComponent<Rigidbody>()) == Enums.AnimState.walkingB) enemyBlocking = true;
            if (enemyClose && enemyBlocking) grab = true;
            if (enemyClose && enemyAttacking) defend = true;
            if (enemyClose && !defend) canAtk = true;
            canApproach = counteredBeam && counteredProjectile;
        }

        [Task]
        public void WalkForward()
        {
            if (Task.current == null) return;
            animControl.WalkFwd();
            Task.current.Succeed();
        }
        [Task]
        public void WalkBackward()
        {
            if (Task.current == null) return;
            animControl.WalkBwd();
            Task.current.Succeed();
        }
        [Task]
        public void UpJump()
        {
            if (Task.current == null) return;
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
        public void Reflect()
        {
            if (Task.current == null) return;
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "reflect" }));
            counteredProjectile = true;
            Task.current.Succeed();
        }
        [Task]
        public void Grab()
        {
            if (Task.current == null) return;
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "guardBreak" }));
            counteredProjectile = true;
            Task.current.Succeed();
        }
        [Task]
        public void Special1()
        {
            if (Task.current == null) return;
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "special1" }));
            stateController.SetLastAtk(Enums.AttackState.special1);
            stateController.SetCharState(Enums.CharState.attacking);
            counteredProjectile = true;
            Task.current.Succeed();
        }
        [Task]
        public void Special2()
        {
            if (Task.current == null) return;
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "special2" }));
            stateController.SetLastAtk(Enums.AttackState.special2);
            stateController.SetCharState(Enums.CharState.attacking);
            counteredProjectile = true;
            Task.current.Succeed();
        }
        [Task]
        public void Super()
        {
            if (Task.current == null) return;
            if (GetComponent<CharacterStateController>().GetSB() >= 50f)
            {
                GetComponent<CharacterStateController>().Super();
                counteredProjectile = true;
                counteredBeam = true;
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
            if (Task.current == null) return;
            animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "midDash" }));
            counteredBeam = true;
            Task.current.Succeed();
        }
        [Task]
        public void Vanish()
        {
            if (Task.current == null) return;
            if (GetComponent<CharacterStateController>().GetSB() >= 10f)
            {
                GetComponent<CharacterStateController>().Vanish();
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
            if (Task.current == null) return;
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
            if (Task.current == null) return;
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
            if (Task.current == null) return;
            if (isJumping)
            {
                Task.current.Fail();
            }
            else
            {
                stateController.SetLastAtk(Enums.AttackState.heavy);
                stateController.SetCharState(Enums.CharState.attacking);
                animControl.TriggerAnimatorParameters(GetComponent<CharacterStateController>().FindAnimatorParameter(new string[] { "heavyAttack" }));
                counteredProjectile = true;
                Task.current.Succeed();
            }
        }
    }
}
