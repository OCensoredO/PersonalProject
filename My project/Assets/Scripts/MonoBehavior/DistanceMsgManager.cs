using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMsgManager : MonoBehaviour
{
    private Boss boss;

    private void Start()
    {
        boss = GetComponentInParent<Boss>();
    }

    public void SendMessageToParent()
    {
        //boss.sen
    }

}
