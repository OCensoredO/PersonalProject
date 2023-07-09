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

            Debug.Log("플레이어 이름: " + playerData.name);
            Debug.Log("플레이어 레벨: " + playerData.level);
            Debug.Log("플레이어 점수: " + playerData.score);
        }
        else
        {
            Debug.LogError("아이고난!" + filePath);
        }
    }
}
