using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatorGeneric
{
    public class GroundCheckScript : MonoBehaviour
    {
        [SerializeField]
        Animator animator;

        private void OnTriggerEnter(Collider other)
        {
            animator.applyRootMotion = true;
            animator.SetBool("airborn", false);
            animator.SetBool("grounded", true);
        }
    }
    
}