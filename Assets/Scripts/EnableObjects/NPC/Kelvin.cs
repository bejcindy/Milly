using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelvin : NPCControl
{
    public BuildingGroupController connectedGroupOne;
    public BuildingGroupController connectedGroupTwo;
    public BuildingGroupController connectedGroupThree;

    protected override void Start()
    {
        base.Start();
        machine.initialState = ChooseInitialState('F');
    }
}
