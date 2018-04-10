using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterStateController : MonoBehaviour
    {
        [SerializeField]
        private float healthPoints = 10000;

        [SerializeField]
        private float superBar = 0;

        [SerializeField]
        private float armor = 1;

        public enum CharState
        {
            standing, walkingF, walkingB, crouching, airborn, blocking, attacking, hitstun
        }

        public enum AttackState
        {
            none, light, medium, heavy
        }

        public enum FacingSide
        {
            P1, P2
        }

        [SerializeField]
        private CharState charState;

        public CharState GetCharState()
        {
            return this.charState;
        }

        public AttackState GetAttackState()
        {
            return this.attackState;
        }

        public float GetHP()
        {
            return this.healthPoints;
        }

        public float GetSB()
        {
            return this.superBar;
        }

        public FacingSide GetFacingSide()
        {
            return this.facing;
        }

        [SerializeField]
        private AttackState attackState;

        [SerializeField]
        private FacingSide facing;

        // Use this for initialization
       private void Start()
        {
            charState = CharState.standing;
            attackState = AttackState.none;
        }

        public void SetState(CharState state)
        {
            charState = state;
        }

        public void SetAttackState(AttackState state)
        {
            attackState = state;
        }

        // Update is called once per frame
        private void Update()
        {

        }

        public void takeDamage(float dmg)
        {
            healthPoints -= dmg / armor;
        }

    }



}

