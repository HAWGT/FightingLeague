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
            InvokeRepeating("RemoveHPTick", 0.0f, 0.5f);
        }
        private void RemoveHPTick()
        {
            if(!receiver.GetComponent<CharacterStateController>().TakeDamage(25, false))
            {
                Destroy(gameObject);
            }
        }
    }
}
