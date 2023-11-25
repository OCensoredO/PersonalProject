using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const int playerSpeed = 500;
    private const int jumpForce = 800;

    private FSM<PMsg> playerFSM;

    private DataManager dataManager;
    private Rigidbody rd;

    private bool isTargetting;
    //private Vector3 movingDirection;

    private void Start()
    {
        isTargetting = true;
        
        rd = GetComponent<Rigidbody>();
        dataManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();

        playerFSM = new FSM<PMsg>();
        playerFSM.Start(new IdlePlayerState(this));

        //movingDirection = Vector3.zero;
    }

    private void Update()
    {
        playerFSM.ManageState();
        //playerFSM.PrintLog();
    }

    public int GetPlayerSpeed() { return playerSpeed; }
    public float GetYVel() { return rd.velocity.y; }

    public void Move()
    {
        Vector3 direction = setDirection();
        //movingDirection = setDirection();

        // 이동
        //gameObject.transform.position += direction * playerSpeed * Time.deltaTime;
        //rd.AddForce(direction * playerSpeed * Time.deltaTime);
        //rd.velocity = direction * 5; //playerSpeed * Time.deltaTime;
        //rd.velocity = new Vector3(direction.x * 5, rd.velocity.y, direction.z * 5);
        // 이거 FixedUpdated로 빼는 것 고려중
        Vector3 playerVel = direction * playerSpeed / 60;
        rd.velocity = new Vector3(playerVel.x, rd.velocity.y, playerVel.z);
    }

    private Vector3 setDirection()
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

        return direction;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            playerFSM.SendMessage(PMsg.Land);
    }
}