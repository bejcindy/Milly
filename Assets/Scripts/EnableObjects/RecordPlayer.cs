using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class RecordPlayer : LivableObject
{
    [Foldout("RecordPlayer")]
    public bool hasRecord;
    public Vinyl currentRecord;

    public Transform recordPos;


    public Quaternion playingRot;
    public Quaternion stopRot;
    public bool isPlaying;

    bool moving;
    protected override void Start()
    {
        base.Start();
        isPlaying = true;
        hasRecord = true;
        playingRot = transform.localRotation;
        stopRot = Quaternion.Euler(0f, 0f, 0f);
    }

    protected override void Update()
    {
        base.Update();

        if (interactable)
        {
            if (!moving)
            {
                rend.gameObject.layer = 9;
                if (Input.GetMouseButtonDown(0))
                {
                    TriggerPlay();
                }
            }
            else
            {
                rend.gameObject.layer = 0;
            }

        }
        else
        {
            rend.gameObject.layer = 0;
        }
    }

    void TriggerPlay()
    {
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
            isPlaying = true;
        }
    }

}
