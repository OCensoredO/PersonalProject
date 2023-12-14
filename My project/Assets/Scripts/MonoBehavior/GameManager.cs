using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score { get; private set; }
    public static int money { get; private set; }
    private float _deltaMoney;

    private delegate void UpdateText(int intVal);
    private UpdateText _updateScoreText;
    private UpdateText _updateMoneyText;

    void Start()
    {
        score = 0;
        money = 0;
        _deltaMoney = 0;
        _updateScoreText = GameObject.Find("ScoreText").GetComponent<TextUI>().UpdateText;
        _updateMoneyText = GameObject.Find("MoneyText").GetComponent<TextUI>().UpdateText;
    }

    public void UpdateStatus(int dmg)
    {
        score += dmg;
        //money += dmg / 10;
        _deltaMoney += dmg / 10f;
        money = (int)_deltaMoney;

        _updateScoreText.Invoke(score);
        _updateMoneyText.Invoke(money);
    }
}

//void Update()
//{
//    //Command command = inputManager.ManageInput();
//    //if (command != null) command.Execute(player);
//}
