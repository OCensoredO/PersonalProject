using UnityEngine;

public enum StateAir
{
    STATE_STANDING,
    STATE_JUMPING
}

public enum StateDim
{
    STATE_2D,
    STATE_3D
}

public class Player
{
    public int playerSpeed;
    public GameObject bulletPrefab;
    public int bulletSpeed;
    public int jumpForce;
    public StateAir stateAir;
    public StateDim stateDim;
}
