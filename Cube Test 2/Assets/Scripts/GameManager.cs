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

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    private void Update()
    {
        if (allPlayers[0].GetComponent<CharacterControl.CharacterStateController>().healthPoints <= 0 || allPlayers[1].GetComponent<CharacterControl.CharacterStateController>().healthPoints <= 0) gameEnded = true;
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
