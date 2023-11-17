using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class TouchObject : LivableObject
{
    public float coolDown = 5f;
    public bool inCD;

    public Animator toiletAnim;
    public EventReference flushSound;
    FMOD.Studio.EventInstance flushEvent;

    protected override void Start()
    {
        base.Start();
        flushEvent = FMODUnity.RuntimeManager.CreateInstance(flushSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(flushEvent, toiletAnim.transform);
    }
    protected override void Update()
    {
        base.Update();

        if (interactable && !inCD)
        {
            UseTouchable();
        }
        else if(inCD)
        {
            gameObject.layer = 0;
            if(coolDown > 0)
            {
                coolDown -= Time.deltaTime;
            }
            else
            {
                coolDown = 5f;
                inCD = false;
            }
        }
        else
        {
            gameObject.layer = 0;
        }
    }

    void UseTouchable()
    {
        gameObject.layer = 9;
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.layer = 0;
            inCD = true;
            UseResult();
        }
    }

    void UseResult()
    {
        if (!toiletAnim.isActiveAndEnabled)
            toiletAnim.enabled = true;
        else
            toiletAnim.Play("ToiletFlush", 0, 0);

        flushEvent.start();
    }
}
