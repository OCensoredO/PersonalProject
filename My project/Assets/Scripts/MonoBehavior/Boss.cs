using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 하드코딩
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
        Debug.Log(msg + " 보내짐");
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player" && state == "idle") state = "melee";

        if (other.tag != "Bullet") return;
        // 인덱스값은 임시로 0으로 둠
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
            // 상태 전이 시, 상태 진입할 때 해야 할 작업 수행
            // 게임 시작 시에는 Idle로 시작, 전이 X
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
