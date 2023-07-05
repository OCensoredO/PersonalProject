using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;
    PlayerController playerController;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void ManageInput()
    {
        // �յ�, �¿� �̵�
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            playerController.Move(new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")));
        }

        // �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerController.Shoot();
        }

        // «Ǫ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.Jump();
        }
    }
}
