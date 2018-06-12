using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class SpinKickScript : MonoBehaviour
    {

        private Rigidbody creator;

        public void SetCreator(Rigidbody rb)
        {
            this.creator = rb;
        }
        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
            Rigidbody body = collision.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
            {
                Destroy(gameObject);
                return;
            }
            else if (body != creator)
            {
                Destroy(gameObject);
                if (body.GetComponent<CharacterStateController>().GetCharState() != Enums.CharState.blocking)
                {
                    body.GetComponent<CharacterStateController>().TakeDamage(1500);
                }
            }
        }
    }
}