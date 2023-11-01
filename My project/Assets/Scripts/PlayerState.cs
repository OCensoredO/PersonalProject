using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PMsg
{
    Land
}

public abstract class PlayerState : State<PMsg>
{
    protected PlayerController playerController;

    public PlayerState(PlayerController playerController) { this.playerController = playerController; }

    public virtual State<PMsg> HandleInput()
    {
        // PlayerController���� ��Ŀ�ø��� ���� ���� ��� ������ PlayerState �ȿ� ���� �ִ� �� �����
        // ���� ��� ��ȯ
        if (Input.GetKeyDown(KeyCode.D))
            playerController.ToggleTargettingMode();

        // �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.F))
            return new ShootingPlayerState(playerController, this);

        return null;
    }

    public virtual void Enter() { return; }
    public virtual void Execute() { return; }
    public virtual void Exit() { return; }
    public virtual State<PMsg> OnMessaged(PMsg pmsg) { return null; }
}

// ���� �� ���� �ð� �� ���� ���·� ���ư��� ����
// ���� ������
public abstract class PlayerTransientState : PlayerState
{
    protected PlayerState prevState;
    //private float startTime;

    public PlayerTransientState(PlayerController playerController, PlayerState prevState) : base(playerController)
    {
        this.prevState = prevState;
    }

    //private void getPrevState() { playerController.SetState(prevState); }
}

public class IdlePlayerState : PlayerState
{
    public IdlePlayerState(PlayerController playerController) : base(playerController) { }

    public override State<PMsg> HandleInput()
    {
        State<PMsg> nextState = base.HandleInput();
        if (nextState != null) return nextState;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerController.SetState(PState.Jump);
            //nextState = new JumpingPlayerState(playerController);
            //return true;
            return new JumpingPlayerState(playerController, this);
        }
        // �� �̵���? �츱 ��� ����غ���
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            //playerController.SetState(PState.Move);
            //nextState = new MovingPlayerState(playerController);
            //return true;
            return new MovingPlayerState(playerController);
        }

        return null;
        //return nextState;
    }
}

public class MovingPlayerState : PlayerState
{
    public MovingPlayerState(PlayerController playerController) : base(playerController) { }

    public override void Execute()
    {
        // �̰� ���� Ŀ�ø��� �Ұ����ҵ�. ���߿� �̵��ӵ� ����/�����ϴ� �� �������� �������� �� �����;� �ҵ���
        playerController.Move();
    }

    public override State<PMsg> HandleInput()
    {
        State<PMsg> nextState = base.HandleInput();
        if (nextState != null) return nextState;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerController.SetState(PState.Jump);
            //return true;
            //nextState = new JumpPlayerState(playerController);
            return new JumpingPlayerState(playerController, this);
        }
        if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
        {
            //playerController.SetState(PState.Idle);
            //return true;
            //nextState = new IdlePlayerState(playerController);
            return new IdlePlayerState(playerController);
        }

        //return false;
        //return nextState;
        return null;
    }
}

public class JumpingPlayerState : PlayerTransientState
{
    public JumpingPlayerState(PlayerController playerController, PlayerState prevState) : base(playerController, prevState) { }

    public override State<PMsg> HandleInput()
    {
        return base.HandleInput();
    }


    public override void Enter()
    {
        base.Enter();
        playerController.Jump();
        //Execute();
    }

    public override void Execute()
    {
        // ���߿����� �յ��¿� �̵� �����ϰԲ� Move() ȣ��
        playerController.Move();
    }
    

    public override State<PMsg> OnMessaged(PMsg pmsg)
    {
        switch (pmsg)
        {
            case PMsg.Land:
                return prevState;
            default:
                return null;
        }
    }
}

// Ʈ������Ʈ ������Ʈ ���� ����
public class ShootingPlayerState : PlayerTransientState
{
    public ShootingPlayerState(PlayerController playerController, PlayerState prevState) : base(playerController, prevState) { }

    public override State<PMsg> HandleInput()
    {
        return prevState;
    }
    public override void Enter()
    {
        base.Enter();
        playerController.Shoot();
        //playerController.ReturnToPrevState();
    }
}
