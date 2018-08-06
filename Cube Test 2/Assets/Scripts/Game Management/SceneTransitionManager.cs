using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameData;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SceneTransitionManager : MonoBehaviour {

	private AssetBundle myScenes;
	private string[] scenePaths;
	private string savePath;

	private Player[] players;


	private void Start()
	{
		myScenes = AssetBundle.LoadFromFile("Assets/Scenes");
		scenePaths = myScenes.GetAllScenePaths();
		savePath = Application.persistentDataPath + "/data.dat";
		players = new Player[2];
	}

	private void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(savePath, FileMode.Open);
		bf.Serialize(file, players);
		file.Close();
	}

	public void ManTree()
	{
		players[0].SetType(0);
		players[1].SetType(1);
		Save();
	}

	public void ManNeuron()
	{
		players[0].SetType(0);
		players[1].SetType(2);
		Save();
	}

	public void TreeTree()
	{
		players[0].SetType(1);
		players[1].SetType(1);
		Save();
	}

	public void NeuronTree()
	{
		players[0].SetType(2);
		players[1].SetType(1);
		Save();
	}

	public void NeuronNeuron()
	{
		players[0].SetType(2);
		players[1].SetType(2);
		Save();
	}

	public void ManMan()
	{
		players[0].SetType(0);
		players[1].SetType(0);
		Save();
	}

	public void StartGame()
	{
		SceneManager.LoadScene("game", LoadSceneMode.Additive);
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("game"));
	}
}
