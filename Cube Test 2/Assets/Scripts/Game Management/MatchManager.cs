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

    private int p1w = 0;
    private int p2w = 0;

    [SerializeField]
    private AudioClip[] musics;

    [SerializeField]
    private GameObject ui;

    private int selectedMusic = 1;

    private void Start()
	{
        secondsLeft = PlayerPrefs.GetInt("RoundTime");
        initTime = secondsLeft;

		if(maxMatches != PlayerPrefs.GetInt("GameRounds"))
		{
			maxMatches = PlayerPrefs.GetInt("GameRounds");
		}

        player1.GetComponent<CharacterColliderController>().SetS1SColors(PlayerPrefs.GetInt("P1S1"), PlayerPrefs.GetInt("P1S"));
        player2.GetComponent<CharacterColliderController>().SetS1SColors(PlayerPrefs.GetInt("P2S1"), PlayerPrefs.GetInt("P2S"));
        player1.GetComponent<CharacterStateController>().SetPlayerInputType(PlayerPrefs.GetInt("Player1"));
        player2.GetComponent<CharacterStateController>().SetPlayerInputType(PlayerPrefs.GetInt("Player2"));
        ui.GetComponent<UIManager>().SetState(0);
        ui.GetComponent<UIManager>().SetCount(1, p1w);
        ui.GetComponent<UIManager>().SetCount(2, p2w);

        selectedMusic = PlayerPrefs.GetInt("Music");
        GetComponent<AudioSource>().clip = musics[selectedMusic];
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
		
		if (matchEnded) return;
        matchEnded = true;
        currentMatch++;
        if (playerID == 1)
        {
            ui.GetComponent<UIManager>().SetState(2);
            p2w++;
            ui.GetComponent<UIManager>().SetCount(2, p2w);
            player2.GetComponent<AnimationController>().ResetAnim();
        }
        if (playerID == 2)
        {
            ui.GetComponent<UIManager>().SetState(1);
            p1w++;
            ui.GetComponent<UIManager>().SetCount(1, p1w);
            player1.GetComponent<AnimationController>().ResetAnim();
        }
        if (playerID == 3)
        {
            if (player1.GetComponent<CharacterStateController>().GetHP() > player2.GetComponent<CharacterStateController>().GetHP())
            {
                ui.GetComponent<UIManager>().SetState(1);
                p1w++;
                ui.GetComponent<UIManager>().SetCount(1, p1w);
                player1.GetComponent<AnimationController>().ResetAnim();
                player2.GetComponent<AnimationController>().ResetAnim();
            }
            if (player1.GetComponent<CharacterStateController>().GetHP() < player2.GetComponent<CharacterStateController>().GetHP())
            {
                ui.GetComponent<UIManager>().SetState(2);
                p2w++;
                ui.GetComponent<UIManager>().SetCount(2, p2w);
                player1.GetComponent<AnimationController>().ResetAnim();
                player2.GetComponent<AnimationController>().ResetAnim();
            }
            if (player1.GetComponent<CharacterStateController>().GetHP() == player2.GetComponent<CharacterStateController>().GetHP())
            {
                ui.GetComponent<UIManager>().SetState(3);
                player1.GetComponent<AnimationController>().ResetAnim();
                player2.GetComponent<AnimationController>().ResetAnim();
            }
        }
        StartCoroutine(ResetPlayers());
        player1.GetComponent<CharacterStateController>().FreezeControls();
        player2.GetComponent<CharacterStateController>().FreezeControls();
    }

    IEnumerator ResetPlayers()
    {
        yield return new WaitForSeconds(5f);
        if (maxMatches % 2 == 0)
        {
            if (p1w >= maxMatches || p2w >= maxMatches)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            if (p1w >= maxMatches - 1 || p2w  >= maxMatches - 1)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        secondsLeft = initTime;
        ui.GetComponent<UIManager>().SetTime(secondsLeft);
        matchEnded = false;
        ui.GetComponent<UIManager>().SetState(0);
        player1.GetComponent<CharacterStateController>().ResetP();
        player2.GetComponent<CharacterStateController>().ResetP();
        player1.GetComponent<CharacterStateController>().UnFreezeControls();
        player2.GetComponent<CharacterStateController>().UnFreezeControls();
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