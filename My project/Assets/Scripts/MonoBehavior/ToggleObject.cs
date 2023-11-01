using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    //public GameObject objectToToggleOnEnter;
    //public GameObject objectToToggleOnExit;
    public bool onEnter;
    public bool onExit;
    public List<GameObject> objectsToToggle;

    private void OnTriggerEnter(Collider other)
    {
        if (!onEnter) return;
        if (!other.CompareTag("Player")) return;

        ToggleObj();
    }


    private void OnTriggerExit(Collider other)
    {
        if (!onExit) return;
        if (!other.CompareTag("Player")) return;

        ToggleObj();
    }

    private void ToggleObj()
    {
        foreach (var obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
