using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using VolumetricFogAndMist2;
using VInspector;

public class Zayne : NPCControl
{

    [Foldout("Cutscene")]
    public bool playingSong = false;
    public bool fadingFog = false;
    public bool turningOnLights = false;
    public Light[] lights;
    public GameObject logo;
    public float[] intensities;
    public float timeSongPlayed = 0;

    public VolumetricFogProfile fog;
    protected override void Start()
    {
        fog.albedo.a = 1;
        base.Start();
        talkable = true;
    }

    protected override void Update()
    {
        base.Update();

        if (playingSong)
        {
            timeSongPlayed += Time.deltaTime;
        }



        if (fadingFog)
        {
            if(fog.albedo.a > 0)
                fog.albedo.a -= Time.deltaTime * 0.2f;
            else
            {
                fog.albedo.a = 0;
                fadingFog = false;
            }

        }

        if (turningOnLights)
        {
            TurnOnLights();
        }
    }
    public void ZayneAction1()
    {
        noTalkInWalk = false;
        noTalkStage = true;
        currentDialogue.gameObject.SetActive(true);
        noMoveAfterTalk = true;
    }

    public void ZayneAction2()
    {
        noTalkInWalk = false;
        noTalkStage = true;
    }


    protected override void OnConversationEnd(Transform other)
    {
        inConversation = false;
        firstTalked = true;
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        currentDialogue.gameObject.SetActive(false);
        noTalkInWalk = true;
    }

    public void TurnOnLights()
    {
        for(int i = 0;  i < lights.Length; i++)
        {
            if (lights[i].intensity < intensities[i])
            {
                lights[i].intensity += Time.deltaTime;
            }
            else
            {
                lights[i].intensity = intensities[i];
            }
        }
    }


    public void ChangeMainQuestDialogue()
    {
        //StopIdle();
        noLookInConvo = true;
        noTalkStage = false;
        firstTalked = false;
        currentDialogue = dialogueHolder.GetChild(2);
        SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
        noMoveAfterTalk = false;
    }

    public void ChangePizzaDialogue()
    {
        StopIdle();
        noTalkStage = false;
        firstTalked = false;
        currentDialogue = dialogueHolder.GetChild(3);
        SetMainTalkFalse();
        string reTriggerName = "NPC/" + gameObject.name + "/Other_Interacted";
        DialogueLua.SetVariable(reTriggerName, false);
        noMoveAfterTalk = true;
    }

    public void FadeOutFog()
    {
        fadingFog = true;
    }

    public void TurnOnLight()
    {
        turningOnLights = true;
    }

    public void MoveZayneAfterWindow()
    {
        noMoveAfterTalk = false;
        StopIdle();
    }

    public void StartThemeSong()
    {
        playingSong = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Static/ThemeSong");
    }


}
