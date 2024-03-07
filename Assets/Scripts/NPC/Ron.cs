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


    }

    protected override void Update()
    {
        base.Update();
    }

    public void RonAction0()
    {
        talkable = false;
    }


    public void RonAction1()
    {
        talkable = true;
    }

    public void RonAction2()
    {
        talkable = false;
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

        }

        if(_counter == 2)
        {
            MainQuestState.demoProgress++;
        }
    }
}
