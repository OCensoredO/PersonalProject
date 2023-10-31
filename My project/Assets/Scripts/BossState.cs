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
        // �ϴ��� �ڵ���ǲ�� �ֱ� �ߴµ� ���� ���ڿ�������, �� ����غ���
        if (boss.hp <= 0) return new BossDieState(boss, prevState);

        if (boss.hp < 5) return new BossRetreatState(boss, prevState);

        // ���� ���̰� �Ͼ�� ����
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