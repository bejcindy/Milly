using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    public Animator myTitle;
    public bool inZone;
    public Transform playerSpawnPos;


    bool showZoneName;
    bool hideZoneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (showZoneName)
        {
            showZoneName = false;
            ShowZoneName();
        }

        if (hideZoneName)
        {
            hideZoneName = false;
            HideZoneName();
        }
    }

    void ShowZoneName()
    {
        if (!myTitle.gameObject.activeSelf)
        {
            myTitle.gameObject.SetActive(true);
        }
        else
        {
            myTitle.SetTrigger("Show");
        }
    }


    void HideZoneName()
    {
        if (myTitle.gameObject.activeSelf)
        {
            if(myTitle.GetCurrentAnimatorClipInfo(0)[0].clip.name != "HideZoneTitle" && myTitle.GetCurrentAnimatorClipInfo(0)[0].clip.name !=  "ZoneTitleOff")
                myTitle.SetTrigger("Hide");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
            showZoneName = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            hideZoneName = true;
        }
    }
}
