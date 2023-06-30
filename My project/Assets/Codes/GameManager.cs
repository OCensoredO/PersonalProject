using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player { get; private set; }
    private InputManager inputManager;
    //public float playerSpeed;

    enum Direction : int
    {
        FORWARD,
        BACKWARD
    }

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        inputManager.ManageInput();
    }

    /*
    public void movePlayer(int direction)
    {
        switch(direction)
        {
            case (int)Direction.FORWARD:
                player.transform.position += new Vector3(0.0f, 0.0f, playerSpeed * Time.deltaTime);
                break;
        }
    }
    */
}
