using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheckingBox : MonoBehaviour
{
    private Dummy dummy;

    private void Start()
    {
        dummy = GetComponentInParent<Dummy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // remote�� idle������ ��, ���� �Ÿ� ���� �÷��̾ ������ melee ���·� ����
        if (other.tag == "Player" && (dummy.getState() == "remote" || dummy.getState() == "idle")) dummy.setState("melee");
    }

    private void OnTriggerExit(Collider other)
    {
        // melee ������ ��, ���� �Ÿ� �̻� �÷��̾ �־����� remote ���·� ����
        if (other.tag == "Player" && dummy.getState() == "melee") dummy.setState("remote");
    }
}
