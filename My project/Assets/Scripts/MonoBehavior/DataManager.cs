using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    private string jsonData;
    public BulletData bulletData { get; private set; }

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Data.json");

        if (File.Exists(filePath))
        {
            jsonData = File.ReadAllText(filePath);
            bulletData = JsonConvert.DeserializeObject<BulletData>(jsonData);
        }
        else
        {
            Debug.LogError("아이고난!" + filePath);
        }
    }
}
