using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestState: MonoBehaviour
{

    [Header("First Gloria Talk")]
    public static bool firstGloriaTalk;
    public GameObject AkiConfrontation;
    public Chair izakayaHighChair;


    public void Update()
    {
        if (firstGloriaTalk && izakayaHighChair.isInteracting && !AkiConfrontation.activeSelf)
            AkiConfrontation.SetActive(true);
    }

    public void SetFirstGloriaTalked()
    {
        firstGloriaTalk = true;
    }
}
