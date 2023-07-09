using UnityEngine;

[CreateAssetMenu(fileName = "NewPrefabData", menuName = "Prefab Data")]
public class PrefabData : ScriptableObject
{
    [SerializeField]
    private string prefabName;

    [SerializeField]
    private int speed;

    [SerializeField]
    private int damage;
}
