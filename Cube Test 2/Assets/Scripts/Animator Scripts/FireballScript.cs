using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class FireballScript : MonoBehaviour
    {
        private Rigidbody creator;

        private bool flagged = false;

        [SerializeField]
        private GameObject explosionPrefab;

        public void SetCreator(Rigidbody rb)
        {
            this.creator = rb;
        }

        private void OnTriggerEnter(Collider other)
        {

            Rigidbody body = other.attachedRigidbody;
            if (body == null || body.isKinematic)
            {
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
                Vector3 pos = gameObject.transform.position;
                var explosion = (GameObject)Instantiate(explosionPrefab, pos, rot);
                Destroy(explosion, 0.25f);
                Destroy(gameObject);
                return;
            }
            else if (body != creator)
            {
                if (body.GetComponent<CharacterStateController>() != null)
                {
                    Destroy(gameObject);
                    if (body == null) return;
                    if (body.GetComponent<CharacterColliderController>() == null) return;
                    if (StateHelper.GetState(body) != Enums.AnimState.walkingB && !flagged)
                    {
                        body.GetComponent<CharacterStateController>().TakeDamage(1000);
                        body.GetComponent<CharacterStateController>().AddSuperBar(2f);
                        creator.GetComponent<CharacterStateController>().AddSuperBar(4f);
                        Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
                        Vector3 pos = body.position;
                        var explosion = (GameObject)Instantiate(explosionPrefab, pos, rot);
                        Destroy(explosion, 0.25f);
                        flagged = true;
                    }
                    else if (StateHelper.GetState(body) == Enums.AnimState.walkingB)
                    {
                        body.GetComponent<AnimationController>().BlockFX();
                    }
                } else
                {
                    Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
                    Vector3 pos = body.position;
                    var explosion = (GameObject)Instantiate(explosionPrefab, pos, rot);
                    Destroy(explosion, 0.25f);
                    Destroy(gameObject);
                    return;
                }
            }
        }

    }
}