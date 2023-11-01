using System.Collections;
using System.Collections.Generic;
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
    LowHP,
    EnoughHP,
    Die,
    BewareBoxEnter,
    BewareBoxExit,
    MonitorBoxEnter,
    MonitorBoxExit,
    MeleeBoxEnter,
    MeleeBoxExit
}

public abstract class BossState : State<BMsg>
{
    protected Boss boss;
    //protected BossState prevState;

    public BossState(Boss boss)
    {
        this.boss = boss;
        //this.prevState = prevState;
    }

    public virtual State<BMsg> HandleInput()
    {
        // 상태 전이가 일어나지 않음
        return null;
    }

    public virtual void Enter() { return; }

    public virtual void Execute() { return; }

    public virtual void Exit() { return; }
    public virtual State<BMsg> OnMessaged(BMsg msg)
    {
        switch(msg)
        {
            case BMsg.LowHP:
                return new RetreatingBossState(boss, this);
            case BMsg.Die:
                return new DeadBossState(boss);
            default:
                Debug.Log("asdfasdf");
                return null;
        }
    }
}


public abstract class BossTransientState : BossState
{
    protected BossState prevState;
    protected float startTime;
    protected float duration;

    public BossTransientState(Boss boss, BossState prevState) : base(boss)
    {
        this.prevState = prevState;
    }

    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
    }

    public override State<BMsg> HandleInput()
    {
        if (Time.time - startTime < duration) return null;
        //return prevState;
        return null;
    }
}


public class IdleBossState : BossState
{
    public IdleBossState(Boss boss) : base(boss) { }

    public override void Enter()
    {
        base.Enter();
        boss.SetColor(Color.cyan);
    }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        State<BMsg> nextState = base.OnMessaged(msg);
        if (nextState != null) return base.OnMessaged(msg);

        switch(msg)
        {
            case BMsg.BewareBoxEnter:
                return new BewaringBossState(boss);
            default:
                return null;
        }
    }
}


public class BewaringBossState : BossState
{
    public BewaringBossState(Boss boss) : base(boss) { }

    public override void Enter()
    {
        base.Enter();
        boss.SetColor(Color.blue);
    }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        State<BMsg> nextState = base.OnMessaged(msg);
        if (nextState != null) return base.OnMessaged(msg);

        switch (msg)
        {
            case BMsg.BewareBoxExit:
                return new IdleBossState(boss);
            case BMsg.MonitorBoxEnter:
                return new MonitoringBossState(boss, null, null);
            default:
                return null;
        }
    }
}


public class MonitoringBossState : BossTransientState
{
    private BossState predictedNextState;

    public override void Enter()
    {
        base.Enter();
        boss.SetColor(Color.yellow);
    }

    public MonitoringBossState(Boss boss, BossState prevState, BossState predictedNextState) : base(boss, prevState)
    {
        duration = 3.0f;

        //if (nextState == null)
            this.predictedNextState = new RemoteBossState(boss, this);
        //else
            //this.nextState = nextState;
    }

    // 이전 상태를 리턴하는 대신 다음 상태를 리턴해야 하므로 재정의
    public override State<BMsg> HandleInput()
    {
        //Debug.Log(Time.time - startTime);
        //Debug.Log(this.nextState);
        if (Time.time - startTime < duration) return null;
        //Debug.Log(nextState);
        return predictedNextState;
    }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        BossState nextState = (BossState)base.OnMessaged(msg);
        if (nextState != null) return nextState;

        switch (msg)
        {
            case BMsg.MeleeBoxEnter:
                nextState = new MeleeBossState(boss, this);
                break;
            case BMsg.MonitorBoxEnter:
                nextState = new RemoteBossState(boss, this);
                break;
        }

        return null;
    }
}


public class MeleeBossState : BossTransientState
{
    public override void Enter()
    {
        base.Enter();
        boss.SetColor(Color.red);
    }

    public MeleeBossState(Boss boss, BossState prevState) : base(boss, prevState) { duration = 2.0f; }
}


public class RemoteBossState : BossTransientState
{
    public override void Enter()
    {
        base.Enter();
        boss.SetColor(Color.grey);
    }

    public RemoteBossState(Boss boss, BossState prevState) : base(boss, prevState) { duration = 4.0f; }
}


public class DeadBossState : BossState
{
    public override void Enter()
    {
        base.Enter();
        boss.SetColor(Color.black);
    }

    public DeadBossState(Boss boss) : base(boss) { }

    public override void Execute()
    {
        base.Execute();
        boss.Restart();
    }
}


public class RetreatingBossState : BossTransientState
{
    private const float healInterval = 1.0f;

    public override void Enter()
    {
        base.Enter();
        boss.SetColor(Color.green);
    }

    public RetreatingBossState(Boss boss, BossState prevState) : base(boss, prevState) { duration = 1000.0f; }

    public override void Execute()
    {
        base.Execute();
        if (Time.time - startTime < healInterval) return;

        startTime = Time.time;
        boss.Heal();
    }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        //BossState nextState = (BossState)base.OnMessaged(msg);
        //if (nextState is DeadBossState) return nextState;

        switch (msg)
        {
            case BMsg.EnoughHP:
                return prevState;
            case BMsg.Die:
                return new DeadBossState(boss);
            default:
                return null;
        }
    }
}