using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class NPCDestinations : MonoBehaviour
{
    [SerializeField] private Transform[] destinations;
    [SerializeField] private float[] waitTimes;
    [SerializeField] private string[] waitActions;
    [SerializeField] public Transform currentDestination => destinations[_counter]; 
    NPCControl control;
    NavMeshAgent agent;
    public int _counter = 0;
    public bool finalStop;
    Transform currentStop;



    private void Awake()
    {
        control = GetComponent<NPCControl>();
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        finalStop = FinalStop();
        
        if(!finalStop)
            currentStop = destinations[_counter];   
    }

    public Transform GetCurrentDestination()
    {
        return destinations[_counter-1];
    }

    public bool FinalStop()
    {
        if (_counter == destinations.Length)
            return true;
        return false;
    }
    public Transform GetNext()
    {

        var point = destinations[_counter];

        return point;
    }

    public bool HasReached(NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.isStopped = true;
                    return true;
                }
            }
        }
        return false;
    }

    public Transform[] GetDestinations()
    {
        return destinations;
    }

    public float GetWaitTime()
    {
        return waitTimes[_counter-1];
    }

    public string GetWaitAction()
    {
        return waitActions[_counter-1];
    }
}
