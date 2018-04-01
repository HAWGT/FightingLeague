using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    // Use this for initialization
    private Transform[] playerTransforms;
    [SerializeField]
    private bool gameEnded = false;
    private GameObject[] allPlayers;


    private void Start()
    {

        allPlayers = GameObject.FindGameObjectsWithTag("Player");

    }

    // Update is called once per frame
    private void Update () {
        if(allPlayers[0].GetComponent<CharacterControl.CharacterStateController>().healthPoints <= 0 || allPlayers[1].GetComponent<CharacterControl.CharacterStateController>().healthPoints <= 0) gameEnded = true;
    }

    public float[] GetPlayerStatusBar()
    {
        float[] bar;
        bar = new float[4];
        bar[0] = allPlayers[0].GetComponent<CharacterControl.CharacterStateController>().healthPoints;
        bar[1] = allPlayers[0].GetComponent<CharacterControl.CharacterStateController>().superBar;
        bar[2] = allPlayers[1].GetComponent<CharacterControl.CharacterStateController>().healthPoints;
        bar[3] = allPlayers[1].GetComponent<CharacterControl.CharacterStateController>().superBar;
        return bar;
    } 
}
