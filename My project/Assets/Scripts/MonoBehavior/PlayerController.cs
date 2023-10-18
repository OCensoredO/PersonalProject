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
public enum PState
{
    Idle,
    Move,
    Jump,
    Shoot
}

public enum Msg
{
    Land
}

public abstract class PlayerState
{
    public PlayerController playerController;

    public PlayerState(PlayerController playerController) { this.playerController = playerController; }

    public virtual void HandleInput()
    {
        // 조준 모드 전환
        if (Input.GetKeyDown(KeyCode.D))
            playerController.ToggleTargettingMode();

        // 총알 발사
        if (Input.GetKeyDown(KeyCode.F))
            playerController.SetState(PState.Shoot);
    }

    public virtual void Enter() { return; }
    public virtual void Execute() { return; }
    public virtual void Exit() { return; }
    public virtual void OnMessaged(Msg msg) { return; }
}

public class IdlePlayerState : PlayerState
{
    public IdlePlayerState(PlayerController playerController) : base(playerController) { }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            playerController.SetState(PState.Move);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.SetState(PState.Jump);
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerController.SetState(PState.Shoot);
        }
    }
    /*
    public override void Execute()
    {
        Debug.Log("가만히...");
        //throw new System.NotImplementedException();
    }
    */

    /*
    public override void OnMessaged(string msg)
    {
        return;
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
    */
}

public class MovePlayerState : PlayerState
{
    public MovePlayerState(PlayerController playerController) : base(playerController) { }

    public override void Execute()
    {
        playerController.Move();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.SetState(PState.Jump);
            return;
        }
        if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
        {
            playerController.SetState(PState.Idle);
            return;
        }
    }

    /*
    public override void OnMessaged(string msg)
    {
        switch (msg)
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
    */
}

public class JumpPlayerState : PlayerState
{
    //private const int jumpDuration = 5;
    //private int jumpTimer = 0;

    public JumpPlayerState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {
        playerController.Jump();
    }

    public override void Execute()
    {
        // 공중에서도 앞뒤좌우 이동 가능하게끔 Move() 호출
        //Debug.Log("점프");
        playerController.Move();
    }

    public override void OnMessaged(Msg msg)
    {
        switch (msg)
        {
            case Msg.Land:
                // 착지 시 앞뒤좌우 이동 중이었을 경우
                if (Input.GetAxisRaw("Horizontal") != 0.0f && Input.GetAxisRaw("Vertical") != 0.0f)
                {
                    playerController.SetState(PState.Move);
                    break;
                }
                // 앞뒤좌우 이동 중이지 않았을 경우
                playerController.SetState(PState.Idle);
                break;
        }
    }
}

public class ShootPlayerState : PlayerState
{
    public ShootPlayerState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {
        base.Enter();
        playerController.Shoot();
        playerController.ReturnToPrevState();
    }
}


public class PlayerController : MonoBehaviour
{
    private const int playerSpeed = 8;
    private const int jumpForce = 300;

    public PlayerState prevState { get; private set; }
    private PlayerState currState;

    private static IdlePlayerState stateIdle;
    private static MovePlayerState stateMove;
    private static JumpPlayerState stateJump;
    private static ShootPlayerState stateShoot;

    private DataManager dataManager;
    private Rigidbody rd;

    private bool isTargetting;

    private void Start()
    {
        isTargetting = true;

        stateIdle = new IdlePlayerState(this);
        stateMove = new MovePlayerState(this);
        stateJump = new JumpPlayerState(this);
        stateShoot = new ShootPlayerState(this);

        rd = GetComponent<Rigidbody>();
        dataManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();

        currState = stateIdle;
    }

    private void Update()
    {
        //HandleInput();
        //state = HandleInput();
        currState.HandleInput();
        currState.Execute();
        Debug.Log(currState.GetType().Name);
    }

    public void SetState(PState state)
    {
        prevState = currState;

        switch(state)
        {
            case PState.Idle:
                currState = stateIdle;
                break;
            case PState.Move:
                currState = stateMove;
                break;
            case PState.Jump:
                currState = stateJump;
                break;
            case PState.Shoot:
                currState = stateShoot;
                break;
            default:
                break;
        }
        currState.Enter();
    }

    /*
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
            case "Air":
                state = stateAir;
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
    */

    /*
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
    */

    public void Move()
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

    public void Jump()
    {
        rd.AddForce(jumpForce * Vector3.up);
    }

    public void Shoot()
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
    }

    public void ToggleTargettingMode() { isTargetting = !isTargetting; }

    public void ReturnToPrevState() { currState = prevState; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            currState.OnMessaged(Msg.Land);
    }
}