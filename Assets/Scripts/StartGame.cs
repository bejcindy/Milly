using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class StartGame : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerCam playerCam;
    public CinemachineVirtualCamera playerCinemachine;
    public bool startSequence;
    // Start is called before the first frame update
    void Start()
    {
        if (startSequence)
        {
            playerMovement.enabled = false;
            playerCam.enabled = false;
        }
        else
        {
            playerMovement.enabled = true;
            playerCam.enabled=true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateGame()
    {
        playerMovement.enabled = true;
        playerCam.enabled = true;
    }
}
