using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public GameObject boss;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        //boss = GameObject.Find("Boss");
    }

    void Update()
    {
        //Debug.Log(boss.GetComponent<boss>().hp);
        //Debug.Log(boss.GetComponent<boss>().hp / 20 * 12f);
        // 하드코딩
        transform.localScale = new Vector3(boss.GetComponent<Boss>().hp / 20f * 12f, 0.5f, 1f);
    }
}
