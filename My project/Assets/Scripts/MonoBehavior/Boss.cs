using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public int hp { get; private set; }
    private float speed;
    private Coroutine patternCoroutine;
    private IEnumerator[] patterns;

    private DataManager dMan;
    private new Renderer renderer;
    private LineRenderer lineRenderer;
    //private GameObject playerObj;

    private FSM<BMsg> bossFSM;

    // hp값은 임시로 지정한 값
    private void Start()
    {
        hp = 50;
        speed = 2f;
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();

        patterns = new IEnumerator[3];
        patterns[0] = shootBeam();
        patterns[1] = generateLaserBox();
        patterns[2] = snipe();
        //playerObj = GameObject.FindGameObjectWithTag("Player");

        // 시연용 기능인 SetColor()를 위해 임시로 선언한 변수
        renderer = GetComponent<Renderer>();
        lineRenderer = GetComponent<LineRenderer>();

        bossFSM = new FSM<BMsg>();
        bossFSM.Start(new IdleBossState(this));

        // 임시
        //StartCoroutine(aim());
    }

    public void Update()
    {
        bossFSM.ManageState();
        //bossFSM.PrintLog();

        if (hp > 20)
        {
            bossFSM.SendMessage(BMsg.EnoughHP);
            return;
        }
        if (hp <= 0)
        {
            bossFSM.SendMessage(BMsg.Die);
            return;
        }
        if (hp < 10)
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

        //patternCoroutine = snipe();
        //StartCoroutine(snipe());
        switch (patternNum)
        {
            case 0:
                Debug.Log("Beam");
                patternCoroutine = StartCoroutine(shootBeam());

                break;
            case 1:
                Debug.Log("Generate LaserBox");
                patternCoroutine = StartCoroutine(generateLaserBox());
                break;
            case 2:
                Debug.Log("Snipe");
                patternCoroutine = StartCoroutine(snipe());
                break;
            default:
                break;
        }
    }

    public void StopPattern()
    {
        if (patternCoroutine is null) return;
        StopCoroutine(patternCoroutine);
    }

    private IEnumerator shootBeam()
    {
        float startTime = Time.time;

        // 패턴 사용 전 이동 속도 임시 변수에 저장, 패턴 사용 중 이동을 잠시 멈추게끔 스피드를 0으로 줌
        float originalSpeed = speed;
        speed = 0f;

        // 빔 프리팹 로드 및 인스턴스 생성
        GameObject beamPrefab = Resources.Load(dMan.gameData.enemyBullets[0].prefab) as GameObject;
        GameObject beamInstance = Instantiate(beamPrefab, transform.position, transform.rotation);

        Ray ray = new Ray();
        ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
        Vector3 targetPos = transform.position + new Vector3(0f, -5f, -0.4f);


        // 일정 시간동안 빔 발사
        while (Time.time - startTime < 2.7f)
        {
            targetPos.y += 2.2f * Time.deltaTime;
            ray.direction = targetPos;
            beamInstance.transform.LookAt(targetPos);

            yield return null;
        }

        // 전진 속도 원래대로 돌려놓기
        speed = originalSpeed;

        // 빔 인스턴스 제거
        Destroy(beamInstance);
        //StopCoroutine(patternCoroutine);
        //yield return null;
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

    private IEnumerator aim()
    {
        float startTime = Time.time;

        // 저격할 좌표, 직전 프레임/현 프레임에서의 플레이어 위치를 저장할 변수 선언
        Vector3 targetPos;
        Vector3 prevPos;
        Vector3 currPos;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        PlayerController pCon = playerObj.GetComponent<PlayerController>();

        prevPos = playerObj.transform.position;
        yield return null;

        // 조준
        while (true)
        {
            // 현 프레임에서의 플레이어 위치 구해서 currPos에 저장
            currPos = playerObj.transform.position;

            // 예측 이동 방향을 구해 플레이어가 이동할 위치 예측
            Vector3 predictedPlayerDirection = (currPos - prevPos).normalized;
            Debug.Log((currPos - prevPos));
            targetPos = predictPosWithGravity(predictedPlayerDirection, playerObj.transform.position, pCon.GetYVel(), pCon.GetPlayerSpeed(), 0.4f);

            // 예측 위치를 추적하는 레이 생성하여 직선으로 렌더링
            Ray ray = new Ray();
            ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
            ray.direction = targetPos;
            renderLine(ray.origin, targetPos);

            // 빔 발사 전까지 일정 시간 대기
            yield return new WaitForFixedUpdate();

            // prevPos 업데이트
            prevPos = currPos;
        }
    }

    private IEnumerator snipe()
    {
        // 코드 개선 필요
        int repeatedCount = 0;
        float startTime = Time.time;

        // 인스턴스 로드
        GameObject beamPrefab = Resources.Load(dMan.gameData.enemyBullets[0].prefab) as GameObject;

        // 저격할 좌표, 직전 프레임/현 프레임에서의 플레이어 위치를 저장할 변수 선언
        Vector3 targetPos;
        Vector3 prevPos;
        Vector3 currPos;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        PlayerController pCon = playerObj.GetComponent<PlayerController>();

        GameObject beamInstance = null;

        prevPos = playerObj.transform.position;
        yield return new WaitForFixedUpdate();

        // 조준-빔 발사 3회 반복
        while (repeatedCount < 3)
        {
            // 현 프레임에서의 플레이어 위치 구해서 currPos에 저장
            currPos = playerObj.transform.position;

            // 예측 이동 방향을 구해 플레이어가 이동할 위치 예측
            Vector3 predictedPlayerDirection = (currPos - prevPos).normalized;
            targetPos = predictPosWithGravity(predictedPlayerDirection, playerObj.transform.position, pCon.GetYVel(), pCon.GetPlayerSpeed(), 0.4f);

            // 예측 위치를 추적하는 레이 생성하여 직선으로 렌더링
            Ray ray = new Ray();
            ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
            ray.direction = targetPos;
            renderLine(ray.origin, targetPos);

            // 빔 발사 전까지 일정 시간 대기
            yield return new WaitForSeconds(0.7f); 

            // 빔 발사
            // 빔 인스턴스 생성 및 크기/회전 조정
            beamInstance = Instantiate(beamPrefab, transform.position, transform.rotation);
            float z = beamInstance.gameObject.transform.localScale.z;
            beamInstance.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, z);
            beamInstance.transform.LookAt(targetPos);

            // 반복 횟수 1회 증가
            repeatedCount++;

            // 일정 시간동안 발사한 빔 지속
            // 이 부분에서 대기하는 시간 때문에 가끔씩 플레이어 위치를 제대로 예측하지 못하는 문제 발생, 추후 수정 필요
            yield return new WaitForSeconds(0.7f);

            startTime = Time.time;
            Destroy(beamInstance);

            // prevPos 업데이트
            prevPos = currPos;
        }

        //Destroy(beamInstance);
        enableLineRenderer(false);
        //StopCoroutine(patternCoroutine);
    }

    public void GetClosertoPlayer()
    {
        transform.Translate(0f, 0f, -speed * Time.deltaTime);
    }

    public Vector3 predictPosWithGravity(Vector3 direction, Vector3 currPos, float currYVel, float movingSpeed, float delaySecond)
    {
        // 주어진 속력과 방향을 기반으로 예측 속도 구하기
        float predictedXVel = movingSpeed * direction.x * Time.fixedDeltaTime;
        float predictedYVel = 0f;
        if (currYVel != 0f) predictedYVel = currYVel + Physics.gravity.y * delaySecond;
        float predictedZVel = movingSpeed * direction.z * Time.fixedDeltaTime;

        Vector3 predictedPlayerPos = currPos + new Vector3(predictedXVel, predictedYVel, predictedZVel) * delaySecond;
        // 땅에 착지할 것으로 예측되는 경우 예측 위치의 y좌표를 0f로 조정
        if (predictedPlayerPos.y < 0f) predictedPlayerPos.y = 0f;

        return predictedPlayerPos;
    }

    // Ray 렌더링용으로 사용하는 함수
    private void renderLine(Vector3 pos0, Vector3 pos1)
    {
        enableLineRenderer(true);
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, pos0);
        lineRenderer.SetPosition(1, pos1);
    }

    private void enableLineRenderer(bool isEnabled) { lineRenderer.enabled = isEnabled; }

    /*
    private int getSign(int value)
    {
        if (value > 0) return 1;
        if (value == 0) return 0;
        return -1;
    }
    */
}