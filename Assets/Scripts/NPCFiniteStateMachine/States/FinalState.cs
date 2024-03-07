using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalState : State
{

    protected override void OnEnter()
    {
        machine.FinishNpc();
    }
}
