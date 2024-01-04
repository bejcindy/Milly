using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using FMODUnity;

public class RecordPlayer : LivableObject
{
    [Foldout("RecordPlayer")]
    public bool hasRecord;
    public bool readyPlacing;
    public Vinyl currentRecord;
    public GameObject recordPlacer;
    public Vector3 recordPos;

    public EventReference needleSound;
    public Quaternion playingRot;
    public Quaternion stopRot;
    public bool isPlaying;

    public bool moving;
    string recorPlaceEvent = "event:/Sound Effects/ObjectInteraction/Vinyls/Vinyl_Place";
    protected override void Start()
    {
        base.Start();
        isPlaying = true;
        hasRecord = true;
        playingRot = transform.localRotation;
        stopRot = Quaternion.Euler(0f, 0f, 0f);
        recordPos = currentRecord.transform.position;
    }

    protected override void Update()
    {
        base.Update();

        if (currentRecord)
        {
            hasRecord = true;
            readyPlacing = false;
        }
        else
        {
            hasRecord = false;
            gameObject.layer = 0;
        }

        if (readyPlacing)
        {
            recordPlacer.gameObject.layer = 9;
        }
        else
        {
            recordPlacer.gameObject.layer = 0;
        }

        if (interactable)
        {
            if (hasRecord)
            {
                if (!CurrentRecordSelected() && !moving && !MainTattooMenu.tatMenuOn)
                {
                    gameObject.layer = 9;
                    if (Input.GetMouseButtonDown(0))
                    {
                        TriggerPlay();
                    }
                }
                else
                {
                    gameObject.layer = 0;
                }

            }
            else
            {
                CheckReadyPlaceRecord();
            }
        }
        else
        {
            readyPlacing = false;
            gameObject.layer = 0;
        }
    }


    void TriggerPlay()
    {
        activated = true;
        if (isPlaying)
        {
            isPlaying = false;
            StartCoroutine(LerpRotation(stopRot, 1f));
        }
        else
        {

            StartCoroutine(LerpRotation(playingRot, 1f));
        }
    }

    public void PlaceRecord(Vinyl vinyl)
    {
        RuntimeManager.PlayOneShot(recorPlaceEvent, transform.position);
        vinyl.transform.SetParent(recordPlacer.transform);
        vinyl.transform.localPosition = Vector3.zero;
        vinyl.transform.localRotation = Quaternion.identity;
        currentRecord = vinyl;
    }


    bool CurrentRecordSelected()
    {
        return currentRecord.selected || currentRecord.inHand;
    }


    void CheckReadyPlaceRecord()
    {
        if (playerLeftHand.isHolding)
        {
            if (playerLeftHand.holdingObj.GetComponent<Vinyl>())
            {
                readyPlacing = true;
            }
            else
            {
                readyPlacing = false;
            }
        }
        else
        {
            readyPlacing = false;
        }


    }

    IEnumerator LerpRotation(Quaternion targetRot, float duration)
    {
        moving = true;
        float time = 0;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(startValue, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = targetRot;
        moving = false;

        if(targetRot == playingRot)
        {
            RuntimeManager.PlayOneShot(needleSound, transform.position);
            isPlaying = true;
        }
    }

}
