using UnityEngine;

public enum BState
{
    Idle,
    Beware,
    Monitor,
    Melee,
    Remote,
    Retreat,
    Die
}

public enum BMsg
{

}

public abstract class BossState
{
    protected Boss boss;
    protected BossState prevState;

    public BossState(Boss boss, BossState prevState)
    {
        this.boss = boss;
        this.prevState = prevState;
    }

    public virtual BossState HandleInput()
    {
        // 일단은 핸들인풋에 넣긴 했는데 뭔가 부자연스러움, 더 고려해보기
        if (boss.hp <= 0) return new BossDieState(boss, prevState);

        if (boss.hp < 5) return new BossRetreatState(boss, prevState);

        // 상태 전이가 일어나지 않음
        return null;
    }

    public virtual void Enter() { return; }

    public virtual void Execute()
    {

    }

    public virtual void Exit() { return; }
    public virtual void OnMessaged(BMsg msg) { return; }
}

public class BossDieState : BossState
{
    public BossDieState(Boss boss, BossState prevState) : base(boss, prevState) { }
}

public class BossRetreatState : BossState
{
    public BossRetreatState(Boss boss, BossState prevState) : base(boss, prevState) { }
}

/*
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
*/