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

    
    private void Start () {
        game = GameObject.Find("Game Manager");

		timer = PlayerPrefs.GetInt("RoundTime");
    }

}
