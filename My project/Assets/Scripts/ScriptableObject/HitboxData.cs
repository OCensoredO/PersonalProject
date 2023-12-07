using UnityEngine;

public enum AttackType
{
    SBullet,
    MBullet,
    LBullet,
    Melee
}

[CreateAssetMenu(fileName = "NewHitboxData", menuName = "HitboxData")]
public class HitboxData : ScriptableObject
{
    [SerializeField]
    public int damage;

    [SerializeField]
    public int knockBack;

    [SerializeField]
    public AttackType attackType;
}
