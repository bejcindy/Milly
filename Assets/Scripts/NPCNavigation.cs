using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCNavigation : MonoBehaviour
{
    public bool loopRoute;
    public float speed;
    public Transform[] destinations;
    public bool talking;

    [SerializeField] float destThreashold;
    [SerializeField] float waitTime;

    NavMeshAgent agent;
    int currentDestIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[0].position);
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!talking)
        {
            if (agent.remainingDistance <= destThreashold)
            {
                StartCoroutine("NextDestination");
            }
        }
        else
            agent.isStopped = true;
    }

    IEnumerator NextDestination()
    {
        //if (currentDestIndex < destinations.Length)
        //    currentDestIndex++;
        //else
        //{
        //    if (loopRoute)
        //        currentDestIndex = 0;
        //    else
        //        GetComponent<NPCNavigation>().enabled = false;
        //}
        agent.SetDestination(destinations[currentDestIndex].position);
        agent.isStopped = true;
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        if (currentDestIndex < destinations.Length)
            currentDestIndex++;
        else
        {
            if (loopRoute)
                currentDestIndex = 0;
            else
                GetComponent<NPCNavigation>().enabled = false;
        }

    }
}
