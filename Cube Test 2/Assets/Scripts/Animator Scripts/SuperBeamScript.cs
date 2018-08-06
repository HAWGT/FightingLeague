﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class SuperBeamScript : MonoBehaviour
    {

        private Rigidbody creator;

        private bool flagged = false;
        private Rigidbody target;
        public void SetCreator(Rigidbody rb)
        {
            this.creator = rb;
        }
        /*private void OnTriggerEnter(Collider other)
        {
            Rigidbody body = other.attachedRigidbody;
            if (body == null || body.isKinematic)
            {
                return;
            }
            else if (body != creator)
            {
                if (!flagged)
                {
                    body.GetComponent<CharacterStateController>().TakeDamage(5000);
                    flagged = true;
                }
            }
        }*/
        private void OnTriggerEnter(Collider other)
        {
            Rigidbody body = other.attachedRigidbody;
            if (body == null || body.isKinematic)
            {
                return;
            }
            else if (body != creator)
            {
                target = body;
                InvokeRepeating("RemoveHP", 0.0f, 0.02f);
            }
        }
        private void RemoveHP()
        {
            if (target.GetComponent<CharacterColliderController>() == null) return;
            if (StateHelper.GetState(target) != Enums.AnimState.walkingB)
            {
                target.GetComponent<CharacterStateController>().TakeDamage(8);
                //creator.GetComponent<CharacterStateController>().AddSuperBar(0.08f);
            }
        }
    }
}
