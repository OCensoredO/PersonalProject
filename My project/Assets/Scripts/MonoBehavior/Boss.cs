using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �ϵ��ڵ�
public class Boss : MonoBehaviour
{
    public int hp = 20;
    private DataManager dMan;

    private FSM<BMsg> bossFSM;

    private void Start()
    {
        //StartCoroutine(UsePattern());
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();

        bossFSM = new FSM<BMsg>();
        bossFSM.Start(new IdleBossState(this));
    }

    public void Update()
    {
        if (hp <= 0)
            bossFSM.SendMessage(BMsg.Die);
        else if (hp < 5)
            bossFSM.SendMessage(BMsg.LowHP);
        
        //if (hp < 0) SceneManager.LoadScene("Main");
    }

    public void SendMessageToFSM(BMsg msg)
    {
        bossFSM.SendMessage(msg);
        Debug.Log(msg + " ������");
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player" && state == "idle") state = "melee";

        if (other.tag != "Bullet") return;
        // �ε������� �ӽ÷� 0���� ��
        hp -= dMan.gameData.bullets[0].damage;
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.tag == "Player" && state == "idle") state = "remote";
    }


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

}
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
