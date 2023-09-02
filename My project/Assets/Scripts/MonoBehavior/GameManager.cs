using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player { get; private set; }
    //private InputManager inputManager;
    //public float playerSpeed;

    void Start()
    {
        //inputManager = gameObject.GetComponent<InputManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Command command = inputManager.ManageInput();
        //if (command != null) command.Execute(player);
    }
}
