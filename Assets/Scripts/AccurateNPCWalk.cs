using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccurateNPCWalk : MonoBehaviour
{

    NPCNavigation npcNav;
    // Start is called before the first frame update
    void Start()
    {
        npcNav = transform.parent.GetComponent<NPCNavigation>();
    }

    public void StartWaling()
    {
        npcNav.enabled = true;
        npcNav.talking = false;
    }
}
