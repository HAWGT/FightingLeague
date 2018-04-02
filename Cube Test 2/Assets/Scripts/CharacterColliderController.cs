using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterControl
{
    public class CharacterColliderController : MonoBehaviour
    {

        private Rigidbody myRigidBody;
        private CharacterStateController stateController;

        // Use this for initialization
        void Start()
        {
            myRigidBody = GetComponent<Rigidbody>();
            stateController = GetComponent<CharacterStateController>();



        }

        // Update is called once per frame
        void Update()
        {
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false;
                c.isTrigger = false;
                print(c);
            }
            if(stateController.attackState == CharacterStateController.AttackState.light)
            {
                foreach (Collider c in GetComponents<Collider>())
                {
                    if (c.name == "leftArm1_LoResClavical" || c.name == "rightArm1_LoResClavical")
                    {
                        c.enabled = true;
                        c.isTrigger = true;
                    }
                }
            }
            if (stateController.attackState == CharacterStateController.AttackState.medium)
            {
                foreach (Collider c in GetComponents<Collider>())
                {
                    if (c.name == "leftArm1_LoResClavical" || c.name == "rightArm1_LoResClavical")
                    {
                        c.enabled = true;
                        c.isTrigger = true;
                    }
                }
            }
            if (stateController.attackState == CharacterStateController.AttackState.heavy)
            {
                foreach (Collider c in GetComponents<Collider>())
                {
                    if (c.name == "leftLeg1_LoResUpperLeg" || c.name == "rightLeg1_LoResUpperLeg")
                    {
                        c.enabled = true;
                        c.isTrigger = true;
                    }
                }
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
            Rigidbody body = collision.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;
            if(stateController.charState != CharacterStateController.CharState.attacking && stateController.charState != CharacterStateController.CharState.blocking)
            {
                print("hit confirmed");
            }
        }
    }
}
