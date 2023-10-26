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
    protected PlayerController playerController;

    public PlayerState(PlayerController playerController) { this.playerController = playerController; }

    public virtual bool HandleInput()
    {
        // 조준 모드 전환
        if (Input.GetKeyDown(KeyCode.D))
            playerController.ToggleTargettingMode();

        // 총알 발사
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerController.SetState(PState.Shoot);
            return true;
        }

        return false;
        //return new ShootPlayerState(playerController);

        //return null;
    }

    public virtual void Enter() { return; }
    public virtual void Execute() { return; }
    public virtual void Exit() { return; }
    public virtual void OnMessaged(Msg msg) { return; }
}

// 들어온 후 일정 시간 뒤 이전 상태로 돌아가는 상태
// 아직 미적용
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

    public override bool HandleInput()
    {
        //PlayerState nextState;

        //nextState = base.HandleInput();

        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            playerController.SetState(PState.Move);
            //nextState = new MovePlayerState(playerController);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.SetState(PState.Jump);
            //nextState = new JumpPlayerState(playerController);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerController.SetState(PState.Shoot);
            //nextState = new ShootPlayerState(playerController);
            return true;
        }

        return false;
        //return nextState;
    }
}

public class MovePlayerState : PlayerState
{
    public MovePlayerState(PlayerController playerController) : base(playerController) { }

    public override void Execute()
    {
        playerController.Move();
    }

    public override bool HandleInput()
    {
        //PlayerState nextState;

        //nextState = base.HandleInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.SetState(PState.Jump);
            return true;
            //nextState = new JumpPlayerState(playerController);
        }
        if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
        {
            playerController.SetState(PState.Idle);
            return true;
            //nextState = new IdlePlayerState(playerController);
        }

        return false;
        //return nextState;
    }
}

public class JumpPlayerState : PlayerTransientState
{
    public JumpPlayerState(PlayerController playerController) : base(playerController) { }


    // 미완성, 동작은 하나 수정 필요
    
    public override void Enter()
    {
        playerController.Jump();
        Execute();
    }

    public override void Execute()
    {
        // 공중에서도 앞뒤좌우 이동 가능하게끔 Move() 호출
        playerController.Move();
    }
    

    public override void OnMessaged(Msg msg)
    {
        switch (msg)
        {
            case Msg.Land:
                // 착지 시 앞뒤좌우 이동 중이었을 경우
                if (Input.GetAxisRaw("Horizontal") != 0.0f && Input.GetAxisRaw("Vertical") != 0.0f)
                {
                    playerController.SetState(PState.Move);
                    break;
                }
                // 앞뒤좌우 이동 중이지 않았을 경우
                playerController.SetState(PState.Idle);
                break;
        }
    }
}

// 트랜지언트 스테이트 적용 예정, 현재는 이전 상태로 돌아가는 과정이 원활하지 않아 일부 경우에 오류 발생
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
