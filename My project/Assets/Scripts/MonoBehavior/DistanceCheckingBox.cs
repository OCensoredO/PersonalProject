using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheckingBox : MonoBehaviour
{
    private GameObject bossObj;

    private void Start()
    {
        bossObj = GameObject.FindGameObjectWithTag("Boss");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (System.Enum.TryParse(tag + "Enter", out BMsg parsedEnum))
            bossObj.SendMessage("SendMessageToFSM", parsedEnum);
            //Debug.Log(parsedEnum);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (System.Enum.TryParse(tag + "Exit", out BMsg parsedEnum))
            bossObj.SendMessage("SendMessageToFSM", parsedEnum);
            //Debug.Log(parsedEnum);
    }
}
