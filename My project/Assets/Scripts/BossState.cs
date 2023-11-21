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

public abstract class BossState : IState<BMsg>
{
    protected Color bColor;
    protected Boss boss;

    public BossState(Boss boss)
    {
        this.boss = boss;
    }

    public virtual IState<BMsg> HandleInput()
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
    public virtual IState<BMsg> OnMessaged(BMsg msg)
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

    public override IState<BMsg> HandleInput()
    {
        if (Time.time - startTime < duration) return null;
        //return prevState;
        return prevState;
    }
}


public abstract class BossBattleState : BossState
{
    protected float _coolTime;
    protected float _startTime;
    protected float _elapsedTimeAfterTransition;

    public BossBattleState(Boss boss, float startTime = 0f, float elapsedTimeAfterTransition = 0f) : base(boss)
    {
        _startTime = startTime;
        _elapsedTimeAfterTransition = elapsedTimeAfterTransition;
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log(Time.time - _startTime + _elapsedTimeAfterTransition);
        boss.GetClosertoPlayer();

        if (Time.time - _startTime + _elapsedTimeAfterTransition < _coolTime) return;

        usePattern();
        _startTime = Time.time;
        _elapsedTimeAfterTransition = 0f;
    }

    protected virtual void usePattern() { return; }
}


public class IdleBossState : BossState
{
    public IdleBossState(Boss boss) : base(boss) { bColor = Color.cyan; }

    public override IState<BMsg> OnMessaged(BMsg msg)
    {
        IState<BMsg> nextState = base.OnMessaged(msg);
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

    public override IState<BMsg> OnMessaged(BMsg msg)
    {
        IState<BMsg> nextState = base.OnMessaged(msg);
        if (nextState != null) return base.OnMessaged(msg);

        switch (msg)
        {
            case BMsg.BewareBoxExit:
                return new IdleBossState(boss);
            case BMsg.MonitorBoxEnter:
                //return new MonitoringBossState(boss, null, null);
                return new RemoteBossState(boss);
            default:
                return null;
        }
    }
}

/*
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
    public override IState<BMsg> HandleInput()
    {
        if (Time.time - startTime < duration) return null;
        return nextState;
    }

    public override IState<BMsg> OnMessaged(BMsg msg)
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
*/

public class MeleeBossState : BossBattleState
{
    /*
    private float _coolTime;
    private float _startTime;
    private float _elapsedTimeAfterTransition;
    */

    public MeleeBossState(Boss boss, float startTime = 0f, float elapsedTimeAfterTransition = 0f) : base(boss, startTime, elapsedTimeAfterTransition)
    {
        bColor = Color.red;
        _coolTime = 4.0f;
        //_startTime = startTime;
        //_elapsedTimeAfterTransition = elapsedTimeAfterTransition;
    }

    /*
    public override void Execute()
    {
        base.Execute();
        Debug.Log(Time.time - _startTime + _elapsedTimeAfterTransition);

        if (Time.time - _startTime + _elapsedTimeAfterTransition < _coolTime) return;

        //boss.UseMeleePattern();
        _startTime = Time.time;
        _elapsedTimeAfterTransition = 0f;
    }
    */

    public override IState<BMsg> OnMessaged(BMsg msg)
    {
        BossState state = (BossState)base.OnMessaged(msg);
        if (state != null) return state;

        switch (msg)
        {
            case BMsg.MeleeBoxExit:
                return new RemoteBossState(boss, _startTime, Time.time - _startTime);
        }

        return null;
    }

    protected override void usePattern()
    {
        //boss.UseMeleePattern();
    }
}


public class RemoteBossState : BossBattleState
{
    /*
    private float _coolTime;
    private float _startTime;
    private float _elapsedTimeAfterTransition;
    */

    public RemoteBossState(Boss boss, float startTime = 0f, float elapsedTimeAfterTransition = 0f) : base(boss, startTime, elapsedTimeAfterTransition)
    {
        bColor = Color.grey;
        _coolTime = 6.0f;
        //_startTime = startTime;
        //_elapsedTimeAfterTransition = elapsedTimeAfterTransition;
    }

    /*
    public override void Execute()
    {
        base.Execute();
        Debug.Log(Time.time - _startTime + _elapsedTimeAfterTransition);

        if (Time.time - _startTime + _elapsedTimeAfterTransition < _coolTime) return;

        boss.UseRemotePattern();
        _startTime = Time.time;
        _elapsedTimeAfterTransition = 0f;
    }
    */
    public override IState<BMsg> OnMessaged(BMsg msg)
    {
        BossState state = (BossState)base.OnMessaged(msg);
        if (state != null) return state;

        switch (msg)
        {
            case BMsg.MeleeBoxEnter:
                return new MeleeBossState(boss, _startTime, Time.time - _startTime);
        }

        return null;
    }

    protected override void usePattern()
    {
        boss.UseRemotePattern();
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

    public override IState<BMsg> OnMessaged(BMsg msg)
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