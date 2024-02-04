using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTattooMenu : MonoBehaviour
{
    public bool menuOn;
    public Transform myTatttoos;
    public TattooMesh draggedTat;
    public CinemachineVirtualCamera myCam;
    public bool draggingTat;

    Vector3 tatOnPos;
    Vector3 tatOffPos;

    void Start()
    {
        tatOnPos = Vector3.zero;
        tatOffPos = new Vector3(0, 10, 0);
        menuOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(draggedTat != null)
        {
            draggingTat = true;
        }
        else
        {
            draggingTat=false;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (menuOn)
            {
                TurnOffMenu();
            }
            else
            {
                TurnOnMenu();
            }
        }

    }

    public void TurnOnMenu()
    {
        myCam.m_Priority = 20;
        StartCoroutine(LerpPosition(tatOnPos, 0.5f));
    }

    public void TurnOffMenu()
    {

        StartCoroutine(LerpPosition(tatOffPos, 0.5f));
    }


    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        if(targetPosition == tatOnPos)
        {
            myTatttoos.gameObject.SetActive (true);
        }
        if(targetPosition == tatOffPos)
        {
            menuOn = false;
        }
        float time = 0;
        Vector3 startPosition = myTatttoos.localPosition;
        while (time < duration)
        {
            myTatttoos.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        myTatttoos.localPosition = targetPosition;
        if(targetPosition == tatOnPos)
        {
            menuOn = true;
        }
        if(targetPosition == tatOffPos)
        {
            myCam.m_Priority = 0;
            myTatttoos.gameObject.SetActive(false);
        }
    }
}
