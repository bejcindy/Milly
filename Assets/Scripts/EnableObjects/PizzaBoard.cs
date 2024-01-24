using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PizzaBoard : LookingObject
{
    public TextMeshPro titleText;
    public float titleTextTargetVal;


    protected override void Start()
    {
        base.Start();
        titleTextTargetVal = titleText.color.a;
        Color temp = titleText.color;
        temp.a = 0;
        titleText.color = temp;
    }
    protected override void Update()
    {
        base.Update();

        if (activated)
        {
            if(titleText.color.a < titleTextTargetVal)
            {
                Color temp = titleText.color;
                temp.a += 0.1f * fadeInterval * Time.deltaTime;
                titleText.color = temp;
            }
        }
    }

}
