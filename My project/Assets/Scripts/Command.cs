using UnityEngine;

public abstract class Command
{
    public abstract void Execute(GameObject gameObject);
}

public class MoveCommand : Command
{
    private int moveSpeed;
    private Vector3 direction;

    public MoveCommand(int moveSpeed, Vector3 direction)
    {
        this.moveSpeed = moveSpeed;
        this.direction = direction;
    }

    public override void Execute(GameObject gameObject)
    {
        gameObject.transform.position += direction * moveSpeed * Time.deltaTime;
        gameObject.transform.LookAt(gameObject.transform.position + direction);
    }
}

public class JumpCommand : Command
{
    private int jumpForce;
    private bool isInAir;

    public JumpCommand(int jumpForce, bool isInAir)
    {
        this.jumpForce = jumpForce;
        this.isInAir = isInAir;
    }

    public override void Execute(GameObject gameObject)
    {
        if (isInAir) return;

        Rigidbody rd = gameObject.GetComponent<Rigidbody>();
        rd.AddForce(Vector3.up * jumpForce);
    }
}

public class ShootCommand : Command
{
    private GameObject bulletPrefab;
    private int bulletSpeed;

    public ShootCommand(GameObject bulletPrefab, int bulletSpeed)
    {
        this.bulletPrefab = bulletPrefab;
        this.bulletSpeed = bulletSpeed;
    }

    public override void Execute(GameObject gameObject)
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f), gameObject.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bulletSpeed);
        GameObject.Destroy(bullet, 2.0f);
    }
}