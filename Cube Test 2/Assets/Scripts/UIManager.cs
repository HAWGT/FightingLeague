using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private Text text;

    // Use this for initialization
    private void Start () {
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	private void Update () {
        float[] bars = GetComponent<GameState>().GetPlayerStatusBar();
	}
}
