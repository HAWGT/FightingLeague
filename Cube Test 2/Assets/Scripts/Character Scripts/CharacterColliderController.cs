using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterColliderController : MonoBehaviour
    {

        [SerializeField]
        private GameObject fireBallPrefab;

        private Transform fireBallSpawn;

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

        private Rigidbody myRigidBody;
        private CharacterStateController stateController;

        // Use this for initialization

        private void SpawnFireBall()
        {
            var fireBall = (GameObject)Instantiate(
            fireBallPrefab,
            fireBallSpawn.position,
            fireBallSpawn.rotation);

            fireBall.GetComponent<Rigidbody>().velocity = fireBall.transform.forward * 6;

            // Destroy the bullet after 2 seconds
            Destroy(fireBall, 1.0f);
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
        }

        private void DisableH()
        {
            cleft.enabled = false;
            cright.enabled = false;
            fleft.enabled = false;
            fright.enabled = false;
            attackingH = false;
        }

        private void Start()
        {
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
                if (attackingL) dmg = 500f;
                if (attackingM) dmg = 700f;
                if (attackingH) dmg = 850f;
                body.GetComponent<CharacterStateController>().TakeDamage(dmg);
                DisableL();
                DisableM();
                DisableH();
            }
        }
    }
}
