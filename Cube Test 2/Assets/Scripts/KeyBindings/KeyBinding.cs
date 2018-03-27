using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyBindings
{
    public class Keybinding : MonoBehaviour
    {

        public string keyBindingName;
        public KeyCode currentBinding;
        public Texture2D keyIcon;

        void Start()
        {

        }

        private void Update()
        {

        }

        public void AssignKey(PotentialKey key)
        {
            keyIcon = key.thisHeyIcon;
            currentBinding = key.thisKey;
        }

        public bool KeyPressedDown()
        {
            if (Input.GetKeyDown(currentBinding))
            {
                Debug.Log(keyBindingName + " was released");
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
