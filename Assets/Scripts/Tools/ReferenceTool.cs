using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceTool : MonoBehaviour
{
    public static Transform player;
    public static PlayerHolding playerHolding;
    public static PlayerLeftHand playerLeftHand;
    public static PlayerMovement playerMovement;
    public static CinemachineVirtualCamera playerCinemachine;
    public static CinemachineBrain playerBrain;
    public static CinemachinePOV playerPOV;
    public static PauseMenu pauseMenu;


    public Transform playerRef;
    public CinemachineVirtualCamera playerCamRef;


    public void Awake()
    {
        player = playerRef;
        playerCinemachine = playerCamRef;
        playerHolding = playerRef.GetComponent<PlayerHolding>();
        playerLeftHand = playerRef.GetComponent<PlayerLeftHand>();
        playerMovement = playerRef.GetComponent<PlayerMovement>();
        playerBrain = Camera.main.GetComponent<CinemachineBrain>();
        playerPOV = playerCamRef.GetCinemachineComponent<CinemachinePOV>();
    }

}
