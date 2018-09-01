using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class DebuffScript : MonoBehaviour
    {

        private Rigidbody receiver;

        public void SetReceiver(Rigidbody rb)
        {
            this.receiver = rb;
            InvokeRepeating("RemoveHPTick", 0.0f, 0.02f);
        }
        private void RemoveHPTick()
        {
            receiver.GetComponent<CharacterStateController>().TakeDamage(1, false);
        }
    }
}