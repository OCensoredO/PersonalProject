using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;
    PlayerController playerController;

    enum Direction : int
    {
        Forward,
        Backward,
        Left,
        Right
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void ManageInput()
    {
        // �յ� �̵�
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //gameManager.movePlayer((int)Direction.FORWARD);
            playerController.Move((int)Direction.Forward);
        }
        else if(Input.GetKey(KeyCode.DownArrow))

        {
            playerController.Move((int)Direction.Backward);
        }
        // �¿� �̵�
        if (Input.GetKey(KeyCode.LeftArrow))

        {
            playerController.Move((int)Direction.Left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))

        {
            playerController.Move((int)Direction.Right);
        }

        // �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Shoot();
        }
    }
}
