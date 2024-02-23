using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Unity.IO;
using FMODUnity;
using PixelCrushers.DialogueSystem;
using VolumetricFogAndMist2;
using VInspector;

public class StartGame : MonoBehaviour
{
    public bool noSkip;
    [Foldout("After Cutscene")]


    public CinemachineVirtualCamera playerCinemachine;
    public CinemachineVirtualCamera izakayaStartCam;

    public GameObject startFade;
    public Door door;
    public FixedCameraObject izaChair;
    public GameObject prologue1;
    public GameObject prologue2;
    public VolumetricFogProfile fogProfile;

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

    public void Awake()
    {

    }
    void Start()
    {
        fogProfile.albedo.a = 1;
        outsideAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Static/Outside_Ambience");
        bathroomMonologue = GetComponent<DialogueSystemTrigger>();
        playerMovement = ReferenceTool.playerMovement;
        playerHolding = ReferenceTool.playerHolding;
        playerCam = ReferenceTool.player.GetComponent<PlayerCam>();
        if (noSkip)
        {
            startFade.SetActive(true);
            izakayaStartCam.m_Priority = 10;
            playerCinemachine.m_Priority = 9;
            playerMovement.enabled = false;
            startCountDown = true;
            door.enabled = false;
            prologue1.SetActive(true);
            prologue2.SetActive(true);

        }
        else
        {
            startFade.SetActive(false);
            playerCinemachine.m_Priority = 10;
            izakayaStartCam.m_Priority = 0;
            playerMovement.enabled = true;
            door.enabled = true;
            prologue1.SetActive(false);
            prologue2.SetActive(false);

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
