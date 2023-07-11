using UnityEngine;

public abstract class Command
{
    public abstract void Execute(GameObject gameObject);
}

public class MoveCommand : Command
{
    public MoveCommand(int moveSpeed, Vector3 direction)
    {
        this.moveSpeed = moveSpeed;
        this.direction = direction;
    }

    private int moveSpeed;
    private Vector3 direction;

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