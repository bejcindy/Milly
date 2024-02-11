using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ron : NPCControl
{
    public EatObject akiPizza;
    public EatObject charlesPizza;
    public CharacterTattoo pizzaTat;
    bool pizzaTatTriggered;
    protected override void Start()
    {
        base.Start();

        noLookInConvo = true;

    }

    protected override void Update()
    {
        base.Update();
        if(akiPizza.ate && charlesPizza.ate)
        {
            if (!pizzaTatTriggered)
            {
                pizzaTatTriggered = true;
                Invoke(nameof(TriggerPizzaTat), 1f);
            }

        }
    }

    void TriggerPizzaTat()
    {
        pizzaTat.triggered = true;
    }

    public void RonAction1()
    {
        talkable = true;
        noLookInConvo = false;
        remainInAnim = false;
    }

    public void RonAction2()
    {
        noLookInConvo = true;
        noTalkStage = true;
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
        if (!menuFirstTriggered)
        {
            menuFirstTriggered = true;
            ActivateTattooMenu();
        }
    }
}
