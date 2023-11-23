using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public int hp { get; private set; }
    private float speed;

    private DataManager dMan;
    private new Renderer renderer;
    private GameObject playerObj;

    private FSM<BMsg> bossFSM;

    // hp값은 임시로 지정한 값
    private void Start()
    {
        hp = 20;
        speed = 2f;
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
        playerObj = GameObject.FindGameObjectWithTag("Player");

        // 시연용 기능인 SetColor()를 위해 임시로 선언한 변수
        renderer = GetComponent<Renderer>();

        bossFSM = new FSM<BMsg>();
        bossFSM.Start(new IdleBossState(this));

        // 임시
        StartCoroutine(aim());
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
        int patternNum = Random.Range(0, 2);
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

    private IEnumerator shootBeam()
    {
        float startTime = Time.time;
        float originalSpeed = speed;

        GameObject beamPrefab = Resources.Load(dMan.gameData.enemyBullets[0].prefab) as GameObject;
        GameObject beamInstance = Instantiate(beamPrefab, transform.position, transform.rotation);

        Ray ray = new Ray();
        ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
        Vector3 targetPos = transform.position + new Vector3(0f, -5f, -0.4f);

        speed = 0f;

        // 빔 발사
        while (Time.time - startTime < 2.7f)
        {
            targetPos.y += 2.2f * Time.deltaTime;
            ray.direction = targetPos;
            //beamInstance.transform.position = ray.origin;
            beamInstance.transform.LookAt(targetPos);

            yield return null;
        }

        speed = originalSpeed;
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

    private IEnumerator aimAndShoot()
    {
        yield return null;
    }

    private IEnumerator aim()
    {
        // 1틱 전 위치와 현재 위치를 비교해 다음 좌표를 예측
        Vector3? prevPos = null;
        Vector3? currPos = null;

        while(true)
        {
            if (!prevPos.HasValue)
            {
                prevPos = playerObj.transform.position;
                yield return null;
            }
            currPos = playerObj.transform.position;

            // 임시 조정
            Vector3 predictedPlayerDirection = ((Vector3)(currPos - prevPos)).normalized;
            predictedPlayerDirection.x = getSign((int)predictedPlayerDirection.x);
            predictedPlayerDirection.z = getSign((int)predictedPlayerDirection.z);

            // 예측 위치를 추적하는 레이 생성
            Ray ray = new Ray();
            ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
            Vector3 targetPos = predictPos(predictedPlayerDirection, 0.3f);
            ray.direction = targetPos;

            // 디버깅하기 위해 레이를 직선으로 렌더링
            renderLine(ray.origin, targetPos);
            
            prevPos = playerObj.transform.position;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void GetClosertoPlayer()
    {
        transform.Translate(0f, 0f, -speed * Time.deltaTime);
    }

    public Vector3 predictPos(Vector3 playerDirection, float delaySecond)
    {
        // 딜레이 = delaySecond / deltatime
        // 속력 = 플레이어 스피드(일단 대충 줌)
        float predictedXVel = 16f * playerDirection.x;

        float predictedYVel = 0f;
        float currYVel = playerObj.GetComponent<Rigidbody>().velocity.y;
        if (currYVel != 0f) predictedYVel = currYVel + Physics.gravity.y * delaySecond;

        float predictedZVel = 16f * playerDirection.z;

        Vector3 predictedPlayerPos = playerObj.transform.position + new Vector3(predictedXVel, predictedYVel, predictedZVel) * delaySecond;
        if (predictedPlayerPos.y < 0f) predictedPlayerPos.y = 0f;
        //return playerObj.transform.position + new Vector3(16f * playerDirection.x * delaySecond, 0, 16f * playerDirection.z * delaySecond);
        return predictedPlayerPos;
    }

    private void renderLine(Vector3 pos0, Vector3 pos1)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, pos0);
        lineRenderer.SetPosition(1, pos1);
    }

    private int getSign(int value)
    {
        int res;

        if (value > 0) res = 1;
        else if (value == 0) res = 0;
        else res = -1;

        return res;
    }
}