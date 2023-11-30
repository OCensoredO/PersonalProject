using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    public GameObject stagePrefab;
    private Vector3 offsetPos;

    private void Start()
    {
        offsetPos = new Vector3(0f, 0f, -150f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(transform.parent.position);
            Instantiate(stagePrefab, transform.parent.position + offsetPos, transform.rotation);
            Destroy(gameObject);
        }
    }
}
