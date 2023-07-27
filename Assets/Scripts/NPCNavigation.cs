using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCNavigation : MonoBehaviour
{
    public Transform[] destinations;

    [SerializeField] float destThreashold;
    [SerializeField] float waitTime;

    NavMeshAgent agent;
    int currentDestIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= destThreashold)
        {
            StartCoroutine("NextDestination");
        }
    }

    IEnumerator NextDestination()
    {
        if (currentDestIndex < destinations.Length)
            currentDestIndex++;
        else
            currentDestIndex = 0;
        agent.SetDestination(destinations[currentDestIndex].position);
        agent.isStopped = true;
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        
    }
}
