using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelvin : NPCControl
{


    protected override void Start()
    {
        base.Start();
        machine.initialState = ChooseInitialState('F');
    }

    public void KelvinAction1()
    {
        noLookInConvo = true;
        currentDialogue = dialogues[1];
    }

    public void KelvinAction2()
    {
        noLookInConvo = true;
        currentDialogue = dialogues[2];
    }

    public void KelvinAction3()
    {
        noTalkStage = true;
    }


}
