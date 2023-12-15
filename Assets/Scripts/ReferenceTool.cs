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

    public Transform playerRef;
    public CinemachineVirtualCamera playerCamRef;

    public void Awake()
    {
        player = playerRef;
        playerCinemachine = playerCamRef;
        playerHolding = player.GetComponent<PlayerHolding>();
        playerLeftHand = player.GetComponent<PlayerLeftHand>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerBrain = player.GetComponent<CinemachineBrain>();
        playerPOV = playerCinemachine.GetCinemachineComponent<CinemachinePOV>();
    }
}
