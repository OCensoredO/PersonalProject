using UnityEngine;

/*
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
*/

/*
public abstract class State
{
    private bool targettingMode;
    private Vector3 direction;
    private int moveSpeed;

    public State(bool targettingMode, Vector3 direction, int moveSpeed)
    {
        this.targettingMode = targettingMode;
        this.direction = direction;
        this.moveSpeed = moveSpeed;
    }

    public State(State state)
    {
        this.targettingMode = state.targettingMode;
        this.direction = state.direction;
        this.moveSpeed = state.moveSpeed;
    }

    public virtual void HandleInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            setDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")));
        }
    }

    private void setDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public virtual void Execute(GameObject gameObject)
    {
        // �̵� �� �ٶ󺸴� ���� ����

        if (targettingMode)
        {
            // �� �ٶ󺸱�
            gameObject.transform.LookAt(gameObject.transform.position + new Vector3(0, 0, 1f));
        }
        else
        {
            // �̵��ϴ� ���� �ٶ󺸱�
            gameObject.transform.LookAt(gameObject.transform.position + direction);
        }

        // �̵�
        gameObject.transform.position += direction * moveSpeed * Time.deltaTime;
    }
}

public class JumpState : State
{
    private int jumpForce;

    public JumpState(bool targettingMode, Vector3 direction, int moveSpeed, int jumpForce)
        : base(targettingMode, direction, moveSpeed)
    {
        this.jumpForce = jumpForce;
    }

    public override void Execute(GameObject gameObject)
    {
        base.Execute(gameObject);
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
    }
}

public class AirState : State
{

    public AirState(bool targettingMode, Vector3 direction, int moveSpeed, int jumpForce)
        : base(targettingMode, direction, moveSpeed) { }

    public AirState(State state) : base(state) { }

    public override void Execute(GameObject gameObject)
    {
        base.Execute(gameObject);
    }
}

public class Player
{
    public int playerSpeed { get; private set; }
    public int jumpForce { get; private set; }
    public bool targettingMode { get; private set; }

    private Vector3 direction;

    private GameObject gameObject;
    private Rigidbody rd;
    private Collider coll;

    private DataManager dataManager;
    private State state;

    public void Initialize(GameObject gameObject, DataManager dataManager)
    {
        // ����Ʈ��
        direction = Vector3.zero;
        state = null;

        playerSpeed = 8;
        jumpForce = 300;
        targettingMode = false;

        this.gameObject = gameObject;
        this.dataManager = dataManager;
        rd = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponent<Collider>();
    }

    public void Update()
    {
        switch(state)
        {
            case JumpState:
                //state = new AirState();
                break;
            case AirState:
                break;
        }

        //state = ManageInput();
        //if (state != null) state.Execute(gameObject);
    }

    public State ManageInput()
    {
        return null;
    }
}
*/