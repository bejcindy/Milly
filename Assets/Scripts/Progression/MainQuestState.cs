using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestState: MonoBehaviour
{
    public bool finalGloriaHotKey;
    public static int demoProgress;
    public int demoFinishCount;
    bool demoFinalGloriaMove;

    public static bool firstGloriaTalk;
    bool afterGloThought;
    public static bool readyAkiConfrontation;
    public static bool gloriaArrivesIza;
    public static bool akiConfronted;
    public static bool parentsCalled;
    public static bool mainQuestStarted;

    public GameObject AkiConfrontation;
    public Chair izakayaHighChair;
    public Transform backToIzaAlt;

    public Loyi loyi;
    public Ron ron;
    public Hugo hugo;
    public Gloria gloria;
    public Charles charles;
    public Felix felix;

    public NPCControl[] allNPCs;


    private void Start()
    {
        firstGloriaTalk = false;
        readyAkiConfrontation = false;
        gloriaArrivesIza = false;
        akiConfronted = false;
        parentsCalled = false;
        mainQuestStarted = false;
    }

    public void Update()
    {
        demoFinishCount = demoProgress;
        CheckParentCallTrigger();
        if (readyAkiConfrontation && izakayaHighChair.isInteracting && !AkiConfrontation.activeSelf && !akiConfronted)
        {
            akiConfronted = true;
            AkiConfrontation.SetActive(true);
        }


        if (gloriaArrivesIza && izakayaHighChair.isInteracting && !ReferenceTool.playerHolding.inDialogue)
        {
            bool izaTalked = DialogueLua.GetVariable("MainQuest/BackToIzaTalked").asBool;
            if (!izaTalked)
            {
                DialogueManager.StartConversation("MainQuest/BackToMiyoshiya");
                backToIzaAlt.gameObject.SetActive(false);
            }
            
            gloriaArrivesIza = false;
        }

        if(firstGloriaTalk && !ReferenceTool.playerHolding.inDialogue && !afterGloThought)
        {
            afterGloThought = true;
            Invoke(nameof(StartGloAfterThought), 1f);
        }

        if(demoProgress == 6)
        {
            if (!demoFinalGloriaMove)
            {
                demoFinalGloriaMove = true;
                gloria.StopIdle();
            }
        }

        if (finalGloriaHotKey)
        {
            demoProgress = 6;
        }

        

    }

    bool CheckDemoFinished()
    {
        foreach(NPCControl npc in allNPCs)
        {
            if (npc._counter != npc.destinations.Length)
            {
                return false;
            }
        }
        return true;
    }

    

    public void CheckParentCallTrigger()
    {
        if(felix.finalStop && loyi.finalStop && ron.finalStop && hugo.finalStop && gloria.finalStop && charles.finalStop)
        {
            if (!parentsCalled)
            {
                parentsCalled = true;
                Invoke(nameof(StartParentsCallingConvo), 10f);
            }
        }
    }



    void StartParentsCallingConvo()
    {
        DialogueManager.StartConversation("MainQuest/CallingParents");
    }

    public void SetFirstGloriaTalked()
    {
        firstGloriaTalk = true;
    }

    public void SetAkiConfrontedTrue()
    {
        akiConfronted = true;
    }

    public void SetGloriaArrivesIza()
    {
        gloriaArrivesIza = true;
    }

    public void SetParentsCalled()
    {
        parentsCalled = true;
    }

    public void SetMainQuestStart()
    {
        mainQuestStarted = true;
    }



    public void SetReadyAkiConfrontation()
    {
        readyAkiConfrontation = true;
    }

    public static void ResetVars()
    {
        firstGloriaTalk = false;
        akiConfronted = false;
        parentsCalled = false;
    }

    void StartGloAfterThought()
    {
        DialogueManager.StartConversation("Thoughts/AfterGloria");
    }

    public void ShowConversationHint()
    {
        DataHolder.ShowHint(DataHolder.hints.convoHint);
    }
    public void HideConversationHint()
    {
        DataHolder.HideHint(DataHolder.hints.convoHint);
    }
}
