using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PState
{
    Idle,
    Move,
    Jump,
    Shoot
}

public enum Msg
{
    Land
}

public abstract class PlayerState
{
    public PlayerController playerController;

    public PlayerState(PlayerController playerController) { this.playerController = playerController; }

    public virtual PlayerState HandleInput()
    {
        // ���� ��� ��ȯ
        if (Input.GetKeyDown(KeyCode.D))
            playerController.ToggleTargettingMode();

        // �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.F))
            return new ShootPlayerState(playerController);
        //playerController.SetState(PState.Shoot);

        return null;
    }

    public virtual void Enter() { return; }
    public virtual void Execute() { return; }
    public virtual void Exit() { return; }
    public virtual void OnMessaged(Msg msg) { return; }
}

// ���� �� ���� �ð� �� ���� ���·� ���ư��� ����
public abstract class PlayerTransientState : PlayerState
{
    protected PlayerState prevState;
    //private float startTime;

    public PlayerTransientState(PlayerController playerController) : base(playerController) { }

    //private void getPrevState() { playerController.SetState(prevState); }
}

public class IdlePlayerState : PlayerState
{
    public IdlePlayerState(PlayerController playerController) : base(playerController) { }

    public override PlayerState HandleInput()
    {
        PlayerState nextState;

        nextState = base.HandleInput();

        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            //playerController.SetState(PState.Move);
            nextState = new MovePlayerState(playerController);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerController.SetState(PState.Jump);
            nextState = new JumpPlayerState(playerController);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //playerController.SetState(PState.Shoot);
            nextState = new ShootPlayerState(playerController);
        }

        return nextState;
    }
}

public class MovePlayerState : PlayerState
{
    public MovePlayerState(PlayerController playerController) : base(playerController) { }

    public override void Execute()
    {
        playerController.Move();
    }

    public override PlayerState HandleInput()
    {
        PlayerState nextState;

        nextState = base.HandleInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerController.SetState(PState.Jump);
            nextState = new JumpPlayerState(playerController);
        }
        if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
        {
            //playerController.SetState(PState.Idle);
            nextState = new IdlePlayerState(playerController);
        }

        return nextState;
    }
}

public class JumpPlayerState : PlayerTransientState
{
    public JumpPlayerState(PlayerController playerController) : base(playerController) { }


    // �� �κ� PlayerState �����ϰ� ���� �ʿ�
    /*
    public override void Enter()
    {
        playerController.Jump();
        Execute();
    }

    public override void Execute()
    {
        // ���߿����� �յ��¿� �̵� �����ϰԲ� Move() ȣ��
        playerController.Move();
    }
    */

    public override void OnMessaged(Msg msg)
    {
        switch (msg)
        {
            case Msg.Land:
                // ���� �� �յ��¿� �̵� ���̾��� ���
                if (Input.GetAxisRaw("Horizontal") != 0.0f && Input.GetAxisRaw("Vertical") != 0.0f)
                {
                    playerController.SetState(PState.Move);
                    break;
                }
                // �յ��¿� �̵� ������ �ʾ��� ���
                playerController.SetState(PState.Idle);
                break;
        }
    }
}

public class ShootPlayerState : PlayerState
{
    public ShootPlayerState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {
        base.Enter();
        playerController.Shoot();
        playerController.ReturnToPrevState();
    }
}
