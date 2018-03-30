using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterStateController : MonoBehaviour
    {
        [SerializeField]
        public float healthPoints = 10000;

        [SerializeField]
        public float superBar;

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

        public CharState charState;

        public AttackState attackState;

        public FacingSide facing;

        // Use this for initialization
        void Start()
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
        void Update()
        {

        }
    }



}

