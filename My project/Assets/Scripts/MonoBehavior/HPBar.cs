using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public GameObject dummy;

    void Start()
    {
        dummy = GameObject.Find("Dummy");
    }

    void Update()
    {
        //Debug.Log(dummy.GetComponent<Dummy>().hp);
        //Debug.Log(dummy.GetComponent<Dummy>().hp / 20 * 12f);
        // 하드코딩
        transform.localScale = new Vector3(dummy.GetComponent<Dummy>().hp / 20f * 12f, 0.5f, 1f);
    }
}
