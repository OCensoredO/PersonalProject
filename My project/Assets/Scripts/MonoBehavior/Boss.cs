using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    private const int maxHp = 300;

    public int hp { get; private set; }
    private float speed;
    private float sprayCool;
    private Coroutine patternCoroutine;
    //private IEnumerator[] patterns;

    private DataManager dMan;
    private new Renderer renderer;
    private LineRenderer lineRenderer;
    //private GameObject playerObj;

    private FSM<BMsg> bossFSM;

    private delegate void UpdateGameStatus(int val);
    private delegate void UpdateHpBar(int val1, int val2);
    private UpdateGameStatus _updateGameStatus;
    private UpdateHpBar _updateHpBar;

    public GameObject fBulletPrefab;

    // hp���� �ӽ÷� ������ ��
    private void Start()
    {
        hp = maxHp;
        speed = 2f;
        sprayCool = 0.17f;
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();

        // �ÿ��� ����� SetColor()�� ���� �ӽ÷� ������ ����
        renderer = GetComponent<Renderer>();
        lineRenderer = GetComponent<LineRenderer>();

        _updateGameStatus = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().UpdateStatus;
        _updateHpBar = GameObject.Find("BossHPBar").GetComponent<HPBar>().UpdateHp;

        bossFSM = new FSM<BMsg>();
        bossFSM.Start(new IdleBossState(this));

        // �ӽ�
        //StartCoroutine(aim());
    }

    public void Update()
    {
        bossFSM.ManageState();
        //bossFSM.PrintLog();

        if (hp > 120)
        {
            bossFSM.SendMessage(BMsg.EnoughHP);
            return;
        }
        if (hp <= 0)
        {
            bossFSM.SendMessage(BMsg.Die);
            return;
        }
        if (hp < 60)
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
        int dmg = dMan.gameData.bullets[0].damage;
        hp -= dmg;

        _updateGameStatus?.Invoke(dmg);
        _updateHpBar?.Invoke(hp, maxHp);
    }

    public void Heal()
    {
        hp += 10;
        sprayCool = 0.05f;
    }
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
        int patternNum = Random.Range(0, 3);

        //StartCoroutine(shootBeam());

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

        // ���� ��� �� �̵� �ӵ� �ӽ� ������ ����, ���� ��� �� �̵��� ��� ���߰Բ� ���ǵ带 0���� ��
        float originalSpeed = speed;
        speed = 0f;

        // �� ������ �ε� �� �ν��Ͻ� ����
        GameObject beamPrefab = Resources.Load(dMan.gameData.enemyBullets[0].prefab) as GameObject;
        GameObject beamInstance = Instantiate(beamPrefab, transform.position, transform.rotation);

        Ray ray = new Ray();
        ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
        Vector3 targetPos = transform.position + new Vector3(0f, -5f, -0.4f);


        // ���� �ð����� �� �߻�
        while (Time.time - startTime < 2.7f)
        {
            targetPos.y += 2.2f * Time.deltaTime;
            ray.direction = targetPos;
            beamInstance.transform.LookAt(targetPos);

            yield return null;
        }

        // ���� �ӵ� ������� ��������
        speed = originalSpeed;

        // �� �ν��Ͻ� ����
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

        // ������ ��ǥ, ���� ������/�� �����ӿ����� �÷��̾� ��ġ�� ������ ���� ����
        Vector3 targetPos;
        Vector3 prevPos;
        Vector3 currPos;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        PlayerController pCon = playerObj.GetComponent<PlayerController>();

        prevPos = playerObj.transform.position;
        yield return null;

        // ����
        while (true)
        {
            // �� �����ӿ����� �÷��̾� ��ġ ���ؼ� currPos�� ����
            currPos = playerObj.transform.position;

            // ���� �̵� ������ ���� �÷��̾ �̵��� ��ġ ����
            Vector3 predictedPlayerDirection = (currPos - prevPos).normalized;
            Debug.Log((currPos - prevPos));
            targetPos = predictPosWithGravity(predictedPlayerDirection, playerObj.transform.position, pCon.GetYVel(), pCon.GetPlayerSpeed(), 0.4f);

            // ���� ��ġ�� �����ϴ� ���� �����Ͽ� �������� ������
            Ray ray = new Ray();
            ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
            ray.direction = targetPos;
            renderLine(ray.origin, targetPos);

            // �� �߻� ������ ���� �ð� ���
            yield return new WaitForFixedUpdate();

            // prevPos ������Ʈ
            prevPos = currPos;
        }
    }

    private IEnumerator snipe()
    {
        // �ڵ� ���� �ʿ�
        int repeatedCount = 0;
        float startTime = Time.time;

        // �ν��Ͻ� �ε�
        GameObject beamPrefab = Resources.Load(dMan.gameData.enemyBullets[0].prefab) as GameObject;

        // ������ ��ǥ, ���� ������/�� �����ӿ����� �÷��̾� ��ġ�� ������ ���� ����
        Vector3 targetPos;
        Vector3 prevPos;
        Vector3 currPos;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        PlayerController pCon = playerObj.GetComponent<PlayerController>();

        GameObject beamInstance = null;

        prevPos = playerObj.transform.position;
        yield return new WaitForFixedUpdate();

        // ����-�� �߻� 3ȸ �ݺ�
        while (repeatedCount < 3)
        {
            if (playerObj == null) break;

            // �� �����ӿ����� �÷��̾� ��ġ ���ؼ� currPos�� ����
            currPos = playerObj.transform.position;

            // ���� �̵� ������ ���� �÷��̾ �̵��� ��ġ ����
            Vector3 predictedPlayerDirection = (currPos - prevPos).normalized;
            targetPos = predictPosWithGravity(predictedPlayerDirection, playerObj.transform.position, pCon.GetYVel(), pCon.GetPlayerSpeed(), 0.4f);

            // ���� ��ġ�� �����ϴ� ���� �����Ͽ� �������� ������
            Ray ray = new Ray();
            ray.origin = transform.position - new Vector3(0f, 0f, transform.localScale.z * 0.51f);
            ray.direction = targetPos;
            renderLine(ray.origin, targetPos);

            // �� �߻� ������ ���� �ð� ���
            yield return new WaitForSeconds(0.7f); 

            // �� �߻�
            // �� �ν��Ͻ� ���� �� ũ��/ȸ�� ����
            beamInstance = Instantiate(beamPrefab, transform.position, transform.rotation);
            float z = beamInstance.gameObject.transform.localScale.z;
            beamInstance.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, z);
            beamInstance.transform.LookAt(targetPos);

            // �ݺ� Ƚ�� 1ȸ ����
            repeatedCount++;

            // ���� �ð����� �߻��� �� ����
            // �� �κп��� ����ϴ� �ð� ������ ������ �÷��̾� ��ġ�� ����� �������� ���ϴ� ���� �߻�, ���� ���� �ʿ�
            yield return new WaitForSeconds(0.7f);

            startTime = Time.time;
            Destroy(beamInstance);

            // prevPos ������Ʈ
            prevPos = currPos;
        }

        enableLineRenderer(false);
    }

    public void StartSpray() { StartCoroutine(Spray()); }

    private IEnumerator Spray()
    {
        while (true)
        {
       

            GameObject fBullet = Instantiate(fBulletPrefab, transform.position, transform.rotation);
            Destroy(fBullet, 10f);

            yield return new WaitForSeconds(sprayCool);
        }
    }

    public void GetClosertoPlayer()
    {
        transform.Translate(0f, 0f, -speed * Time.deltaTime);
    }

    public Vector3 predictPosWithGravity(Vector3 direction, Vector3 currPos, float currYVel, float movingSpeed, float delaySecond)
    {
        // �־��� �ӷ°� ������ ������� ���� �ӵ� ���ϱ�
        float predictedXVel = movingSpeed * direction.x * Time.fixedDeltaTime;
        float predictedYVel = 0f;
        if (currYVel != 0f) predictedYVel = currYVel + Physics.gravity.y * delaySecond;
        float predictedZVel = movingSpeed * direction.z * Time.fixedDeltaTime;

        Vector3 predictedPlayerPos = currPos + new Vector3(predictedXVel, predictedYVel, predictedZVel) * delaySecond;
        // ���� ������ ������ �����Ǵ� ��� ���� ��ġ�� y��ǥ�� 0f�� ����
        if (predictedPlayerPos.y < 0f) predictedPlayerPos.y = 0f;

        return predictedPlayerPos;
    }

    // Ray ������������ ����ϴ� �Լ�
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

}