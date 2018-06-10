using CharacterControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatorGeneric
{
    public class GroundCheckScript : MonoBehaviour
    {
        [SerializeField]
        Animator animator;

        [SerializeField]
        GameObject p1;

        [SerializeField]
        GameObject p2;

        private void OnTriggerEnter(Collider other)
        {
            if ((p1.transform.position.x < p2.transform.position.x) && p1.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2 && p2.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1) {
                p1.GetComponent<CharacterStateController>().SetFacingSide(Enums.FacingSide.P1);
                p2.GetComponent<CharacterStateController>().SetFacingSide(Enums.FacingSide.P2);
            } else if((p1.transform.position.x > p2.transform.position.x) && p1.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1 && p2.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                p1.GetComponent<CharacterStateController>().SetFacingSide(Enums.FacingSide.P2);
                p2.GetComponent<CharacterStateController>().SetFacingSide(Enums.FacingSide.P1);
            }
            animator.applyRootMotion = true;
            animator.SetBool("airborn", false);
            
        }
    }
    
}