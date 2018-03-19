using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class AnimationController : MonoBehaviour
    {

        [SerializeField]
        private Animator animator;

        private CharacterStateController stateController;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            stateController = GetComponent<CharacterStateController>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}