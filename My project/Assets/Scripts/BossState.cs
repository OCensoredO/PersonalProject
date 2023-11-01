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

        // ���� ���̰� �Ͼ�� ����
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
                return new RetreatingBossState(boss);
            case BMsg.Die:
                return new DeadBossState(boss);
            default:
                return null;
        }
    }
}

public class IdleBossState : BossState
{
    public IdleBossState(Boss boss) : base(boss) { }

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

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        State<BMsg> nextState = base.OnMessaged(msg);
        if (nextState != null) return base.OnMessaged(msg);

        switch (msg)
        {
            case BMsg.BewareBoxExit:
                return new IdleBossState(boss);
            case BMsg.MonitorBoxEnter:
                return new MonitoringBossState(boss);
            default:
                return null;
        }
    }
}


public class MonitoringBossState : BossState
{
    public MonitoringBossState(Boss boss) : base(boss) { }

    public override State<BMsg> OnMessaged(BMsg msg)
    {
        State<BMsg> nextState = base.OnMessaged(msg);
        if (nextState != null) return base.OnMessaged(msg);

        switch (msg)
        {
            case BMsg.BewareBoxExit:
                return new IdleBossState(boss);
            case BMsg.MonitorBoxEnter:
                return new MonitoringBossState(boss);
            default:
                return null;
        }
    }
}

public class DeadBossState : BossState
{
    public DeadBossState(Boss boss) : base(boss) { }
}

public class RetreatingBossState : BossState
{
    public RetreatingBossState(Boss boss) : base(boss) { }
}

/*
public abstract class BossState
{
    //public string stateName { get; private set; }
    // ���� ��� ������ �۾� ����
    public abstract void Execute();
    // ��������Ʈ ���� ������ �۾� ����
    public abstract void Enter();

    //public void setStateName(string stateName) { this.stateName = stateName; }

    //abstract public void OnExit();
}

public class BossStateIdle : BossState
{
    public override void Execute()
    {
        Debug.Log("��� ��");
    }

    public override void Enter()
    {
        Debug.Log("��� ���� ����");
    }
}

public class BossStateMelee : BossState
{
    public override void Execute()
    {
        Debug.Log("�ٰŸ� ����");
    }

    public override void Enter()
    {
        Debug.Log("�ٰŸ� ���� ����");
    }
}

public class BossStateRemote : BossState
{
    public override void Execute()
    {
        Debug.Log("���Ÿ� ����");
    }

    public override void Enter()
    {
        Debug.Log("���Ÿ� ���� ����");
    }
}

public class BossStateRetreat : BossState
{
    public override void Execute()
    {
        Debug.Log("����");
    }

    public override void Enter()
    {
        Debug.Log("���� ���� ����");
    }
}
*/