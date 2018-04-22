﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterColliderController : MonoBehaviour
    {

        [SerializeField]
        private Collider cleft;
        [SerializeField]
        private Collider cright;
        [SerializeField]
        private Collider fleft;
        [SerializeField]
        private Collider fright;
        [SerializeField]
        private Collider tleft;
        [SerializeField]
        private Collider tright;
        [SerializeField]
        private Collider hleft;
        [SerializeField]
        private Collider hright;
        [SerializeField]
        private Collider laleft;
        [SerializeField]
        private Collider laright;
        [SerializeField]
        private Collider ualeft;
        [SerializeField]
        private Collider uaright;

        private Rigidbody myRigidBody;
        private CharacterStateController stateController;

        // Use this for initialization

        private void ToggleLM()
        {
            tleft.enabled = !tleft.enabled;
            tright.enabled = !tright.enabled;
            hleft.enabled = !hleft.enabled;
            hright.enabled = !hright.enabled;
            laleft.enabled = !laleft.enabled;
            laright.enabled = !laright.enabled;
            ualeft.enabled = !ualeft.enabled;
            uaright.enabled = !uaright.enabled;
        }

        private void ToggleH()
        {
            cleft.enabled = !cleft.enabled;
            cright.enabled = !cright.enabled;
            fleft.enabled = !fleft.enabled;
            fright.enabled = !fright.enabled;
        }
        private void Start()
        {
            myRigidBody = GetComponent<Rigidbody>();
            stateController = GetComponent<CharacterStateController>();

            cleft.enabled = false;
            cright.enabled = false;
            fleft.enabled = false;
            fright.enabled = false;

            tleft.enabled = false;
            tright.enabled = false;
            hleft.enabled = false;
            hright.enabled = false;
            laleft.enabled = false;
            laright.enabled = false;
            ualeft.enabled = false;
            uaright.enabled = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
            Rigidbody body = collision.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;
            if(stateController.GetCharState() != Enums.CharState.attacking && stateController.GetCharState() != Enums.CharState.blocking)
            {
                //print("hit confirmed");
                float dmg = 0;
                if (Enums.AttackState.light == stateController.GetAttackState()) dmg = 500;
                if (Enums.AttackState.medium == stateController.GetAttackState()) dmg = 700;
                if (Enums.AttackState.heavy == stateController.GetAttackState()) dmg = 850;
                body.GetComponent<CharacterStateController>().TakeDamage(dmg);
            }
        }
    }
}
