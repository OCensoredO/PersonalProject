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

        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInAir) return;
            rd.AddForce(Vector3.up * jumpForce);
            isInAir = true;
            //return new JumpCommand(300, playerController.isInAir);
        }

        // 조준 모드 전환
        if (Input.GetKeyDown(KeyCode.D))
        {
            isTargetting = !isTargetting;
            //return new TurnCommand();
        }

        // 총알 발사
        if (Input.GetKeyDown(KeyCode.F))
        {
            int bulletIndex = 1;
            // int bulletIndex = someBulletIndex;

            // 총알 데이터 로드
            GameObject bulletPrefab = Resources.Load(dataManager.gameData.bullets[bulletIndex].prefab) as GameObject;
            int bulletSpeed = dataManager.gameData.bullets[bulletIndex].speed;

            // 총알 발사 위치, 회전값 설정
            Vector3 bulletPosition = gameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f);
            Quaternion bulletRotation = gameObject.transform.rotation;

            // 총알 instantiate
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition, bulletRotation);

            // 총알 날리기
            bullet.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bulletSpeed);

            // 일정 시간 후 파괴
            Destroy(bullet, 2.0f);
            //return new ShootCommand(bulletPrefab, bulletSpeed);
        }

        // 이동
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            Vector3 direction;

            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

            if (isTargetting)
            {
                // 앞 바라보기
                gameObject.transform.LookAt(gameObject.transform.position + new Vector3(0, 0, 1f));
            }
            else
            {
                // 이동하는 방향 바라보기
                gameObject.transform.LookAt(gameObject.transform.position + direction);
            }

            // 이동
            gameObject.transform.position += direction * playerSpeed * Time.deltaTime;
        }

        // 스킬
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
    public PlayerController playerController;

    public PlayerState(PlayerController playerController) { this.playerController = playerController; }

    public abstract void Execute();

    public abstract void OnMessaged(string msg);
}

public class IdlePlayerState : PlayerState
{
    public IdlePlayerState(PlayerController playerController) : base(playerController) { }

    public override void Execute()
    {
        Debug.Log("가만히...");
        //throw new System.NotImplementedException();
    }

    public override void OnMessaged(string msg)
    {
        switch(msg)
        {
            case "Jump":
                playerController.SetState("Jump");
                break;
            case "Move":
                playerController.SetState("Move");
                break;
            default:
                Debug.Log("Unknown Msg: " + msg);
                break;
        }
    }
}

public class JumpPlayerState : PlayerState
{
    public JumpPlayerState(PlayerController playerController) : base(playerController) { }

    public override void Execute()
    {
        Debug.Log("점프");
    }

    public override void OnMessaged(string msg)
    {
        switch (msg)
        {
            case "Land":
                playerController.SetState("Idle");
                break;
            default:
                Debug.Log("Unknown Msg: " + msg);
                break;
        }
    }
}

public class MovePlayerState : PlayerState
{
    public MovePlayerState(PlayerController playerController) : base(playerController) { }

    public override void Execute()
    {
        Debug.Log("이동");
    }

    public override void OnMessaged(string msg)
    {
        switch(msg)
        {
            case "Stop":
                playerController.SetState("Idle");
                break;
            case "Jump":
                playerController.SetState("Jump");
                break;
            default:
                Debug.Log("Unknown Msg: " + msg);
                break;
        }
    }
}

public class ShootPlayerState : PlayerState
{
    public ShootPlayerState(PlayerController playerController) : base(playerController) { }

    public override void Execute()
    {
        Debug.Log("발사");
        playerController.SetState("Idle");
    }

    public override void OnMessaged(string msg)
    {
        Debug.Log("Unknown Msg: " + msg);
        return;
    }
}

public class PlayerController : MonoBehaviour
{
    //private string currentState;
    private PlayerState state;
    private static IdlePlayerState stateIdle;
    private static JumpPlayerState stateJump;
    private static MovePlayerState stateMove;
    private static ShootPlayerState stateShoot;

    private void Start()
    {
        stateIdle = new IdlePlayerState(this);
        stateJump = new JumpPlayerState(this);
        stateMove = new MovePlayerState(this);
        stateShoot = new ShootPlayerState(this);

        state = stateIdle;
        //currentState = PlayerState.Idle;
    }

    private void Update()
    {
        HandleInput();
        //state = HandleInput();
        state.Execute();
    }

    public void SetState(string stateName)
    {
        switch (stateName)
        {
            case "Idle":
                state = stateIdle;
                break;
            case "Jump":
                state = stateJump;
                break;
            case "Move":
                state = stateMove;
                break;
            case "Shoot":
                state = stateShoot;
                break;
            default:
                Debug.LogError("Unknown state");
                break;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state.OnMessaged("Jump");
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            state.OnMessaged("Move");
            return;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            state.OnMessaged("Stop");
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            state.OnMessaged("Shoot");
        }
    }
}