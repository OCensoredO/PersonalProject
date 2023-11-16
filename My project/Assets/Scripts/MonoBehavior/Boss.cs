using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public int hp { get; private set; }
    private DataManager dMan;
    new Renderer renderer;

    private FSM<BMsg> bossFSM;

    // hp값은 임시로 지정한 값
    private void Start()
    {
        hp = 20;
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();

        // 시연용 기능인 SetColor()를 위해 임시로 선언한 변수
        renderer = GetComponent<Renderer>();

        bossFSM = new FSM<BMsg>();
        bossFSM.Start(new IdleBossState(this));
    }

    public void Update()
    {
        bossFSM.ManageState();
        //bossFSM.PrintLog();

        if (hp > 12)
        {
            bossFSM.SendMessage(BMsg.EnoughHP);
            return;
        }
        if (hp <= 0)
        {
            bossFSM.SendMessage(BMsg.Die);
            return;
        }
        if (hp < 8)
        {
            bossFSM.SendMessage(BMsg.LowHP);
        }
    }

    public void SendMessageToFSM(BMsg msg)
    {
        bossFSM.SendMessage(msg);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Bullet") return;
        // 인덱스값은 임시로 0으로 둠
        hp -= dMan.gameData.bullets[0].damage;
    }

    public void Heal() { hp++; }
    // 시연을 위해 임시로 기능 구현
    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    // 시연용 임시 기능
    public void SetColor(Color color)
    {
        renderer.material.color = color;
    }

    public void UseRemotePattern()
    {
        int patternNum = Random.Range(0, 3);
        switch (patternNum)
        {
            case 0:
                Debug.Log("Beam");
                StartCoroutine(shootBeam());
                break;
            case 1:
                Debug.Log("Generate LaserBox");
                StartCoroutine(generateLaserBox());
                break;
            default:
                break;
        }
    }

    public void ShootBeam()
    {
        //GameObject beamPrefab = Resources.Load(dMan.gameData.enemyBullets[0].prefab) as GameObject;
        //GameObject beamInstance;
        //beamInstance = Instantiate(beamPrefab, transform.position, transform.rotation);
        //beamInstance.transform.parent = transform;
        //Debug.Log(beamInstance.transform.parent.name);
        //Instantiate(beamInstance);

        //StartCoroutine(shootBeam());
        StartCoroutine(generateLaserBox());
    }

    private IEnumerator shootBeam()
    {
        float startTime = Time.time;

        GameObject beamPrefab = Resources.Load(dMan.gameData.enemyBullets[0].prefab) as GameObject;
        GameObject beamInstance = Instantiate(beamPrefab, transform.position, transform.rotation);

        Ray ray = new Ray();
        ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
        Vector3 targetPos = transform.position + new Vector3(0f, -5f, -0.4f);

        while (Time.time - startTime < 2.7f)
        {
            targetPos.y += 2.2f * Time.deltaTime;
            ray.direction = targetPos;
            //beamInstance.transform.position = ray.origin;
            beamInstance.transform.LookAt(targetPos);

            yield return null;
        }

        Destroy(beamInstance);
        yield return null;
    }

    private IEnumerator generateLaserBox()
    {
        GameObject LaserGeneratorPrefab = Resources.Load(dMan.gameData.enemyBullets[1].prefab) as GameObject;
        GameObject LaserGeneratorInstance = Instantiate(LaserGeneratorPrefab, transform.position, transform.rotation);

        float generatorPosY = Random.Range(-8f, 0.5f);
        LaserGeneratorInstance.transform.Translate(0f, generatorPosY, 0f);

        Destroy(LaserGeneratorInstance, 20.0f);

        while(LaserGeneratorInstance != null)
        {
            LaserGeneratorInstance.transform.Translate(0f, 0f, -12f * Time.deltaTime);
            yield return null;
        }

        yield return null;
    }
}
/*
IEnumerator UsePattern()
{
    while (true)
    {
        // 상태 전이 시, 상태 진입할 때 해야 할 작업 수행
        // 게임 시작 시에는 Idle로 시작, 전이 X
        //if (ChangeState())  state.Enter();
        //state.Execute();

        yield return new WaitForSeconds(4.0f);
    }

}
*/

/*
            switch (state)
            {
                case "idle":
                    // idle일 때, 거리가 멀면 동작이 끝나면서 remote 상태로 전이
                    Debug.Log("쉬는 중...");
                    state = "remote";
                    break;
                case "melee":
                    Debug.Log("가까이서 공격");
                    state = "idle";
                    break;
                case "remote":
                    Debug.Log("멀리서 사격");
                    state = "idle";
                    break;
                case "retreat":
                    Debug.Log("후퇴!");
                    break;
                default:
                    break;
            }

            Debug.Log(state);
            */

/*
int patternNum = Random.Range(0, 3);
switch (patternNum)
{
    case 0:
        Debug.Log("탄막 뿌리기1");
        break;
    case 1:
        Debug.Log("탄막 뿌리기2");
        break;
    case 2:
        Debug.Log("레이저");
        break;
    default:
        break;
}
*/
