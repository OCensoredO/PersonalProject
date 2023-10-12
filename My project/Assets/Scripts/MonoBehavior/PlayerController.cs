using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public abstract class State
{
    public abstract void Execute();
}
*/

/*
public class PlayerController : MonoBehaviour
{
    //public Player player;

    public int playerSpeed = 8;
    //public int bulletSpeed = 1000;
    public int jumpForce = 300;

    public bool isInAir;
    public bool isTargetting;

    //public GameObject bulletPrefab;
    public Rigidbody rd;
    public Collider coll;
    public DataManager dataManager;

    private string state = "idle";

    private void Start()
    {
        rd = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponent<Collider>();
        dataManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
        isInAir = false;
        isTargetting = true;
    }

    private void Update()
    {

        // ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInAir) return;
            rd.AddForce(Vector3.up * jumpForce);
            isInAir = true;
            //return new JumpCommand(300, playerController.isInAir);
        }

        // ���� ��� ��ȯ
        if (Input.GetKeyDown(KeyCode.D))
        {
            isTargetting = !isTargetting;
            //return new TurnCommand();
        }

        // �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.F))
        {
            int bulletIndex = 1;
            // int bulletIndex = someBulletIndex;

            // �Ѿ� ������ �ε�
            GameObject bulletPrefab = Resources.Load(dataManager.gameData.bullets[bulletIndex].prefab) as GameObject;
            int bulletSpeed = dataManager.gameData.bullets[bulletIndex].speed;

            // �Ѿ� �߻� ��ġ, ȸ���� ����
            Vector3 bulletPosition = gameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f);
            Quaternion bulletRotation = gameObject.transform.rotation;

            // �Ѿ� instantiate
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition, bulletRotation);

            // �Ѿ� ������
            bullet.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bulletSpeed);

            // ���� �ð� �� �ı�
            Destroy(bullet, 2.0f);
            //return new ShootCommand(bulletPrefab, bulletSpeed);
        }

        // �̵�
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            Vector3 direction;

            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

            if (isTargetting)
            {
                // �� �ٶ󺸱�
                gameObject.transform.LookAt(gameObject.transform.position + new Vector3(0, 0, 1f));
            }
            else
            {
                // �̵��ϴ� ���� �ٶ󺸱�
                gameObject.transform.LookAt(gameObject.transform.position + direction);
            }

            // �̵�
            gameObject.transform.position += direction * playerSpeed * Time.deltaTime;
        }

        // ��ų
        if (Input.GetKeyDown(KeyCode.A))
        {
            // someCurrentSkill.Activate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isInAir = false;
        }
    }

    public void handleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInAir) return;
            rd.AddForce(Vector3.up * jumpForce);
            isInAir = true;
            //return new JumpCommand(300, playerController.isInAir);
        }
        
        //switch(state)
        //{
        //    case "Idle":
        //        break;
        //    case "Jump":
        //        break;
        //    default:
        //        break;
        //}
        
    }
}
*/

/*
public enum PlayerState
{
    Idle,
    Running,
    Jumping
}
*/

public abstract class PlayerState
{

    public abstract void Execute();
}

public class IdlePlayerState : PlayerState
{
    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}

public class JumpPlayerState : PlayerState
{
    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}

public class MovePlayerState : PlayerState
{
    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}

public class PlayerController : MonoBehaviour
{
    //private string currentState;
    private PlayerState state;
    private static IdlePlayerState stateIdle;
    private static JumpPlayerState stateJump;

    private void Start()
    {
        state = stateIdle;
        //currentState = PlayerState.Idle;
    }

    private void Update()
    {
        state = HandleInput();
        state.Execute();
    }

    PlayerState HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Player initiates a jump
            //currentState = PlayerState.Jumping;
            return stateJump;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // Player is running to the right
            //currentState = PlayerState.Running;
        }
        else
        {
            // Player is idle
            //currentState = PlayerState.Idle;
        }
        return null;
    }

    /*
    // Update the player state
    void UpdateState()
    {
        // Perform actions based on the current state
        switch (currentState)
        {
            case PlayerState.Idle:
                IdleState();
                break;
            case PlayerState.Running:
                RunningState();
                break;
            case PlayerState.Jumping:
                JumpingState();
                break;
        }
    }
    */

    // State-specific methods
    void IdleState()
    {
        Debug.Log("Player is idle");
        // Implement idle state behavior
    }

    void RunningState()
    {
        Debug.Log("Player is running");
        // Implement running state behavior
    }

    void JumpingState()
    {
        Debug.Log("Player is jumping");
        // Implement jumping state behavior
    }
}