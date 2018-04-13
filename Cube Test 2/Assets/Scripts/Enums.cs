using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resource
{
    class Enums
    {
        public enum CharState
        {
            standing, walkingF, walkingB, crouching, airborn, blocking, attacking, hitstun
        }

        public enum AttackState
        {
            none, light, medium, heavy, special1, special2
		}

        public enum Inputs
        {
            Neutral, Forward, Backward, Up, Down, DownBack, DownForward, Attack
        }

        public enum FacingSide
        {
            P1, P2
        }
    }
}
