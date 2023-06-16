using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject playerObject;
    Vector3 cameraPositionOffset;

    void Start()
    {
        playerObject = GameObject.Find("Player");
        cameraPositionOffset = new Vector3(0.0f, 0.0f, -5.0f);
    }


    void Update()
    {
        //this.transform.LookAt(playerObject.transform);
        this.transform.position = playerObject.transform.position + cameraPositionOffset;
    }
}
