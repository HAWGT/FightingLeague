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


            //DISABLE ALL COLLIDERS
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false;
            }
            //DISABLE ALL COLLIDERS



        }

        // Update is called once per frame
        void Update()
        {

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
