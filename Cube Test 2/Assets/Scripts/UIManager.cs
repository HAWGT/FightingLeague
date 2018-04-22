using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private Text text;
    private GameObject game;

    private float hp1;
    private float sb1;
    private float hp2;
    private float sb2;

    public void UpdateP1(float a, float b)
    {
        hp1 = a;
        sb1 = b;
		text.text = hp1.ToString() + "\t" + hp2.ToString() + "\n" + sb1.ToString() + "\t" + sb2.ToString();
    }
    public void UpdateP2(float a, float b)
    {
        hp2 = a;
        sb2 = b;
		text.text = hp1.ToString() + "\t" + hp2.ToString() + "\n" + sb1.ToString() + "\t" + sb2.ToString();
    }

    // Use this for initialization
    private void Start () {
        game = GameObject.Find("Game Manager");
        text = GetComponent<Text>();
    }

}
