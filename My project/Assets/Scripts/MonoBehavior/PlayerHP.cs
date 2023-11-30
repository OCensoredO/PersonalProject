using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int hp { get; private set; }

    void Start()
    {
        hp = 30;
    }

    public void TakeDamage(int dmg) { hp -= dmg; }
}
