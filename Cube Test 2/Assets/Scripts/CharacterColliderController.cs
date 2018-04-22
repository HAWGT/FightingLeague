using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterControl
{
    public class CharacterColliderController : MonoBehaviour
    {

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

        private Rigidbody myRigidBody;
        private CharacterStateController stateController;

        // Use this for initialization

        private void ToggleLM()
        {
            tleft.enabled = !tleft.enabled;
            tright.enabled = !tright.enabled;
            hleft.enabled = !hleft.enabled;
            hright.enabled = !hright.enabled;
            laleft.enabled = !laleft.enabled;
            laright.enabled = !laright.enabled;
            ualeft.enabled = !ualeft.enabled;
            uaright.enabled = !uaright.enabled;
        }

        private void ToggleH()
        {
            cleft.enabled = !cleft.enabled;
            cright.enabled = !cright.enabled;
            fleft.enabled = !fleft.enabled;
            fright.enabled = !fright.enabled;
        }
        private void Start()
        {
            myRigidBody = GetComponent<Rigidbody>();
            stateController = GetComponent<CharacterStateController>();



        }

        // Update is called once per frame
        private void Update()
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
            if(stateController.GetCharState() != CharacterStateController.CharState.attacking && stateController.GetCharState() != CharacterStateController.CharState.blocking)
            {
                //print("hit confirmed");
                float dmg = 0;
                if (CharacterStateController.AttackState.light == stateController.GetAttackState()) dmg = 500;
                if (CharacterStateController.AttackState.medium == stateController.GetAttackState()) dmg = 700;
                if (CharacterStateController.AttackState.heavy == stateController.GetAttackState()) dmg = 850;
                body.GetComponent<CharacterStateController>().takeDamage(dmg);
            }
        }
    }
}
