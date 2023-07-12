using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BulletData
{
    public List<Bullet> bullets;
}

public class Bullet
{
    public string name;
    public int speed;
    public int damage;
}