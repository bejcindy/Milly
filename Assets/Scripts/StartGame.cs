using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Unity.IO;
using FMOD;

public class StartGame : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerCam playerCam;
    public CinemachineVirtualCamera playerCinemachine;
    public bool startSequence;
    public Animator startAnim;
    // Start is called before the first frame update

    FMOD.Studio.EventInstance izakayaAmbienceEvent;
    FMOD.Studio.EventInstance outsideAmbienceEvent;
    void Start()
    {
        izakayaAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Static/Izakaya_Noise");
        outsideAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Static/Outside_Ambience");

        if (startSequence)
        {
            izakayaAmbienceEvent.start();
            playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
            playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
            playerMovement.enabled = false;
            playerCam.enabled = false;
        }
        else
        {
            playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 200;
            playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 200;
            playerMovement.enabled = true;
            playerCam.enabled=true;
            outsideAmbienceEvent.start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(startAnim.isActiveAndEnabled && startAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >1)
        {
            startAnim.transform.parent.gameObject.SetActive(false);
        }
    }

    public void ActivateGame()
    {
        playerMovement.enabled = true;
        playerCam.enabled = true;
        Invoke(nameof(ActivateMouse), 1.5f);
    }

    public void ActivateMouse()
    {
        playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 200;
        playerCinemachine.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 200;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StopAmbience()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound Effects/Slide_Door", playerCam.transform.position);
        izakayaAmbienceEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        izakayaAmbienceEvent.release();
        outsideAmbienceEvent.start();
    }


}
