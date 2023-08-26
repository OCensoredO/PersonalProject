using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;

    //public int playerSpeed = 8;
    //public GameObject bulletPrefab;
    //public int bulletSpeed = 1000;
    //public int jumpForce = 300;
    //public bool isInAir;
    public Rigidbody rd;
    public Collider coll;

    private void Start()
    {
        rd = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponent<Collider>();
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

    public void handleInput()
    {
        switch(player.stateAir)
        {
            case StateAir.STATE_STANDING:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rd.AddForce(Vector3.up * player.jumpForce);
                    player.stateAir = StateAir.STATE_JUMPING;
                }
                break;
            case StateAir.STATE_JUMPING:
                // set player's stateAir to STATE_STANDING when landing on ground
                break;
        }
    }
}
