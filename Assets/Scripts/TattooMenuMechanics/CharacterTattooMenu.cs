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

        if (menuOn)
        {
            TurnOnMenu();
        }
    }

    public void TurnOnMenu()
    {
        myCam.m_Priority = 20;
        myTatttoos.gameObject.SetActive(true);
    }

    public void ShowTattoos()
    {

    }

    public void HideTattoos()
    {

    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {

        float time = 0;
        Vector3 startPosition = myTatttoos.localPosition;
        while (time < duration)
        {
            myTatttoos.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        myTatttoos.localPosition = targetPosition;
    }
}
