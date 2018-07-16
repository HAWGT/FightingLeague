using CharacterControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class GroundCheckScript : MonoBehaviour
    {
        [SerializeField]
        GameObject cchar;

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
            cchar.GetComponent<Animator>().applyRootMotion = true;
            cchar.GetComponent<Animator>().SetBool("airborn", false);
            
        }

        public void UpdateSide()
        {
            if ((p1.transform.position.x < p2.transform.position.x) && p1.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2 && p2.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1)
            {
                p1.GetComponent<CharacterStateController>().SetFacingSide(Enums.FacingSide.P1);
                p2.GetComponent<CharacterStateController>().SetFacingSide(Enums.FacingSide.P2);
            }
            else if ((p1.transform.position.x > p2.transform.position.x) && p1.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1 && p2.GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2)
            {
                p1.GetComponent<CharacterStateController>().SetFacingSide(Enums.FacingSide.P2);
                p2.GetComponent<CharacterStateController>().SetFacingSide(Enums.FacingSide.P1);
            }
            cchar.GetComponent<Animator>().applyRootMotion = true;
        }

        private void Update()
        {
            bool wf, wb;
            wf = cchar.GetComponent<Animator>().GetBool("walkingForward");
            wb = cchar.GetComponent<Animator>().GetBool("walkingBackward");
            if (wf == true && wb == true)
            {
                cchar.GetComponent<Animator>().SetBool("walkingForward", false);
                cchar.GetComponent<Animator>().SetBool("walkingBackward", false);
            }
        }
    }

    
}