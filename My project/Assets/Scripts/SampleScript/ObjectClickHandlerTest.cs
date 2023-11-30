using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClickHandlerTest : MonoBehaviour
{
    void Start()
    {
        // Subscribe to the event in the EventManager
        EventManagerTest.OnObjectClicked += HandleObjectClicked;
    }

    // Function to handle the event
    private void HandleObjectClicked(GameObject clickedObject)
    {
        Debug.Log($"Clicked on object: {clickedObject.name}");
    }
}
