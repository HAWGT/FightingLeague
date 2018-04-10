using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private Text text;
    private GameObject game;

    // Use this for initialization
    private void Start () {
        game = GameObject.Find("Game Manager");
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	private void Update () {
        float[] bars;
        bars = new float[4];
        bars = game.GetComponent<GameManager>().GetPlayerStatusBar();
        text.text = bars[0].ToString() + "\t" + bars[1].ToString() + "\n" + bars[2].ToString() + "\t" + bars[3].ToString();
        
    }
}
