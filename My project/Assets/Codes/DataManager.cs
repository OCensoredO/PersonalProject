using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Data.json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(jsonData);

            Debug.Log("�÷��̾� �̸�: " + playerData.name);
            Debug.Log("�÷��̾� ����: " + playerData.level);
            Debug.Log("�÷��̾� ����: " + playerData.score);
        }
        else
        {
            Debug.LogError("���̰�!" + filePath);
        }
    }
}
