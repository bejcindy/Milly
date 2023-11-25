using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akihito : NPCControl 
{

    public void ActivateAki()
    {
        onHoldChar = true;
        ChangeLayer(17);
    }
}
