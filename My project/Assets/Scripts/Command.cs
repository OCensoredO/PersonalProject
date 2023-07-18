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
        //gameObject.transform.LookAt(gameObject.transform.position + direction);
        if (direction.z != 0.0f)
        {
            gameObject.transform.LookAt(gameObject.transform.position + new Vector3(0, 0, direction.z));
        }
        else
        {
            gameObject.transform.LookAt(gameObject.transform.position + direction);
        }
    }
}

public class TurnCommand : Command
{
    public override void Execute(GameObject gameObject)
    {
        gameObject.transform.LookAt(gameObject.transform.position + Vector3.forward);
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
        Vector3 bulletPosition = gameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f);
        Quaternion bulletRotation = gameObject.transform.rotation;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletPosition, bulletRotation);
        bullet.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bulletSpeed);
        GameObject.Destroy(bullet, 2.0f);
    }
}