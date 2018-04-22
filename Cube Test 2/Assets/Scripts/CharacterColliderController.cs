using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterControl
{
    public class CharacterColliderController : MonoBehaviour
    {
        [SerializeField]
        private Collider[] colliders;

        [SerializeField]
        private Collider co1;
        [SerializeField]
        private Collider co2;
        [SerializeField]
        private Collider co3;
        [SerializeField]
        private Collider co4;
        [SerializeField]
        private Collider co5;
        [SerializeField]
        private Collider co6;
        [SerializeField]
        private Collider co7;
        [SerializeField]
        private Collider co8;
        [SerializeField]
        private Collider co9;
        [SerializeField]
        private Collider co10;
        [SerializeField]
        private Collider co11;
        [SerializeField]
        private Collider co12;

        private Rigidbody myRigidBody;
        private CharacterStateController stateController;

        // Use this for initialization

        private void ToggleLM()
        {
            co5.enabled = !co5.enabled;
            co6.enabled = !co6.enabled;
            co7.enabled = !co7.enabled;
            co8.enabled = !co8.enabled;
            co9.enabled = !co9.enabled;
            co10.enabled = !co10.enabled;
            co11.enabled = !co11.enabled;
            co12.enabled = !co12.enabled;
        }

        private void ToggleH()
        {
            co1.enabled = !co1.enabled;
            co2.enabled = !co2.enabled;
            co3.enabled = !co3.enabled;
            co4.enabled = !co4.enabled;
        }
        private void Start()
        {
            myRigidBody = GetComponent<Rigidbody>();
            stateController = GetComponent<CharacterStateController>();



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
            if(stateController.GetCharState() != CharacterStateController.CharState.attacking && stateController.GetCharState() != CharacterStateController.CharState.blocking)
            {
                //print("hit confirmed");
                float dmg = 0;
                if (CharacterStateController.AttackState.light == stateController.GetAttackState()) dmg = 500;
                if (CharacterStateController.AttackState.medium == stateController.GetAttackState()) dmg = 700;
                if (CharacterStateController.AttackState.heavy == stateController.GetAttackState()) dmg = 850;
                body.GetComponent<CharacterStateController>().TakeDamage(dmg);
            }
        }
    }
}
