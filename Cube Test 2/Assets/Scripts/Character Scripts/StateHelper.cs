using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class StateHelper : MonoBehaviour
    {
        public static Enums.AnimState GetState(Rigidbody player)
        {
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Light")) return Enums.AnimState.light;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Medium")) return Enums.AnimState.medium;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Heavy")) return Enums.AnimState.heavy;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Special1")) return Enums.AnimState.special1;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Special2")) return Enums.AnimState.special2;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Super")) return Enums.AnimState.super;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Reflect")) return Enums.AnimState.reflect;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MidDash")) return Enums.AnimState.dash;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Vanish")) return Enums.AnimState.vanish;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("GuardBreak")) return Enums.AnimState.grab;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hitstun")) return Enums.AnimState.hitstun;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("WalkingF")) return Enums.AnimState.walkingF;
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("WalkingB")) return Enums.AnimState.walkingB;
            if (player.transform.position.y > 0.26) return Enums.AnimState.airborn;
            return Enums.AnimState.standing;
        }
    }
}