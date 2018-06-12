using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class FireballScript : MonoBehaviour
    {
        private Rigidbody creator;

        [SerializeField]
        private GameObject explosionPrefab;

        [SerializeField]
        private Collider ownCollider;

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
                ownCollider.enabled = false;
                print("Hit on " + collision.collider.ToString());
                Destroy(gameObject);
                if (body.GetComponent<CharacterStateController>().GetCharState() != Enums.CharState.blocking)
                {
                    body.GetComponent<CharacterStateController>().TakeDamage(1500);
                }
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = body.position;
                var explosion = (GameObject)Instantiate(explosionPrefab, pos, rot);
                Destroy(explosion, 0.25f);
            }
        }

    }
}