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



public class StartGame : MonoBehaviour, ISaveSystem
{
    public bool ignoreSave;
    public bool skipStart;
    public bool startFinished;
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
    public bool startCountDown;

    PlayerMovement playerMovement;
    DialogueSystemTrigger bathroomMonologue;
    public EventReference playerSigns;

    public void Awake()
    {
        if(skipStart) 
        {
            startFade.SetActive(false);
            playerCinemachine.m_Priority = 10;
            izakayaStartCam.enabled = false;
            izakayaStartCam.m_Priority = 0;
            door.enabled = true;
            prologue1.SetActive(false);
            prologue2.SetActive(false);
        }
    }
    void Start()
    {
        bathroomMonologue = GetComponent<DialogueSystemTrigger>();
        playerMovement = ReferenceTool.playerMovement;
        fogProfile.albedo.a = 1;

        skipStart = false;

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

    public void FinishStart()
    {
        startFinished = true;
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


    public void PlayerSigns()
    {
        FMODUnity.RuntimeManager.PlayOneShot(playerSigns, playerMovement.transform.position);
    }

    public void SaveData(ref GameData data)
    {
        string id = GetComponent<ObjectID>().id;
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError(gameObject.name + " ID is null.");
        }

        if(data.startDataDict.ContainsKey(id))
            data.startDataDict.Remove(id);
        data.startDataDict.Add(id, startFinished);
    }

    public void LoadData(GameData data)
    {
        string id = GetComponent<ObjectID>().id;
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError(gameObject.name + " ID is null.");
        }

        if(data.startDataDict.TryGetValue(id, out bool startFinished))
        {
            this.startFinished = startFinished;
            if (startFinished)
            {
                izakayaStartCam.enabled = false;
                startCountDown = false;
                playerCinemachine.m_Priority = 10;
                izakayaStartCam.m_Priority = 0;

                door.enabled = true;
                prologue1.SetActive(false);
                prologue2.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class StartData
{
    public bool playerMovable;
    public bool initialDialogueOn;
    public bool proFinished1;
    public bool proFinished2;
    public int startCamPriority;
    public int playerCamPriority;

    public StartData(bool proFinished1, bool proFinished2, int startCamPriority, int playerCamPriority, bool playerMovable, bool initialDialogueOn)
    {
        this.proFinished1 = proFinished1;
        this.proFinished2 = proFinished2;
        this.startCamPriority = startCamPriority;
        this.playerCamPriority = playerCamPriority;
        this.playerMovable = playerMovable;
        this.initialDialogueOn = initialDialogueOn;
    }
}
