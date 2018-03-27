using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyBindings
{
    public class PotentialKey : MonoBehaviour
    {

        public KeyCode thisKey;
        public Texture2D thisKeyIcon;

        public PotentialKey getThis()
        {
            return this;
        }
    }

}

