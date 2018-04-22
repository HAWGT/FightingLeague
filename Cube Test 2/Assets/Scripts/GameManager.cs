using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    private bool gameEnded = false;
    //public static GameManager instance = null;


    internal void Die(int playerID)
    {
        gameEnded = true;
    }
}
