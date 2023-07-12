using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    private Dictionary<KeyCode, bool> inputState = new Dictionary<KeyCode, bool>();
    public DataManager dataManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        dataManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
    }

    public Command ManageInput()
    {
        // «Ǫ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerController.Jump();
            return new JumpCommand(300, playerController.isInAir);
        }

        // �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.F))
        {
            //playerController.Shoot();
            //return null;
            //return new ShootCommand(Resources.Load(, 1000);
        }

        // �յ�, �¿� �̵�
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            //playerController.Move(new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")));
            return new MoveCommand(8, new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")));
        }
        return null;
    }
}
