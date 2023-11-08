using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<Bullet> bullets;
    public List<EnemyBullet> enemyBullets;
}

[System.Serializable]
public class Bullet
{
    public string name;
    public int speed;
    public int damage;
    public string prefab;
}

[System.Serializable]
public class EnemyBullet
{
    public string name;
    public int speed;
    public int damage;
    public string prefab;
}