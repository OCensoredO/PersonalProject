using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTriggerManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision coll)
    {
        Debug.Log(coll.gameObject.tag);
        // Add more checks for other child GameObjects as needed
    }
}
