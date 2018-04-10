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
        if (allPlayers[0].GetComponent<CharacterControl.CharacterStateController>().GetHP() <= 0 || allPlayers[1].GetComponent<CharacterControl.CharacterStateController>().GetHP() <= 0) gameEnded = true;
    }

    public float[] GetPlayerStatusBar()
    {
        float[] bar;
        bar = new float[4];
        bar[0] = allPlayers[0].GetComponent<CharacterControl.CharacterStateController>().GetHP();
        bar[1] = allPlayers[0].GetComponent<CharacterControl.CharacterStateController>().GetSB();
        bar[2] = allPlayers[1].GetComponent<CharacterControl.CharacterStateController>().GetHP();
        bar[3] = allPlayers[1].GetComponent<CharacterControl.CharacterStateController>().GetSB();
        return bar;
    }
}
