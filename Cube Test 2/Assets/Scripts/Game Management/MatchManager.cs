using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

    // Use this for initialization
    private bool matchEnded = false;
    //public static GameManager instance = null;

    public void MatchEnd(int playerID)
    {
        matchEnded = true;
    }

    public bool IsMatchOver()
    {
        return matchEnded;
    }
}
