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
    //private GameObject playerObj;

    private FSM<BMsg> bossFSM;

    // hp���� �ӽ÷� ������ ��
    private void Start()
    {
        hp = 20;
        speed = 2f;
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
        //playerObj = GameObject.FindGameObjectWithTag("Player");

        // �ÿ��� ����� SetColor()�� ���� �ӽ÷� ������ ����
        renderer = GetComponent<Renderer>();

        bossFSM = new FSM<BMsg>();
        bossFSM.Start(new IdleBossState(this));

        // �ӽ�
        //StartCoroutine(aim());
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
        // �ε������� �ӽ÷� 0���� ��
        hp -= dMan.gameData.bullets[0].damage;
    }

    public void Heal() { hp++; }
    // �ÿ��� ���� �ӽ÷� ��� ����
    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    // �ÿ��� �ӽ� ���
    public void SetColor(Color color)
    {
        renderer.material.color = color;
    }

    public void UseRemotePattern()
    {
        int patternNum = Random.Range(0, 2);

        StartCoroutine(snipe());
        /*
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
        */
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

        // �� �߻�
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

    private IEnumerator snipe()
    {
        // �ڵ� ���� �ʿ�
        int repeatedCount = 0;
        //float timer = 0.5f;
        float startTime = Time.time;
        //float currTime = Time.time;

        GameObject beamPrefab = Resources.Load(dMan.gameData.enemyBullets[0].prefab) as GameObject;
        Vector3? targetPos = null;

        Vector3? prevPos = null;
        Vector3? currPos = null;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        PlayerController pCon = playerObj.GetComponent<PlayerController>();

        GameObject beamInstance = null;

        while (repeatedCount < 3)
        {
            if (!(beamInstance is null)) Destroy(beamInstance);

            if (!prevPos.HasValue)
            {
                prevPos = playerObj.transform.position;
                yield return null;
            }
            currPos = playerObj.transform.position;

            // ���� �̵� ���� ���ϱ�
            Vector3 predictedPlayerDirection = new Vector3();
            predictedPlayerDirection.x = Input.GetAxisRaw("Horizontal");
            predictedPlayerDirection.y = ((Vector3)(currPos - prevPos)).normalized.y;
            predictedPlayerDirection.z = Input.GetAxisRaw("Vertical");

            targetPos = predictPosWithGravity(predictedPlayerDirection, playerObj.transform.position, pCon.GetYVel(), pCon.GetPlayerSpeed(), 0.4f);

            // ���� ��ġ�� �����ϴ� ���� ����
            Ray ray = new Ray();
            ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
            ray.direction = (Vector3)targetPos;

            // ������ϱ� ���� ���̸� �������� ������
            renderLine(ray.origin, (Vector3)targetPos);

            prevPos = playerObj.transform.position;
            yield return new WaitForSeconds(0.7f);

            //Debug.Log("Shoot");
            beamInstance = Instantiate(beamPrefab, transform.position, transform.rotation);
            float z = beamInstance.gameObject.transform.localScale.z;
            beamInstance.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, z);
            beamInstance.transform.LookAt((Vector3)targetPos);
            
            repeatedCount++;
            yield return new WaitForSeconds(0.7f);
            startTime = Time.time;
        }

        Destroy(beamInstance);
        yield return null;
    }

    //Debug.Log(Time.time + ", " + (Time.time - startTime));

    /*
    if (Time.time - startTime < 0.5f)
    {
        //Debug.Log("Aim");
        if (!(beamInstance is null)) Destroy(beamInstance);

        if (!prevPos.HasValue)
        {
            prevPos = playerObj.transform.position;
            yield return null;
        }
        currPos = playerObj.transform.position;

        // ���� �̵� ���� ���ϱ�
        Vector3 predictedPlayerDirection = new Vector3();
        predictedPlayerDirection.x = Input.GetAxisRaw("Horizontal");
        predictedPlayerDirection.y = ((Vector3)(currPos - prevPos)).normalized.y;
        predictedPlayerDirection.z = Input.GetAxisRaw("Vertical");

        targetPos = predictPosWithGravity(predictedPlayerDirection, playerObj.transform.position, pCon.GetYVel(), pCon.GetPlayerSpeed(), 0.3f);

        // ���� ��ġ�� �����ϴ� ���� ����
        Ray ray = new Ray();
        ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
        ray.direction = (Vector3)targetPos;

        // ������ϱ� ���� ���̸� �������� ������
        renderLine(ray.origin, (Vector3)targetPos);

        prevPos = playerObj.transform.position;
        yield return null;
        continue;
    }
    */
    //Debug.Log("Aim");

    private IEnumerator aim()
    {
        // 1ƽ �� ��ġ�� ���� ��ġ�� ���� ���� ��ǥ�� ����
        Vector3? prevPos = null;
        Vector3? currPos = null;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        PlayerController pCon = playerObj.GetComponent<PlayerController>();

        while (true)
        {
            if (!prevPos.HasValue)
            {
                prevPos = playerObj.transform.position;
                yield return null;
            }
            currPos = playerObj.transform.position;

            // ���� �̵� ���� ���ϱ�
            Vector3 predictedPlayerDirection = new Vector3();
            predictedPlayerDirection.x = Input.GetAxisRaw("Horizontal");
            predictedPlayerDirection.y = ((Vector3)(currPos - prevPos)).normalized.y;
            predictedPlayerDirection.z = Input.GetAxisRaw("Vertical");
            
            Vector3 targetPos = predictPosWithGravity(predictedPlayerDirection, playerObj.transform.position, pCon.GetYVel(), pCon.GetPlayerSpeed(), 0.3f);

            // ���� ��ġ�� �����ϴ� ���� ����
            Ray ray = new Ray();
            ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
            ray.direction = targetPos;

            // ������ϱ� ���� ���̸� �������� ������
            renderLine(ray.origin, targetPos);
            
            prevPos = playerObj.transform.position;
            yield return null;
        }
    }

    public void GetClosertoPlayer()
    {
        transform.Translate(0f, 0f, -speed * Time.deltaTime);
    }

    /*
    public Vector3 predictPos(Vector3 playerDirection, float delaySecond)
    {
        // ������ = delaySecond / deltatime
        // �ӷ� = �÷��̾� ���ǵ�(�ϴ� ���� ��)
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
    */

    public Vector3 predictPosWithGravity(Vector3 direction, Vector3 currPos, float currYVel, float movingSpeed, float delaySecond)
    {
        // �־��� �ӷ°� ������ ������� ���� �ӵ� ���ϱ�
        float predictedXVel = movingSpeed * direction.x;
        float predictedYVel = 0f;
        if (currYVel != 0f) predictedYVel = currYVel + Physics.gravity.y * delaySecond;
        float predictedZVel = movingSpeed * direction.z;

        Vector3 predictedPlayerPos = currPos + new Vector3(predictedXVel, predictedYVel, predictedZVel) * delaySecond;
        // ���� ������ ������ �����Ǵ� ��� ���� ��ġ�� y��ǥ�� 0f�� ����
        if (predictedPlayerPos.y < 0f) predictedPlayerPos.y = 0f;

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
        if (value > 0) return 1;
        if (value == 0) return 0;
        return -1;
    }
}