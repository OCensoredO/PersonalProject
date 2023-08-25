using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerSpeed = 8;
    public GameObject bulletPrefab;
    public int bulletSpeed = 1000;
    public int jumpForce = 300;
    //public bool isInAir;

    private void Start()
    {
        //isInAir = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.tag == "Ground")
        {
            isInAir = false;
        }
        */
    }
}
