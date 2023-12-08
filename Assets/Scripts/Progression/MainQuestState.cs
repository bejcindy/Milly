using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestState: MonoBehaviour
{

    [Header("First Gloria Talk")]
    public static bool firstGloriaTalk;

    public static bool readyAkiConfrontation;
    public static bool gloriaArrivesIza;
    public static bool akiConfronted;
    public static bool parentsCalled;
    public static bool mainQuestStarted;

    public GameObject AkiConfrontation;
    public Chair izakayaHighChair;
    public Transform backToIzaAlt;

    private void Start()
    {
        firstGloriaTalk = false;
        akiConfronted = false;
        parentsCalled = false;
    }

    public void Update()
    {
        if (readyAkiConfrontation && izakayaHighChair.isInteracting && !AkiConfrontation.activeSelf)
            AkiConfrontation.SetActive(true);

        if (gloriaArrivesIza && izakayaHighChair.isInteracting)
        {
            DialogueManager.StartConversation("MainQuest/BackToMiyoshiya");
            backToIzaAlt.gameObject.SetActive(false);
            gloriaArrivesIza = false;
        }
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
}
