using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStartLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform gloriaDest;
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera cutsceneCam;
    NavMeshAgent agent;
    PlayerMovement playerMovement;
    Rigidbody rb;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        playerMovement.initialCutsceneMove = true;
        playerCam.m_Priority = 9;
        cutsceneCam.m_Priority = 10;
        //agent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
