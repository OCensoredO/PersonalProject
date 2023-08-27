using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �ϵ��ڵ�, ��ĥ ��
public class Dummy : MonoBehaviour
{
    public int hp = 20;
    public DataManager dMan;

    public void Update()
    {
        if (hp < 0) SceneManager.LoadScene("Main");
        dMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground") return;
        Debug.Log("asdf");
        //hp -= 3;
        hp -= dMan.gameData.bullets[0].damage;
    }
}
