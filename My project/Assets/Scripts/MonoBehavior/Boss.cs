using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 하드코딩, 고칠 것
public class Boss : MonoBehaviour
{
    public int hp = 20;
    public DataManager dMan;
    // 상태, 임시로 int로 지정
    private int state = 2;
    public bool isBerserk = false;

    private void Start()
    {
        StartCoroutine(UsePattern());
    }

    public void Update()
    {
        // 임시 게임 재시작 조건(체력 전소)
        if (hp < 0) SceneManager.LoadScene("Main");
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Bullet") return;
        //hp -= 3;
        hp -= dMan.gameData.bullets[0].damage;
        if (hp < 11) isBerserk = true;
    }

    IEnumerator UsePattern()
    {
<<<<<<< Updated upstream
        while (true)
=======
        SceneManager.LoadScene("Main");
    }

    // 시연용 임시 기능
    public void SetColor(Color color)
    {
        renderer.material.color = color;
    }

    public void UseRemotePattern()
    {
        int patternNum = Random.Range(0, 2);
        switch (patternNum)
>>>>>>> Stashed changes
        {
            int patternNum = Random.Range(0, 3);
            switch(state)
            {
                // 근거리
                case 1:
                    Debug.Log("근거리");
                    break;
                // 원거리
                case 2:
                    Debug.Log("원거리");
                    break;
            }
            
            yield return new WaitForSeconds(4.0f);
        }
    }

    /*
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

public void SetState(bool isMelee)
    {
        // 1 : 근거리, 2 : 원거리
        if (isMelee)
            state = 1;
        else
            state = 2;
    }
}