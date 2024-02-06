using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PizzaBoardText : MonoBehaviour
{
    public float textTargetVal;
    public bool myPizzaQueued;
    public bool myPizzaOn;
    public EatObject myPizza;
    PizzaBoard pizzaBoard;

    void Start()
    {
        textTargetVal = 1f;
        pizzaBoard = transform.parent.parent.GetComponent<PizzaBoard>();
    }


    void Update()
    {
        if (myPizza.enabled)
        {
            QueueMyPizza();
        }
    }

    void QueueMyPizza()
    {
        if (!myPizzaQueued) {

            myPizzaQueued = true;
            pizzaBoard.QueuePizza(this);
        }
    }

    public void ChangeTextLayer(int i)
    {
        foreach (Transform t in transform)
        {
            t.gameObject.layer = i;
        }
    }

    public void TurnOnTextColor()
    {
        if(textTargetVal > 0)
        {
            foreach (Transform child in transform)
            {
                TextMeshPro childText = child.GetComponent<TextMeshPro>();
                ChangeTextColor(childText);

            }
        }

    }

    void ChangeTextColor(TextMeshPro text)
    {
        if (text.color.a < textTargetVal)
        {
            Color temp = text.color;
            temp.a += 0.1f * Time.deltaTime;
            text.color = temp;
        }
        else
        {
            textTargetVal = 0;
            text.gameObject.layer = 17;
        }
    }
}
