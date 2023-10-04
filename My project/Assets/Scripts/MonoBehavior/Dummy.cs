using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 하드코딩, 고칠 것
public class Dummy : MonoBehaviour
{
    public int hp = 20;
    public DataManager dMan;
    private string state = "idle";

    private void Start()
    {
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

    public string getState() { return state; }

    public void setState(string state) { this.state = state; }

    IEnumerator UsePattern()
    {
        while (true)
        {
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

            /*
            switch (state)
            {
                case "idle":
                    // 일정 시간 후 거리가 멀면 remote 상태로 전환
                    state = "remote";
                    break;
                case "melee":
                    Debug.Log("근거리 패턴 사용");
                    state = "idle";
                    break;
                case "remote":
                    Debug.Log("원거리 패턴 사용");
                    state = "idle";
                    break;
                case "retreat":
                    break;
                default:
                    break;
            }
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
            yield return new WaitForSeconds(4.0f);
        }
    }
}
