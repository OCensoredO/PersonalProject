using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastExample : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits something
            if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, LayerMask.GetMask("Default")))
            {
                // Log the name of the object hit
                Debug.Log("Hit: " + hit.transform.name);

                // You can perform additional actions based on the hit object
                // For example, you might apply damage, open a door, etc.
                lineRenderer.SetPosition(0, ray.origin + new Vector3(0f, 0f, 1f));
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                // If the ray doesn't hit anything, set a default endpoint
                lineRenderer.SetPosition(0, ray.origin + new Vector3(0f, 0f, 1f));
                lineRenderer.SetPosition(1, ray.origin + ray.direction * 10.0f); // 10.0f is the default length
            }
        }
    }
}
