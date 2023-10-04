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
        // remote나 idle상태일 때, 일정 거리 내로 플레이어가 들어오면 melee 상태로 전이
        if (other.tag == "Player" && (dummy.getState() == "remote" || dummy.getState() == "idle")) dummy.setState("melee");
    }

    private void OnTriggerExit(Collider other)
    {
        // melee 상태일 때, 일정 거리 이상 플레이어가 멀어지면 remote 상태로 전이
        if (other.tag == "Player" && dummy.getState() == "melee") dummy.setState("remote");
    }
}
