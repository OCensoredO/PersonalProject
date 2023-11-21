using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PMsg
{
    Land
}


public abstract class PlayerState : IState<PMsg>
{
    protected PlayerController playerController;

    public PlayerState(PlayerController playerController) { this.playerController = playerController; }

    public virtual IState<PMsg> HandleInput()
    {
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
    public virtual IState<PMsg> OnMessaged(PMsg pmsg) { return null; }
}


// ���� �� ���, Ȥ�� ���� �ð� �� ���� ���·� ���ư��� ����
public abstract class PlayerTransientState : PlayerState
{
    protected PlayerState prevState;

    public PlayerTransientState(PlayerController playerController, PlayerState prevState) : base(playerController)
    {
        this.prevState = prevState;
    }
}


public class IdlePlayerState : PlayerState
{
    public IdlePlayerState(PlayerController playerController) : base(playerController) { }

    public override IState<PMsg> HandleInput()
    {
        IState<PMsg> nextState = base.HandleInput();
        if (nextState != null) return nextState;

        if (Input.GetKeyDown(KeyCode.Space))
            return new JumpingPlayerState(playerController, this);

        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
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
        playerController.Move();
    }

    public override IState<PMsg> HandleInput()
    {
        IState<PMsg> nextState = base.HandleInput();
        if (nextState != null) return nextState;

        if (Input.GetKeyDown(KeyCode.Space))
            return new JumpingPlayerState(playerController, this);

        if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
            return new IdlePlayerState(playerController);

        return null;
    }
}


public class JumpingPlayerState : PlayerTransientState
{
    // Shoot ���¿��� Jump ���·� ���� ��, ���߿����� ������ �ϴ� ������ �����ϱ� ���� ���� ���¸� �����
    private PlayerState nextState;

    public JumpingPlayerState(PlayerController playerController, PlayerState prevState) : base(playerController, prevState) { }

    public override IState<PMsg> HandleInput()
    {
        nextState = (PlayerState)base.HandleInput();
        return nextState;
    }


    public override void Enter()
    {
        base.Enter();
        if (nextState is ShootingPlayerState) return;
        playerController.Jump();
    }

    public override void Execute()
    {
        // ���߿����� �յ��¿� �̵� �����ϰԲ� Move() ȣ��
        playerController.Move();
    }
    
    public override IState<PMsg> OnMessaged(PMsg pmsg)
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


public class ShootingPlayerState : PlayerTransientState
{
    public ShootingPlayerState(PlayerController playerController, PlayerState prevState) : base(playerController, prevState) { }

    public override IState<PMsg> HandleInput()
    {
        return prevState;
    }

    public override void Enter()
    {
        base.Enter();
        playerController.Shoot();
    }
}
