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

        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;
            if(stateController.charState != CharacterStateController.CharState.attacking && stateController.charState != CharacterStateController.CharState.blocking)
            {
                print("hit confirmed");
            }
        }
    }
}
