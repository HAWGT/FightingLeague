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

    private int secondsLeft = 300;
    private int initTime;

	private int currentMatch = 0;
	private int maxMatches = 0;

    [SerializeField]
    private AudioClip[] musics;

    [SerializeField]
    private GameObject ui;

    private int selectedMusic = 1;

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	private void Start()
	{
        //secondsLeft = PlayerPrefs.GetInt("RoundTime");
        initTime = secondsLeft;

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
        //GetComponent<AudioSource>().clip = musics[1];
        GetComponent<AudioSource>().Play();
        ui.GetComponent<UIManager>().SetTime(secondsLeft);
        InvokeRepeating("TimeTick", 1, 1);

    }

    private void TimeTick()
    {
        secondsLeft--;
        ui.GetComponent<UIManager>().SetTime(secondsLeft);
        if (secondsLeft <= 0) MatchEnd(3);
    }

	public void MatchEnd(int playerID)
    {
        matchEnded = true;
        currentMatch++;
        if(playerID == 3)
        {
            //TIMEOUT
        }
        StartCoroutine(ResetPlayers());
    }

    IEnumerator ResetPlayers()
    {
        yield return new WaitForSeconds(5f);
        secondsLeft = initTime;
        ui.GetComponent<UIManager>().SetTime(secondsLeft);
        matchEnded = false;
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