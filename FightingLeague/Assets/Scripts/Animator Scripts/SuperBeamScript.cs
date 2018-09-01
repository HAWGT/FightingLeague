using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class SuperBeamScript : MonoBehaviour
    {

        private Rigidbody creator;

        private Rigidbody target;
        public void SetCreator(Rigidbody rb)
        {
            this.creator = rb;
        }
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
                InvokeRepeating("RemoveHPTick", 0.0f, 0.02f);
            }
        }
        private void RemoveHPTick()
        {
            if (target == null || target.GetComponent<CharacterStateController>() == null) return;
            target.GetComponent<CharacterStateController>().TakeDamage(10, false);
            target.GetComponent<CharacterStateController>().AddSuperBar(0.05f);
        }
    }
}
