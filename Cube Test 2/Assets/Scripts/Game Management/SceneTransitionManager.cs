using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour {

	[SerializeField]
	private Dropdown dropRoundTime;
	[SerializeField]
	private InputField textRoundNumber;

	public void ManTree()
	{
		PlayerPrefs.SetInt("Player1", 0);
		PlayerPrefs.SetInt("Player2", 1);
	}

	public void ManNeuron()
	{
		PlayerPrefs.SetInt("Player1", 0);
		PlayerPrefs.SetInt("Player2", 2);
	}

	public void TreeTree()
	{
		PlayerPrefs.SetInt("Player1", 1);
		PlayerPrefs.SetInt("Player2", 1);
	}

	public void NeuronTree()
	{
		PlayerPrefs.SetInt("Player1", 2);
		PlayerPrefs.SetInt("Player2", 1);
	}

	public void NeuronNeuron()
	{
		PlayerPrefs.SetInt("Player1", 2);
		PlayerPrefs.SetInt("Player2", 2);
	}

	public void ManMan()
	{
		PlayerPrefs.SetInt("Player1", 0);
		PlayerPrefs.SetInt("Player2", 0);
	}

	public void RoundChange(InputField input)
	{
        PlayerPrefs.SetInt("GameRounds", int.Parse(input.text));
		print(input.text);
	}

	public void SaveOptions()
	{
		PlayerPrefs.SetInt("GameRounds", int.Parse(textRoundNumber.text));
		switch (dropRoundTime.value)
		{
			case 0:
				PlayerPrefs.SetInt("RoundTime", 30);
				break;
			case 1:
				PlayerPrefs.SetInt("RoundTime", 60);
				break;
			case 2:
				PlayerPrefs.SetInt("RoundTime", 99);
				break;

			default:
				PlayerPrefs.SetInt("RoundTime", 30);
				break;
		}
	}

	public void StartGame()
	{
		SceneManager.LoadScene("game");
	}
	
}
