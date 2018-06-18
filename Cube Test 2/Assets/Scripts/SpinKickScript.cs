using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class SpinKickScript : MonoBehaviour
    {

        private Rigidbody creator;

        private bool flagged = false;

        [SerializeField]
        private GameObject explosionPrefab;

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
                if (body.GetComponent<CharacterStateController>().GetCharState() != Enums.CharState.blocking && !flagged)
                {
                    body.GetComponent<CharacterStateController>().TakeDamage(1500);
                    ContactPoint contact = collision.contacts[0];
                    Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                    Vector3 pos = body.position;
                    var explosion = (GameObject)Instantiate(explosionPrefab, pos, rot);
                    Destroy(explosion, 0.25f);
                    flagged = true;
                }
            }
        }
    }
}