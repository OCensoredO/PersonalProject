using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI text;
    const string defaultMsg = "Score: ";

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = defaultMsg + 0;
    }

    public void UpdateScore(int score)
    {
        text.text = defaultMsg + score;
    }
}
