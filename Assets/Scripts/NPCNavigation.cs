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
    public bool paused;

    [SerializeField] float destThreashold;
    [SerializeField] float waitTime;

    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    public Animator anim;
    public int currentDestIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        agent.SetDestination(destinations[0].position);
        anim = transform.GetChild(0).GetComponent<Animator>();
        agent.speed = speed;
        obstacle.enabled = false;
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!talking)
        {
            if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= destThreashold && !paused)
//                if (agent.remainingDistance <= destThreashold && agent.enabled == true)
            {
                StartCoroutine("NextDestination");
            }
        }
        else
            agent.isStopped = true;
    }

    IEnumerator NextDestination()
    {
        paused = true;
        Debug.Log("checking how many times are we calling this shit");
        //if (currentDestIndex < destinations.Length)
        //    currentDestIndex++;
        //else
        //{
        //    if (loopRoute)
        //        currentDestIndex = 0;
        //    else
        //        GetComponent<NPCNavigation>().enabled = false;
        //}
        obstacle.enabled = true;
        agent.isStopped = true;
        //agent.enabled = false;
        if (anim != null)
        {
            anim.ResetTrigger("Start");
            anim.ResetTrigger("Move");
            anim.SetTrigger("Stop");
        }
        if (destinations[currentDestIndex].name.Contains("circle"))
        {
            obstacle.enabled = false;
            //agent.enabled = true;
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
            agent.SetDestination(destinations[currentDestIndex].position);
            paused = false;
        }
        else
        {

            yield return new WaitForSeconds(waitTime);
            if (anim != null)
            {
                anim.ResetTrigger("Stop");
                anim.SetTrigger("Move");
            }
            obstacle.enabled = false;
            //agent.enabled = true;
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
            agent.SetDestination(destinations[currentDestIndex].position);
            paused = false;
        }
    }
}
