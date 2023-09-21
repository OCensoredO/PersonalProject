using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    Collider coll;
    Boss boss;

    void Start()
    {
        coll = GetComponent<Collider>();
        boss = GameObject.Find("Dummy").GetComponent<Boss>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("´êÀ½");
        boss.SetState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("³ª°¨");
        boss.SetState(false);
    }
}
