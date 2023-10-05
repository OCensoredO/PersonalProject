using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 하드코딩
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
            // 상태 전이 시, 상태 진입할 때 해야 할 작업 수행
            // 게임 시작 시에는 Idle로 시작, 전이 X
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
