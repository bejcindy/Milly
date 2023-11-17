using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Faucet : LivableObject
{

    public Animator faucetWater;
    Animator faucetHandleAnim;
    public bool waterOn;
    public bool controlCD;
    public float cdVal = 2f;

    public EventReference waterRunning;
    FMOD.Studio.EventInstance waterEvent;

    protected override void Start()
    {
        base.Start();
        faucetHandleAnim = GetComponent<Animator>();
        waterEvent = FMODUnity.RuntimeManager.CreateInstance(waterRunning);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(waterEvent, faucetWater.transform);
        waterEvent.start();
    }
    protected override void Update()
    {
        base.Update();

        if (interactable)
        {
            FaucetControl();
        }

        else
            gameObject.layer = 0;
    }

    void FaucetControl()
    {

        if (!controlCD)
        {
            gameObject.layer = 9;
            if (Input.GetMouseButtonDown(0))
            {
                controlCD = true;

                if (waterOn)
                {
                    faucetWater.SetTrigger("Off");
                    faucetHandleAnim.SetTrigger("Off");
                    waterOn = false;
                    waterEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                }

                else
                {
                    faucetWater.SetTrigger("On");
                    faucetHandleAnim.SetTrigger("On");
                    waterOn = true;
                    waterEvent.start();

                }
            }
        }
        else
        {
            gameObject.layer = 0;
            if (cdVal > 0)
                cdVal -= Time.deltaTime;
            else
            {
                cdVal = 2f;
                controlCD = false;
            }
        }

    }
}
