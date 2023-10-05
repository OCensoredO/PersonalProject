using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �ϵ��ڵ�
public class Dummy : MonoBehaviour
{
    public int hp = 20;
    public DataManager dMan;
    //private string state = "idle";
    //private BossState state;
    private static BossStateIdle stateIdle = new BossStateIdle();
    private static BossStateMelee stateMelee = new BossStateMelee();
    private static BossStateRemote stateRemote = new BossStateRemote();
    private static BossStateRetreat stateRetreat = new BossStateRetreat();
    Stack<BossState> bossStatesStack;
    //public string nextStateName { get; private set; }

    private void Start()
    {
        bossStatesStack.Push(stateIdle);
        //state = stateIdle;
        //nextStateName = "Idle";
        StartCoroutine(UsePattern());
    }

    public void Update()
    {
        
        if (hp < 0) SceneManager.LoadScene("Main");
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player" && state == "idle") state = "melee";

        if (other.tag != "Bullet") return;
        //hp -= 3;
        hp -= dMan.gameData.bullets[0].damage;
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.tag == "Player" && state == "idle") state = "remote";
    }

    //public string getState() { return state; }

    //public void setState(string state) { this.state = state; }

    //public string getNextState() { return nextState; }

    //public void setNextState(string nextStateName) { this.nextStateName = nextStateName; }

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

    private bool ChangeState()
    {


        /*
        string currStateName = state.GetType().Name;
        if (currStateName.Substring(9) == nextStateName) return false;

        switch(nextStateName)
        {
            case "Idle":
                state = stateIdle;
                break;
            case "Melee":
                state = stateMelee;
                break;
            case "Remote":
                state = stateRemote;
                break;
            case "Retreat":
                state = stateRetreat;
                break;
            default:
                break;
        }
        return true;
        */

        return true;
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
