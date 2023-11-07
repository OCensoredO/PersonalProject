using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    //private string jsonData;
    public GameData gameData { get; private set; }

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Data.json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            //gameData = JsonConvert.DeserializeObject<GameData>(jsonData);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            Debug.LogError("아이고난!" + filePath);
        }
    }
}
