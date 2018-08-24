using CharacterControl;
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

    [SerializeField]
    private AudioClip[] musics;

    private int selectedMusic = 1;

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
        selectedMusic = PlayerPrefs.GetInt("Music");
        GetComponent<AudioSource>().clip = musics[selectedMusic];
        GetComponent<AudioSource>().Play();


    }

	public void MatchEnd(int playerID)
    {
        matchEnded = true;
        currentMatch++;
        StartCoroutine(ResetPlayers());
    }

    IEnumerator ResetPlayers()
    {
        yield return new WaitForSeconds(5f);
        player1.GetComponent<CharacterStateController>().ResetP();
        player2.GetComponent<CharacterStateController>().ResetP();
    }

    public bool IsMatchOver()
    {
        return matchEnded;
    }

	public void ChangeAIValue(int playerID, int playerHP, int playerSuper)
	{
		switch (playerID)
		{
			case 1:
				player2.GetComponent<NetworkInterface>().ChangeHP(playerID, playerHP, playerSuper);
				break;
			case 2:
				player1.GetComponent<NetworkInterface>().ChangeHP(playerID, playerHP, playerSuper);
				break;
		}
	}

}