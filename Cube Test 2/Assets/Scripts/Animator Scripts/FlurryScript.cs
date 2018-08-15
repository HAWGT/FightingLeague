﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class FlurryScript : MonoBehaviour
    {
        private Rigidbody creator;

        private bool flagged = false;
        public void SetCreator(Rigidbody rb)
        {
            this.creator = rb;
        }
        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody body = collision.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
            {
                Destroy(gameObject);
                return;
            }
            else if (body != creator)
            {
                Destroy(gameObject);
                if (target == null) return;
                if (body.GetComponent<CharacterColliderController>() == null) return;
                if (!flagged)
                {
                    body.GetComponent<CharacterStateController>().TakeDamage(1000);
                    body.GetComponent<CharacterStateController>().AddSuperBar(10f);
                    creator.GetComponent<CharacterStateController>().AddSuperBar(20f);
                    flagged = true;
                }
            }
        }
    }
}