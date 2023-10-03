using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheckingBox : MonoBehaviour
{
    private Dummy dummy;

    private void Start()
    {
        dummy = GetComponentInParent<Dummy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && dummy.state == "idle") dummy.state = "melee";
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && dummy.state == "idle") dummy.state = "remote";
    }
    */
}
