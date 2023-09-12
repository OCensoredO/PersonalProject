using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하드코딩, 고칠 것
public class PlayerController : MonoBehaviour
{
    public Player player;

    public int playerSpeed = 8;
    //public int bulletSpeed = 1000;
    public int jumpForce = 300;

    public bool isInAir;
    public bool is2D;

    //public GameObject bulletPrefab;
    public Rigidbody rd;
    public Collider coll;
    public DataManager dataManager;

    private void Start()
    {
        rd = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponent<Collider>();
        dataManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
        isInAir = false;
        is2D = false;
    }

    private void Update()
    {
        // 짬푸
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInAir) return;
            rd.AddForce(Vector3.up * jumpForce);
            isInAir = true;
            //return new JumpCommand(300, playerController.isInAir);
        }

        // 2차원 이동/3차원 이동 전환
        if (Input.GetKeyDown(KeyCode.R))
        {
            is2D = !is2D;
            //return new TurnCommand();
        }

        // 총알 발사
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 총알 데이터 로드
            GameObject bulletPrefab = Resources.Load(dataManager.gameData.bullets[1].prefab) as GameObject;
            int bulletSpeed = dataManager.gameData.bullets[1].speed;

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
            if (is2D)
            {
                direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);
                gameObject.transform.LookAt(gameObject.transform.position + direction);
            }
            else
            {
                direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
                gameObject.transform.LookAt(gameObject.transform.position + new Vector3(0, 0, direction.z));
            }
            gameObject.transform.position += direction * playerSpeed * Time.deltaTime;
                    //playerController.Move(new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")));
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
        switch(player.stateAir)
        {
            case StateAir.STATE_STANDING:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rd.AddForce(Vector3.up * player.jumpForce);
                    player.stateAir = StateAir.STATE_JUMPING;
                }
                break;
            case StateAir.STATE_JUMPING:
                // set player's stateAir to STATE_STANDING when landing on ground
                break;
        }
    }
}
