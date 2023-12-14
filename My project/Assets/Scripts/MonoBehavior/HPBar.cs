using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public void UpdateHp(int hp, int maxHp)
    {
        transform.localScale = new Vector3((float)hp / (float)maxHp * 10f, 0.5f, 1f);
    }
}
