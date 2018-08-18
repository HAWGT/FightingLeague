using NeuralNetwork;
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
	[SerializeField]
	private GameObject player1;

	[SerializeField]
	private GameObject player2;


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
            SceneManager.LoadScene("MainMenu");
        }
        if (currentMatch < maxMatches)
        {
            currentMatch++;
            SceneManager.LoadScene("game");
        }

    }

    public bool IsMatchOver()
    {
        return matchEnded;
    }

	public void ChangeAIValue(int playerID, int playerHP)
	{
		switch (playerID)
		{
			case 1:
				player2.GetComponent<NetworkInterface>().ChangeHP(playerID, playerHP);
				break;
			case 2:
				player1.GetComponent<NetworkInterface>().ChangeHP(playerID, playerHP);
				break;
		}
	}

}