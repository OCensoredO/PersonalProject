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

    // hp���� �ӽ÷� ������ ��
    private void Start()
    {
        hp = 20;
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();

        // �ÿ��� ����� SetColor()�� ���� �ӽ÷� ������ ����
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
        // ���� ���� ��, ���� ������ �� �ؾ� �� �۾� ����
        // ���� ���� �ÿ��� Idle�� ����, ���� X
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
                    // idle�� ��, �Ÿ��� �ָ� ������ �����鼭 remote ���·� ����
                    Debug.Log("���� ��...");
                    state = "remote";
                    break;
                case "melee":
                    Debug.Log("�����̼� ����");
                    state = "idle";
                    break;
                case "remote":
                    Debug.Log("�ָ��� ���");
                    state = "idle";
                    break;
                case "retreat":
                    Debug.Log("����!");
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
        Debug.Log("ź�� �Ѹ���1");
        break;
    case 1:
        Debug.Log("ź�� �Ѹ���2");
        break;
    case 2:
        Debug.Log("������");
        break;
    default:
        break;
}
*/
