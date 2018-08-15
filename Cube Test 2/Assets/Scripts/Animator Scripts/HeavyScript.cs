using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class HeavyScript : MonoBehaviour
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
                if (StateHelper.GetState(body) != Enums.AnimState.walkingB && !flagged)
                {
                    body.GetComponent<CharacterStateController>().TakeDamage(800);
                    body.GetComponent<CharacterStateController>().AddSuperBar(3f);
                    creator.GetComponent<CharacterStateController>().AddSuperBar(6f);
                    flagged = true;
                }
            }
        }
    }
}