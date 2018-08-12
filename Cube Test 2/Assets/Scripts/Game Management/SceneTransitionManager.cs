using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameData;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour {

	InputField roundsInputField;
	Dropdown timerDropdown;

	private void Start()
	{
		roundsInputField = GameObject.Find("RoundsInputField").GetComponent<InputField>();
		timerDropdown = GameObject.Find("TimerDropdown").GetComponent<Dropdown>();
	}

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

	public void RoundChange()
	{
		PlayerPrefs.SetInt("GameRounds", int.Parse(roundsInputField.text));
	}

	public void TimerChange()
	{
		PlayerPrefs.SetInt("RoundTime", int.Parse(timerDropdown.itemText.text));
		print(timerDropdown.itemText.text);
	}

	public void StartGame()
	{
		SceneManager.LoadScene("game");
	}
	
}
