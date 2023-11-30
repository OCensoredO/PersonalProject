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
    public bool continuousDamagable;

    [SerializeField]
    public AttackType attackType;
}
