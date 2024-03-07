using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using NPCFSM;

public class MoveState : State
{
    bool isFollowing;
    float footstepGap = .7f;
    float timer;
    protected override void OnEnter()
    {
        machine.TurnOnNavMesh();
        if (!machine.CheckReachDestination())
            machine.BeginNavigation();
        else
        {
            machine.SetNPCDestination();
            machine.BeginNavigation();
        }
        machine.SetAnimatorTrigger("Move");
    }

    protected override void OnUpdate()
    {

        if (machine.CheckReachDestination())
        {
            machine.SetMainTalkFalse();
            machine.ChangeState(machine.idleState);
        }

        FootstepSF();
    }

    protected override void OnExit()
    {
        //machine.ResetAnimTrigger("Move");
        //machine.StopNavigation();
    }

    void FootstepSF()
    {
        if (timer < footstepGap)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (!machine.footStepSF.IsNull)
                RuntimeManager.PlayOneShot(machine.footStepSF, machine.transform.position);
            timer = 0;
        }
    }
}
