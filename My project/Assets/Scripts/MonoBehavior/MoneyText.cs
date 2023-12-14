using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    TextMeshProUGUI text;
    const string defaultMsg = "Money: ";

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = defaultMsg + 0;
    }

    public void UpdateMoney(int money)
    {
        text.text = defaultMsg + money;
    }
}
