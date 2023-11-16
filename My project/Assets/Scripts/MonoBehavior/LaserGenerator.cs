using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    private int boxLeft;

    void Start()
    {
        boxLeft = 2;
    }

    public void UpdateGeneratorCondition()
    {
        boxLeft--;
        if (boxLeft == 0) Destroy(this.gameObject);
    }
}
