using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerTest : MonoBehaviour
{
    // Define a delegate with a GameObject parameter
    public delegate void GameObjectDelegate(GameObject go);

    // Declare an event using the delegate
    public static event GameObjectDelegate OnObjectClicked;

    // Function to simulate an event trigger
    public static void ClickObject(GameObject clickedObject)
    {
        // Trigger the event with the provided GameObject
        OnObjectClicked?.Invoke(clickedObject);
    }
}
