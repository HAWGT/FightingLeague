using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    // Use this for initialization
    private Transform[] playerTransforms;
    [SerializeField]
    private bool gameEnded = false;


    private void Start()
    {

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        playerTransforms = new Transform[allPlayers.Length];
        for (int i = 0; i < allPlayers.Length; i++)
        {
            playerTransforms[i] = allPlayers[i].transform;
        }
        //print(allPlayers[0].gameObject.GetComponent<CharacterStateController>().healthPoints);

    }

    // Update is called once per frame
    void Update () {
        //READ HP
        //if(player[0].healthPoints <= 0 || player[1].healthPoints <= 0) gameEnded = true;
    }
}
