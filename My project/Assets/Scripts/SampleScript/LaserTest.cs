using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTest : MonoBehaviour
{
    public GameObject laserPrefab;

    private GameObject laser;
    private LineRenderer lineRenderer;
    private Ray ray;
    private Vector3 target;

    private void Start()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        

        ray = new Ray();
        ray.origin = transform.position + new Vector3(0f, 0f, 1f);
        target = new Vector3(0f, -3f, 1f);

        laser = Instantiate(laserPrefab, transform.position, transform.rotation);
        laser.transform.localScale = new Vector3(1f, 1f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        target.y += 0.003f;
        ray.direction = target;
        laser.transform.position = ray.origin;

        //laser.transform.rotation = Quaternion.LookRotation(ray.direction);
        laser.transform.LookAt(target, Vector3.up);
        //laser.transform.rotation = Quaternion.LookRotation(target - laser.transform.position, Vector3.forward);

        lineRenderer.SetPosition(0, ray.origin + new Vector3(0f, 0f, 1f));
        lineRenderer.SetPosition(1, target);
    }
}
