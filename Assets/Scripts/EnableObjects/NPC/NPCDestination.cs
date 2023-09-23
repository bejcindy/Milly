using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("NPCFSM/NPCDestination"))]
public class NPCDestination : ScriptableObject
{
    public List<Transform> destinations;
    public List<float> waitTimes;
    public List<string> waitActions;
    
    public Transform GetDestination(int i)
    {
        return destinations[i];
    }

    public float GetWaitTime(int i)
    {
        return waitTimes[i];
    }

    public string GetAction(int i)
    {
        return waitActions[i];
    }
}
