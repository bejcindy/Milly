using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Faucet : LivableObject
{

    public Animator faucetWater;
    public ParticleSystem water;
    Animator faucetHandleAnim;
    public bool waterOn;
    public bool controlCD;
    public float cdVal = 2f;

    public EventReference waterRunning;
    FMOD.Studio.EventInstance waterEvent;
    string faucetUseEvent = "event:/Sound Effects/FaucetHandle";
    string waterDrainEvent = "event:/Sound Effects/ObjectInteraction/Sink_Drain";
    PlayerHolding playerHolding;
    bool setNull;

    protected override void Start()
    {
        base.Start();
        faucetHandleAnim = GetComponent<Animator>();
        waterEvent = FMODUnity.RuntimeManager.CreateInstance(waterRunning);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(waterEvent, faucetWater.transform);
        waterEvent.start();
        playerHolding = GameObject.Find("Player").GetComponent<PlayerHolding>();
    }
    protected override void Update()
    {
        base.Update();

        if (interactable)
        {
            FaucetControl();
        }

        else
        {
            gameObject.layer = 0;
            if (!setNull)
            {
                playerHolding.clickableObj = null;
                setNull = true;
            }
        }
    }

    void FaucetControl()
    {

        if (!controlCD)
        {
            gameObject.layer = 9;
            playerHolding.clickableObj = gameObject;
            setNull = false;
            if (Input.GetMouseButtonDown(0))
            {
                FMODUnity.RuntimeManager.PlayOneShot(faucetUseEvent, transform. position);
                controlCD = true;
                if (waterOn)
                {
                    RuntimeManager.PlayOneShot(waterDrainEvent, water.transform.position);
                    faucetWater.SetTrigger("Off");
                    water.Stop();
                    faucetHandleAnim.SetTrigger("Off");
                    waterOn = false;
                    waterEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                }

                else
                {
                    faucetWater.SetTrigger("On");
                    water.Play();
                    faucetHandleAnim.SetTrigger("On");
                    waterOn = true;
                    waterEvent.start();

                }
            }
        }
        else
        {
            gameObject.layer = 0;
            if (!setNull)
            {
                playerHolding.clickableObj = null;
                setNull = true;
            }
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
