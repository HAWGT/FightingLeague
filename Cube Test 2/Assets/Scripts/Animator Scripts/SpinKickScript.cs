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
                if (body == null) return;
                if (body.GetComponent<CharacterColliderController>() == null) return;
                if (StateHelper.GetState(body) != Enums.AnimState.walkingB && !flagged)
                {
                    body.GetComponent<CharacterStateController>().TakeDamage(1500);
                    body.GetComponent<CharacterStateController>().AddSuperBar(8f);
                    creator.GetComponent<CharacterStateController>().AddSuperBar(16f);
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