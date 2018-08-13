using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{

    // Use this for initialization
    private bool matchEnded = false;
	//public static GameManager instance = null;

	private int currentMatch = 0;
	private int maxMatches = 0;

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	private void Start()
	{
		if(maxMatches != PlayerPrefs.GetInt("GameRounds"))
		{
			maxMatches = PlayerPrefs.GetInt("GameRounds");
		}

		if (currentMatch > maxMatches)
		{
			currentMatch++;
		}
		
	}

	public void MatchEnd(int playerID)
    {
        matchEnded = true;
        if (currentMatch == maxMatches)
        {
            //return to menu
        }
        if (currentMatch < maxMatches)
        {
            currentMatch++;
            //loadlevel
        }

    }

    public bool IsMatchOver()
    {
        return matchEnded;
    }
}