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
        cameraPositionOffset = gameObject.transform.position;
    }


    void Update()
    {
        //this.transform.LookAt(playerObject.transform);
        this.transform.position = cameraPositionOffset + new Vector3(0.0f, 0.0f, playerObject.transform.position.z);
    }
}
