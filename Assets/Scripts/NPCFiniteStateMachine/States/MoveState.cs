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
        Debug.Log("Entered move state");
        if (!machine.CheckFollowPlayer())
        {
            if (!machine.CheckReachDestination())
                machine.BeginNavigation();
            else
                machine.SetNPCDestination();
        }

        else
        {
            machine.FollowPlayer();
            isFollowing = true;
        }

        machine.StopRemainInAnim();
        machine.BeginNavigation();
        machine.SetAnimatorTrigger("Move");
    }

    protected override void OnUpdate()
    {
        if (!machine.CheckFollowPlayer())
        {
            if (!isFollowing)
            {
                if (machine.CheckReachDestination())
                {
                    machine.SetMainTalkFalse();
                    machine.ChangeState(machine.idleState);
                }
            }
            else
            {
                isFollowing = false;
                machine.SetNPCDestination();
            }

        }
        else
        {
            isFollowing = true;
            if(machine.CheckReachDestination())
                machine.ChangeState(machine.followState);
        }

        if (machine.CheckInConversation() && machine.CheckReachDestination())
            machine.ChangeState(machine.talkState);
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
