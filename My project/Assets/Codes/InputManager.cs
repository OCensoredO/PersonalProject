using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;

    enum Direction : int
    {
        FORWARD,
        BACKWARD
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void ManageInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            gameManager.movePlayer((int)Direction.FORWARD);
        }
    }
}
