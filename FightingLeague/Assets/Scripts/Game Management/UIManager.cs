using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private GameObject game;

    [SerializeField]
    private Image hpBar1;
    [SerializeField]
    private Image hpBar2;

	[SerializeField]
	private Image superBar1;
	[SerializeField]
	private Image superBar2;
    [SerializeField]
    private GameObject timerTxt;
    [SerializeField]
    private GameObject statusTxt;
    [SerializeField]
    private GameObject P1RC;
    [SerializeField]
    private GameObject P2RC;

    private int gameState = 0;

	private float hp1;
    private float sb1;
    private float hp2;
    private float sb2;
	private int timer;

    public void UpdateP1(float a, float b)
    {
        hp1 = a;
        sb1 = b;
        hpBar1.fillAmount = a / 10000;
		superBar1.fillAmount = b / 100;
    }
    public void UpdateP2(float a, float b)
    {
        hp2 = a;
        sb2 = b;
        hpBar2.fillAmount = a / 10000;
		superBar2.fillAmount = b / 100;
	}

    public void SetTime(int time)
    {
		if(time < 0)
		{
			time = 0;
		}
		timer = time;
		timerTxt.GetComponent<Text>().text = timer.ToString();
    }

    public void SetState(int state)
    {
        if (state != 0 && state != 1 && state != 2 && state != 3 && state != 4) state = 0;
        if (state == 0)
        {
            statusTxt.GetComponent<Text>().text = "";
        }
        if (state == 1)
        {
            statusTxt.GetComponent<Text>().text = "PLAYER 1 WINS!";
        }
        if (state == 2)
        {
            statusTxt.GetComponent<Text>().text = "PLAYER 2 WINS!";
        }
        if (state == 3)
        {
            statusTxt.GetComponent<Text>().text = "DRAW!";
        }
        if (state == 4)
        {
            statusTxt.GetComponent<Text>().text = "AI TRANING MODE!";
        }
    }

    public void SetCount(int a, int b)
    {
        if (a != 1 && a != 2) return;
        if (a == 1)
        {
            P1RC.GetComponent<Text>().text = b.ToString();
        }
        if (a == 2)
        {
            P2RC.GetComponent<Text>().text = b.ToString();
        }
    }

    
    private void Start () {
        game = GameObject.Find("Game Manager");

		timer = PlayerPrefs.GetInt("RoundTime");
    }

}
