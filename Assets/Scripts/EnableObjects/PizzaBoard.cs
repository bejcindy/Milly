using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PizzaBoard : LookingObject
{
    public TextMeshPro titleText;
    public float titleTextTargetVal;
    public bool currentPizzaTextOn;

    public List<PizzaBoardText> pizzaTexts = new List<PizzaBoardText>();
    public PizzaBoardText currentPizzaText;
    public Transform frankPizzaText;

    bool firstTransformed;
    protected override void Start()
    {
        base.Start();
        
        foreach(Transform child in transform)
        {
            if (child.GetComponent<PizzaBoardText>())
            {
                pizzaTexts.Add(child.GetComponent<PizzaBoardText>());
            }
        }
    }
    protected override void Update()
    {
        base.Update();

        if(activated && !firstTransformed)
        {
            ChangeTextColor(titleText);
            currentPizzaText.TurnOnTextColor();
        }

        if (focusingThis)
        {
            titleText.gameObject.layer = 13;
            currentPizzaText.ChangeTextLayer(13);
        }
        else
        {
            if (activated)
            {
                if(titleText.gameObject.layer != 17)
                {
                    titleText.gameObject.layer = 17;
                    currentPizzaText.ChangeTextLayer(17);
                }
                currentPizzaText.TurnOnTextColor();
            }
        }



    }

    public void QueuePizza(PizzaBoardText pizzaText)
    {
        currentPizzaText = pizzaText;
    }

    void ChangeTextColor(TextMeshPro text)
    {
        if(text.color.a < titleTextTargetVal)
        {
            Color temp = text.color;
            temp.a += 0.1f * Time.deltaTime;
            text.color = temp;
        }
        else
        {
            text.gameObject.layer = 17;
            firstTransformed = true;
        }
    }

}
