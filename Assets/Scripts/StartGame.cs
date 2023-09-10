using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Unity.IO;

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
            playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
            playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
            playerMovement.enabled = false;
            playerCam.enabled = false;
        }
        else
        {
            playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 300;
            playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 300;
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
        Invoke(nameof(ActivateMouse), 1.5f);
    }

    public void ActivateMouse()
    {
        playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 300;
        playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 300;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}
