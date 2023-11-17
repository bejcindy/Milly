using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : LivableObject
{

    public Animator faucetWater;
    Animator faucetHandleAnim;
    public bool waterOn;
    public bool controlCD;
    public float cdVal = 2f;

    protected override void Start()
    {
        base.Start();
        faucetHandleAnim = GetComponent<Animator>();
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
                }

                else
                {
                    faucetWater.SetTrigger("On");
                    faucetHandleAnim.SetTrigger("On");
                    waterOn = true;
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
