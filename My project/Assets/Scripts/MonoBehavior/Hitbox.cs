using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private HitboxData hitboxData;
    public HitboxData HitboxData { set { hitboxData = value; } }

    public int GetDmg()                 { return hitboxData.damage; }
    public bool IsContinuousDamagable() { return hitboxData.continuousDamagable; }
    public AttackType GetAttackType()   { return hitboxData.attackType; }
}
