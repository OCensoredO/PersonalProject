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
        // 조준 모드 전환
        if (Input.GetKeyDown(KeyCode.D))
            playerController.ToggleTargettingMode();

        // 총알 발사
        if (Input.GetKeyDown(KeyCode.F))
            return new ShootingPlayerState(playerController, this);

        return null;
    }

    public virtual void Enter() { return; }
    public virtual void Execute() { return; }
    public virtual void Exit() { return; }
    public virtual IState<PMsg> OnMessaged(PMsg pmsg) { return null; }
}


// 들어온 후 즉시, 혹은 일정 시간 뒤 이전 상태로 돌아가는 상태
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
    // Shoot 상태에서 Jump 상태로 전이 시, 공중에서도 점프를 하는 현상을 방지하기 위해 다음 상태를 기억함
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
        // 공중에서도 앞뒤좌우 이동 가능하게끔 Move() 호출
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
