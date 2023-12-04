using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestState: MonoBehaviour
{

    [Header("First Gloria Talk")]
    public static bool firstGloriaTalk;
    public static bool readyAkiConfrontation;
    public static bool akiConfronted;
    public static bool parentsCalled;
    public GameObject AkiConfrontation;
    public Chair izakayaHighChair;

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
    }

    public void SetFirstGloriaTalked()
    {
        firstGloriaTalk = true;
    }

    public void SetAkiConfrontedTrue()
    {
        akiConfronted = true;
    }

    public void SetParentsCalled()
    {
        parentsCalled = true;
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
