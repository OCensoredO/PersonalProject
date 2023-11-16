using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �ϵ��ڵ�, ��ĥ ��
public class Boss : MonoBehaviour
{
    public int hp = 20;
    public DataManager dMan;
    // ����, �ӽ÷� int�� ����
    private int state = 2;
    public bool isBerserk = false;

    private void Start()
    {
        StartCoroutine(UsePattern());
    }

    public void Update()
    {
        // �ӽ� ���� ����� ����(ü�� ����)
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

    // �ÿ��� �ӽ� ���
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
                // �ٰŸ�
                case 1:
                    Debug.Log("�ٰŸ�");
                    break;
                // ���Ÿ�
                case 2:
                    Debug.Log("���Ÿ�");
                    break;
            }
            
            yield return new WaitForSeconds(4.0f);
        }
    }

    /*
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

public void SetState(bool isMelee)
    {
        // 1 : �ٰŸ�, 2 : ���Ÿ�
        if (isMelee)
            state = 1;
        else
            state = 2;
    }
}