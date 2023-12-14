using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUI : MonoBehaviour
{
    TextMeshProUGUI text;
    public string defaultMsg;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = defaultMsg + 0;
    }

    public void UpdateText(int value)
    {
        text.text = defaultMsg + value;
    }
}
