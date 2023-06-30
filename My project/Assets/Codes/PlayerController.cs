using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;

    enum Direction : int
    {
        Forward,
        Backward,
        Left,
        Right
    }

    void Start()
    {
        
    }


    public void Move(int direction)
    {
        switch (direction)
        {
            case (int)Direction.Forward:
                gameObject.transform.position += new Vector3(0.0f, 0.0f, playerSpeed * Time.deltaTime);
                break;
            case (int)Direction.Backward:
                gameObject.transform.position -= new Vector3(0.0f, 0.0f, playerSpeed * Time.deltaTime);
                break;
            case (int)Direction.Left:
                gameObject.transform.position -= new Vector3(playerSpeed * Time.deltaTime, 0.0f, 0.0f);
                break;
            case (int)Direction.Right:
                gameObject.transform.position += new Vector3(playerSpeed * Time.deltaTime, 0.0f, 0.0f);
                break;
            default:
                break;
        }
    }

    public void Shoot()
    {

    }
}
