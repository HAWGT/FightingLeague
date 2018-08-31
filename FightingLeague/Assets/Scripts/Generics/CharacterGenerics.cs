using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterControl
{
    public class Enums
    {
        public enum AnimState
        {
            standing, walkingF, walkingB, crouching, airborn, blocking, hitstun, crouchBlock, none, light, medium, heavy, special1, special2, super,
            reflect,
            dash,
            vanish,
            grab
        }
        public enum CharState
        {
            standing, walkingF, walkingB, crouching, airborn, blocking, attacking, hitstun, crouchBlock
        }

        public enum AttackState
        {
            none, light, medium, heavy, special1, special2, super
        }

        public enum Inputs
        {
            //this ID represents non-existant state in system
            NullStateID = 0, Up,
            Neutral, Down, DownBack, Back, DownFront, Front, Light, Medium, Heavy, Special1, Special2, Super,
			Vanish, GuardBreak, Dash, Reflect
        }

        public enum FacingSide
        {
            P1, P2
        }

        public enum NumPad
        {
            Left, Down, Right, Up, Neutral
        }

        public enum Transition
        {
            //represents non-existing transition in system
            NullTransition = 0,
            ResetToNeutral, NeutralDown, DownToDB, DBToBack, BackToMedium, DownToDF, DownToNeutral,
            DFToFront, FrontToMedium, FrontToHeavy, DownDown, BDBD, BackBack, DFDF, FrontFront,
			DoubleToDown, DDownToLight,
			DDownToMedium
		}
    }
}
