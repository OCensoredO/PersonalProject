using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ÇÏµåÄÚµù, °íÄ¥ °Í
public class Dummy : MonoBehaviour
{
    public int hp = 20;
    public DataManager dMan;

    private void Start()
    {
        StartCoroutine(UsePattern());
    }

    public void Update()
    {
        if (hp < 0) SceneManager.LoadScene("Main");
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground") return;
        //hp -= 3;
        hp -= dMan.gameData.bullets[0].damage;
    }

    IEnumerator UsePattern()
    {
        while (true)
        {
            int patternNum = Random.Range(0, 3);
            switch (patternNum)
            {
                case 0:
                    Debug.Log("Åº¸· »Ñ¸®±â1");
                    break;
                case 1:
                    Debug.Log("Åº¸· »Ñ¸®±â2");
                    break;
                case 2:
                    Debug.Log("·¹ÀÌÀú");
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(4.0f);
        }
    }
}
