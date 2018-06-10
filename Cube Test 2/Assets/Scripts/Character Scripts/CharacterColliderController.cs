using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterColliderController : MonoBehaviour
    {

        [SerializeField]
        private GameObject fireBallPrefab;

        private bool attackingL = false;
        private bool attackingM = false;
        private bool attackingH = false;

        [SerializeField]
        private Collider cleft;
        [SerializeField]
        private Collider cright;
        [SerializeField]
        private Collider fleft;
        [SerializeField]
        private Collider fright;
        [SerializeField]
        private Collider tleft;
        [SerializeField]
        private Collider tright;
        [SerializeField]
        private Collider hleft;
        [SerializeField]
        private Collider hright;
        [SerializeField]
        private Collider laleft;
        [SerializeField]
        private Collider laright;
        [SerializeField]
        private Collider ualeft;
        [SerializeField]
        private Collider uaright;

        [SerializeField]
        private Collider h1;
        [SerializeField]
        private Collider h2;
        [SerializeField]
        private Collider b1;
        [SerializeField]
        private Collider b2;

        private Animator myAnimator;

        private Rigidbody myRigidBody;
        private CharacterStateController stateController;

        // Use this for initialization

        private void SpawnFireBall()
        {
            Vector3 temp = myRigidBody.position;
            temp.y = temp.y + 0.7f;
            float xPos = 0f;
            if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P1) xPos = 0.7f;
            if (GetComponent<CharacterStateController>().GetFacingSide() == Enums.FacingSide.P2) xPos = -0.7f;
            temp.x = temp.x + xPos;
            var fireBall = (GameObject)Instantiate(
            fireBallPrefab,
            temp,
            myRigidBody.rotation);

            fireBall.GetComponent<Rigidbody>().velocity = fireBall.transform.forward * 20;
            fireBall.GetComponent<FireballScript>().SetCreator(myRigidBody);

            // Destroy the bullet after 2 seconds
            Destroy(fireBall, 2.0f);
        }

        private void EnableL()
        {
            tleft.enabled = true;
            tright.enabled = true;
            hleft.enabled = true;
            hright.enabled = true;
            laleft.enabled = true;
            laright.enabled = true;
            ualeft.enabled = true;
            uaright.enabled = true;
            attackingL = true;
        }

        private void EnableM()
        {
            tleft.enabled = true;
            tright.enabled = true;
            hleft.enabled = true;
            hright.enabled = true;
            laleft.enabled = true;
            laright.enabled = true;
            ualeft.enabled = true;
            uaright.enabled = true;
            attackingM = true;
        }

        private void EnableH()
        {
            cleft.enabled = true;
            cright.enabled = true;
            fleft.enabled = true;
            fright.enabled = true;
            attackingH = true;
        }

        private void DisableL()
        {
            tleft.enabled = false;
            tright.enabled = false;
            hleft.enabled = false;
            hright.enabled = false;
            laleft.enabled = false;
            laright.enabled = false;
            ualeft.enabled = false;
            uaright.enabled = false;
            attackingL = false;
            myAnimator.applyRootMotion = true;
    }

        private void DisableM()
        {
            tleft.enabled = false;
            tright.enabled = false;
            hleft.enabled = false;
            hright.enabled = false;
            laleft.enabled = false;
            laright.enabled = false;
            ualeft.enabled = false;
            uaright.enabled = false;
            attackingM = false;
            myAnimator.applyRootMotion = true;
        }

        private void DisableH()
        {
            cleft.enabled = false;
            cright.enabled = false;
            fleft.enabled = false;
            fright.enabled = false;
            attackingH = false;
            myAnimator.applyRootMotion = true;
    }

        private void Start()
        {
            myAnimator = GetComponent<AnimationController>().GetAnimator();
            myRigidBody = GetComponent<Rigidbody>();
            stateController = GetComponent<CharacterStateController>();

            DisableL();
            DisableM();
            DisableH();

        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
            Rigidbody body = collision.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;
            if((attackingL || attackingM || attackingH) && body.GetComponent<CharacterStateController>().GetCharState() != Enums.CharState.blocking)
            {
                //print("hit confirmed");
                float dmg = 0;
                if (attackingL)
                {
                    dmg = 500f;
                }
                if (attackingM)
                {
                    dmg = 700f;
                    myRigidBody.GetComponent<AnimationController>().Push(dmg);
                }
                if (attackingH)
                {
                    dmg = 850f;
                    myRigidBody.GetComponent<AnimationController>().Push(dmg);
                }
                body.GetComponent<CharacterStateController>().TakeDamage(dmg);
                DisableL();
                DisableM();
                DisableH();
            }
        }
    }
}
