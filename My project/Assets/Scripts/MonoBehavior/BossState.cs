using UnityEngine;

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