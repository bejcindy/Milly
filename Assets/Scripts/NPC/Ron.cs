using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ron : NPCControl
{
    public static bool akiPizzaAte;
    public static bool charlesPizzaAte;
    public EatObject akiPizza;
    public EatObject charlesPizza;
    public PizzaBoard pizzaBoard;
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
        if(akiPizzaAte && charlesPizzaAte)
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

        if(_counter == 1)
        {
            pizzaBoard.enabled = true;  
            MainQuestState.demoProgress++;
        }
    }
}
