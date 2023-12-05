using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text += "Hello";
    }

    // Update is called once per frame
    void Update()
    {
        //text.text += 
    }

    public void UpdateScore(int score)
    {

    }
}
