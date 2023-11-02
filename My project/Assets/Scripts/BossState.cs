using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected Color bColor;
    protected Boss boss;

    public BossState(Boss boss)
    {
        this.boss = boss;
    }

    public virtual State<BMsg> HandleInput()
    {
        // 상태 전이가 일어나지 않음
        return null;
    }

    public virtual void Enter()
    {
        boss.SetColor(bColor);
    }

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
                return null;
        }
    }
}


public abstract class BossTransientState : BossState
{
    protected BossState prevState;
    public BossState nextState;
    protected float startTime;
    protected float duration;

    public BossTransientState(Boss boss, BossState prevState, BossState nextState = null) : base(boss)
    {
        this.prevState = prevState;
        this.nextState = nextState;
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
        return prevState;
    }
}


public class IdleBossState : BossState
{
    public IdleBossState(Boss boss) : base(boss) { bColor = Color.cyan; }

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
    public BewaringBossState(Boss boss) : base(boss) { bColor = Color.blue ; }

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
    public MonitoringBossState(Boss boss, BossState prevState, BossState nextState) : base(boss, prevState, nextState)
    {
        bColor = Color.yellow;
        duration = 3.0f;

        if (this.nextState == null)
            this.nextState = new RemoteBossState(boss, this);
    }

    // 이전 상태를 리턴하는 대신 다음 상태를 리턴해야 하므로 재정의
    public override State<BMsg> HandleInput()
    {
        if (Time.time - startTime < duration) return null;
        return nextState;
    }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        BossState state = (BossState)base.OnMessaged(msg);
        if (state != null) return state;

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
    public MeleeBossState(Boss boss, BossTransientState prevState) : base(boss, prevState)
    {
        bColor = Color.red;
        duration = 2.0f;
    }


    public override State<BMsg> HandleInput()
    {
        if (Time.time - startTime < duration) return null;
        return new MonitoringBossState(boss, this, nextState);
    }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        BossState state = (BossState)base.OnMessaged(msg);
        if (state != null) return state;

        switch (msg)
        {
            case BMsg.MeleeBoxEnter:
                nextState = new MeleeBossState(boss, this);
                break;
            case BMsg.MeleeBoxExit:
                nextState = new RemoteBossState(boss, this);
                break;
        }

        return null;
    }
}


public class RemoteBossState : BossTransientState
{
    public RemoteBossState(Boss boss, BossState prevState) : base(boss, prevState)
    {
        bColor = Color.grey;
        duration = 4.0f;
    }

    public override State<BMsg> HandleInput()
    {
        if (Time.time - startTime < duration) return null;
        return new MonitoringBossState(boss, this, nextState);
    }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        BossState state = (BossState)base.OnMessaged(msg);
        if (state != null) return state;

        switch (msg)
        {
            case BMsg.MeleeBoxEnter:
                nextState = new MeleeBossState(boss, this);
                break;
            case BMsg.MeleeBoxExit:
                nextState = new RemoteBossState(boss, this);
                break;
        }

        return null;
    }
}


public class DeadBossState : BossState
{
    public DeadBossState(Boss boss) : base(boss) { bColor = Color.black; }

    public override void Execute()
    {
        base.Execute();
        boss.Restart();
    }
}


public class RetreatingBossState : BossTransientState
{
    private const float healInterval = 1.0f;

    public RetreatingBossState(Boss boss, BossState prevState) : base(boss, prevState)
    {
        bColor = Color.green;
        duration = 1000.0f;
    }

    public override void Execute()
    {
        base.Execute();
        if (Time.time - startTime < healInterval) return;

        startTime = Time.time;
        boss.Heal();
    }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
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