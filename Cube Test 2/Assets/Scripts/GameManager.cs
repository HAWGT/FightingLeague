using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    private Transform[] playerTransforms;
    [SerializeField]
    private bool gameEnded = false;
    private GameObject[] allPlayers;
    public static GameManager instance = null;


    private void Start()
    {

        allPlayers = GameObject.FindGameObjectsWithTag("Player");

    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    internal void Die(int playerID)
    {
        gameEnded = true;
    }
}
