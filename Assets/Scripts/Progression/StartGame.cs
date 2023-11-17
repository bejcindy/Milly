using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Unity.IO;
using FMODUnity;
using PixelCrushers.DialogueSystem;

public class StartGame : MonoBehaviour
{
    public bool startSequence;

    public CinemachineVirtualCamera playerCinemachine;
    public CinemachineVirtualCamera izakayaStartCam;

    public GameObject startFade;

    float startDelay = 5f;
    bool startCountDown;

    PlayerMovement playerMovement;
    PlayerCam playerCam;
    PlayerHolding playerHolding;
    DialogueSystemTrigger bathroomMonologue;
    // Start is called before the first frame update
    public EventReference playerSigns;
    FMOD.Studio.EventInstance izakayaAmbienceEvent;
    FMOD.Studio.EventInstance outsideAmbienceEvent;
    void Start()
    {
        //izakayaAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Static/Izakaya_Noise");
        outsideAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Static/Outside_Ambience");
        bathroomMonologue = GetComponent<DialogueSystemTrigger>();
        GameObject player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerHolding = player.GetComponent<PlayerHolding>();
        playerCam = player.GetComponent<PlayerCam>();

        if (startSequence)
        {
            startFade.SetActive(true);
            izakayaStartCam.m_Priority = 10;
            playerCinemachine.m_Priority = 9;
            playerMovement.enabled = false;
            startCountDown = true;

        }
        else
        {
            startFade.SetActive(false);
            izakayaStartCam.m_Priority = 9;
            playerCinemachine.m_Priority = 10;
            playerMovement.enabled = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountDown)
        {
            BathroomCutscene();
        }
    }

    void BathroomCutscene()
    {
        if (startDelay > 0)
            startDelay -= Time.deltaTime;
        else
        {
            startCountDown = false;
            bathroomMonologue.enabled = true;
        }
    }

    public void ActivateGame()
    {
        playerMovement.enabled = true;
        izakayaStartCam.m_Priority = 9;
        playerCinemachine.m_Priority = 10;
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

    public void PlayerSigns()
    {
        FMODUnity.RuntimeManager.PlayOneShot(playerSigns, playerMovement.transform.position);
    }


}
