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

public enum PMsg
{
    Land
}

public abstract class PlayerState
{
    protected PlayerController playerController;

    public PlayerState(PlayerController playerController) { this.playerController = playerController; }

    public virtual PlayerState HandleInput()
    {
        // PlayerController와의 디커플링을 위해 조준 모드 정보를 PlayerState 안에 집어 넣는 것 고려중
        // 조준 모드 전환
        if (Input.GetKeyDown(KeyCode.D))
            playerController.ToggleTargettingMode();

        // 총알 발사
        if (Input.GetKeyDown(KeyCode.F))
            return new ShootingPlayerState(playerController);
        /*
    {
        //playerController.SetState(PState.Shoot);
        //return true;
        return new ShootingPlayerState(playerController);
    }
        */

        //execute();
        //return false;
        //return new ShootPlayerState(playerController);

        return null;
    }

    public virtual void Enter() { return; }
    public virtual void Execute() { return; }
    public virtual void Exit() { return; }
    public virtual PlayerState OnMessaged(PMsg pmsg) { return null; }
}

// 들어온 후 일정 시간 뒤 이전 상태로 돌아가는 상태
// 아직 미적용
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

    public override PlayerState HandleInput()
    {
        PlayerState nextState = base.HandleInput();
        if (nextState != null) return nextState;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerController.SetState(PState.Jump);
            //nextState = new JumpingPlayerState(playerController);
            //return true;
            return new JumpingPlayerState(playerController, this);
        }
        // 이 이동값? 살릴 방법 고려해보기
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
        // 이거 땜에 커플링은 불가피할듯. 나중에 이동속도 증가/감소하는 거 넣으려면 플컨에서 값 가져와야 할듯함
        playerController.Move();
    }

    public override PlayerState HandleInput()
    {
        PlayerState nextState = base.HandleInput();
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


    // 미완성, 동작은 하나 수정 필요
    
    public override void Enter()
    {
        playerController.Jump();
        //Execute();
    }

    public override void Execute()
    {
        // 공중에서도 앞뒤좌우 이동 가능하게끔 Move() 호출
        playerController.Move();
    }
    

    public override PlayerState OnMessaged(PMsg pmsg)
    {
        switch (pmsg)
        {
            case PMsg.Land:
                return prevState;
                /*
                // 착지 시 앞뒤좌우 이동 중이었을 경우
                if (Input.GetAxisRaw("Horizontal") != 0.0f && Input.GetAxisRaw("Vertical") != 0.0f)
                {
                    playerController.SetState(PState.Move);
                    break;
                }
                // 앞뒤좌우 이동 중이지 않았을 경우
                playerController.SetState(PState.Idle);
                break;
                */
        }

        return null;
    }
}

// 트랜지언트 스테이트 적용 예정
public class ShootingPlayerState : PlayerState
{
    public ShootingPlayerState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {
        base.Enter();
        playerController.Shoot();
        playerController.ReturnToPrevState();
    }
}
