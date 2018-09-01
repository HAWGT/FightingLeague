using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class BuffScript : MonoBehaviour
    {

        private Rigidbody creator;

        public void SetCreator(Rigidbody rb)
        {
            this.creator = rb;
            InvokeRepeating("AddHPTick", 0.0f, 0.02f);
        }
        private void AddHPTick()
        {
            creator.GetComponent<CharacterStateController>().FuryBuff(1);
        }
    }
}