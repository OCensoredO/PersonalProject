using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �ϵ��ڵ�, ��ĥ ��
public class Dummy : MonoBehaviour
{
    public int hp = 20;
    public DataManager dMan;
    public string state = "idle";

    private void Start()
    {
        StartCoroutine(UsePattern());
    }

    public void Update()
    {
        switch (state)
        {
            case "idle":
                // ���� �ð� �� �Ÿ��� �ָ� remote ���·� ��ȯ
                state = "remote";
                break;
            case "melee":
                Debug.Log("�ٰŸ� ���� ���");
                state = "idle";
                break;
            case "remote":
                Debug.Log("���Ÿ� ���� ���");
                state = "idle";
                break;
            case "retreat":
                break;
            default:
                break;
        }

        Debug.Log(state);
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

    IEnumerator UsePattern()
    {
        while (true)
        {
            /*
            switch (state)
            {
                case "idle":
                    // ���� �ð� �� �Ÿ��� �ָ� remote ���·� ��ȯ
                    state = "remote";
                    break;
                case "melee":
                    Debug.Log("�ٰŸ� ���� ���");
                    state = "idle";
                    break;
                case "remote":
                    Debug.Log("���Ÿ� ���� ���");
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
            yield return new WaitForSeconds(4.0f);
        }
    }
}
