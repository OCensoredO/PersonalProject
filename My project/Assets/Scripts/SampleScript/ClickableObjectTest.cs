using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjectTest : MonoBehaviour
{
    void OnMouseDown()
    {
        // Simulate a click on the object
        EventManagerTest.ClickObject(gameObject);
    }
}
