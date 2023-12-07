using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private HitboxData hitboxData;
    public HitboxData HitboxData { set { hitboxData = value; } }

    public int GetDmg()                 { return hitboxData.damage; }
    public int GetKB() { return hitboxData.knockBack; }
    public AttackType GetAttackType()   { return hitboxData.attackType; }
}
