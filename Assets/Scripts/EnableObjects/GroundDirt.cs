//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDirt : MonoBehaviour,ISaveSystem
{
    public int sweepTimes;
    public bool sweepable;
    public bool cleaned;
    public bool sweeped;
    public bool centerFocused;
    public bool playerEntered;
    public bool isVisible;
    public Broom broom;

    float alphaVal;
    float startAlphaVal;
    float stageAlphaVal;
    Material dirtMat;
    Renderer rend;
    string id;

    void Awake()
    {
        if (GetComponent<ObjectID>())
            id = GetComponent<ObjectID>().id;
        else
            Debug.LogError(gameObject.name + " doesn't have ObjectID Component.");
        rend = GetComponent<Renderer>();
        dirtMat = rend.material;
    }

    void Start()
    {
        sweepTimes = Random.Range(2, 4);       
        alphaVal = dirtMat.GetFloat("_AlphaClipThreshold");
        startAlphaVal = alphaVal;
        stageAlphaVal = startAlphaVal;
        broom = ReferenceTool.broom;
    }


    void Update()
    {
        if (sweeped && !cleaned)
        {
            CleanDirtAlpha();
        }

        if(playerEntered && isVisible && Broom.hasBroom && !cleaned)
        {
            sweepable = true;
            broom.selectedDirt = this;


        }
        else
        {
            sweepable = false;
            if(broom.selectedDirt == this)
            {
                broom.selectedDirt = null;
            }
        }
    }

    float CalculateTargetAlpha()
    {
        return stageAlphaVal + ((1 - startAlphaVal) / sweepTimes);
    }

    void CleanDirtAlpha()
    {
        if(alphaVal < CalculateTargetAlpha())
        {
            alphaVal += 0.5f * Time.deltaTime;
            dirtMat.SetFloat("_AlphaClipThreshold", alphaVal);
        }
        else
        {
            if (alphaVal < 1)
            {
                stageAlphaVal = alphaVal;
                sweeped = false;
            }
            else
            {
                cleaned = true;
                this.enabled = false;
            }

        }
    }

    public void SweepDirt()
    {
        sweeped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = false;
        }
    }


    public void OnBecameVisible()
    {
        isVisible = true;
    }

    public void OnBecameInvisible()
    {
        isVisible = false;
    }

    public void LoadData(GameData data)
    {
        if (data.groundDirtDict.TryGetValue(id, out bool savedCleaned))
        {
            cleaned = savedCleaned;
            if (cleaned)
            {
                alphaVal = 1;
                dirtMat.SetFloat("_AlphaClipThreshold", alphaVal);
                enabled = false;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.groundDirtDict.ContainsKey(id))
            data.groundDirtDict.Remove(id);
        data.groundDirtDict.Add(id, cleaned);
    }
}
