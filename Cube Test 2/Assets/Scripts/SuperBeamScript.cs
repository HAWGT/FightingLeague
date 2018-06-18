using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class SuperBeamScript : MonoBehaviour
    {

        private Rigidbody creator;

        private bool flagged = false;
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
        private void OnTriggerStay(Collider other)
        {
            Rigidbody body = other.attachedRigidbody;
            if (body == null || body.isKinematic)
            {
                return;
            }
            else if (body != creator)
            {
                if (body.GetComponent<CharacterStateController>().GetCharState() != Enums.CharState.blocking)
                {
                    body.GetComponent<CharacterStateController>().TakeDamage(15);
                }
            }
        }
    }
}
