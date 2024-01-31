using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PizzaBoard : LookingObject
{
    public TextMeshPro titleText;
    public float titleTextTargetVal;

    public bool currentPizzaTextOn;
    public Transform currentPizzaText;
    public Transform frankPizzaText;

    bool firstTransformed;
    protected override void Start()
    {
        base.Start();

        currentPizzaText = frankPizzaText;
    }
    protected override void Update()
    {
        base.Update();

        if (firstActivated && !firstTransformed)
        {
            ChangeTextColor(titleText);
            foreach (Transform child in currentPizzaText)
            {
                TextMeshPro childText = child.GetComponent<TextMeshPro>();
                ChangeTextColor(childText);

            }
        }

        if (focusingThis)
        {
            titleText.gameObject.layer = 13;
            foreach (Transform child in currentPizzaText)
            {
                child.gameObject.layer = 13;
            }
        }
        else
        {
            if (activated)
            {
                if(titleText.gameObject.layer != 17)
                {
                    titleText.gameObject.layer = 17;
                    foreach (Transform child in currentPizzaText)
                    {
                        child.gameObject.layer = 17;
                    }
                }
            }
        }

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
