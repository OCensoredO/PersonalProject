using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //private Boss _boss;
    public int score { get; private set; }
    public static int money { get; private set; }
    private float _deltaMoney;

    private delegate void UpdateText(int intVal);
    private UpdateText _updateScoreText;


    void Start()
    {
        score = 0;
        money = 0;
        _deltaMoney = 0;
        _updateScoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>().UpdateScore;
        //_boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }

    void Update()
    {
        //Command command = inputManager.ManageInput();
        //if (command != null) command.Execute(player);
    }

    public void UpdateStatus(int dmg)
    {
        score += dmg;
        //money += dmg / 10;
        _deltaMoney += dmg / 10f;
        Debug.Log((score, _deltaMoney));
        _updateScoreText.Invoke(score);

    }
}
