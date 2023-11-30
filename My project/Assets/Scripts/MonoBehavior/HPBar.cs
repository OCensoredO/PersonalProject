using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public GameObject boss;
    float bossMaxHP;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossMaxHP = boss.GetComponent<Boss>().hp;
    }

    void Update()
    {
        //Debug.Log(boss.GetComponent<boss>().hp);
        //Debug.Log(boss.GetComponent<boss>().hp / 20 * 12f);
        float bossHP = boss.GetComponent<Boss>().hp;
        transform.localScale = new Vector3(bossHP / bossMaxHP * 10f, 0.5f, 1f);
    }
}
