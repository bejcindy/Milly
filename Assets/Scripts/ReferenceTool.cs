using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceTool : MonoBehaviour
{
    public static Transform player;
    public static CinemachineVirtualCamera playerCinemachine;

    public Transform playerRef;
    public CinemachineVirtualCamera playerCamRef;

    public void Awake()
    {
        player = playerRef;
        playerCinemachine = playerCamRef;
    }
}
