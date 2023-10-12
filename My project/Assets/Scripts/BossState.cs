using UnityEngine;

public abstract class BossState
{
    //public string stateName { get; private set; }
    // 패턴 사용 따위의 작업 수행
    public abstract void Execute();
    // 스프라이트 변경 따위의 작업 수행
    public abstract void Enter();

    //public void setStateName(string stateName) { this.stateName = stateName; }

    //abstract public void OnExit();
}

public class BossStateIdle : BossState
{
    public override void Execute()
    {
        Debug.Log("대기 중");
    }

    public override void Enter()
    {
        Debug.Log("대기 상태 진입");
    }
}

public class BossStateMelee : BossState
{
    public override void Execute()
    {
        Debug.Log("근거리 공격");
    }

    public override void Enter()
    {
        Debug.Log("근거리 상태 진입");
    }
}

public class BossStateRemote : BossState
{
    public override void Execute()
    {
        Debug.Log("원거리 공격");
    }

    public override void Enter()
    {
        Debug.Log("원거리 상태 진입");
    }
}

public class BossStateRetreat : BossState
{
    public override void Execute()
    {
        Debug.Log("후퇴");
    }

    public override void Enter()
    {
        Debug.Log("후퇴 상태 진입");
    }
}