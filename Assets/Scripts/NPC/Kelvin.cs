using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelvin : NPCControl
{

    public void KelvinAction0()
    {
        talkable = true;
    }

    public void KelvinAction1()
    {
        talkable = true;
    }

    public void KelvinAction2()
    {
        talkable = true;
    }

    public void KelvinAction3()
    {
        talkable = false;
        gameObject.SetActive(false);
    }

}
