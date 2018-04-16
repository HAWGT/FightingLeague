using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterControl
{
    public class CharacterColliderController : MonoBehaviour
    {

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
            if(stateController.GetAttackState() == CharacterStateController.AttackState.light)
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
            if (stateController.GetAttackState() == CharacterStateController.AttackState.medium)
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
            if (stateController.GetAttackState() == CharacterStateController.AttackState.heavy)
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
