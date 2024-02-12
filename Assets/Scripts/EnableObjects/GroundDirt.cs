using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDirt : MonoBehaviour
{
    public int sweepTimes;
    public int dirtStage;
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
    bool[] checkBoundVisible;
    Vector3 pointOnScreen;
    PlayerHolding playerHolding;

    void Start()
    {
        sweepTimes = Random.Range(2, 4);
        rend = GetComponent<Renderer>();
        dirtMat = rend.material;
        alphaVal = dirtMat.GetFloat("_AlphaClipThreshold");
        startAlphaVal = alphaVal;
        stageAlphaVal = startAlphaVal;
        broom = ReferenceTool.broom;
        playerHolding = ReferenceTool.playerHolding;
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
            playerHolding.dirtObj = gameObject;

        }
        else
        {
            sweepable = false;
            if(broom.selectedDirt == this)
            {
                broom.selectedDirt = null;
                playerHolding.dirtObj = null;
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
                if (broom.selectedDirt == this)
                    broom.selectedDirt = null;
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

    bool CheckPointInView(Vector3 pointPos)
    {
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(pointPos);
        if ((pointOnScreen.x < Screen.width * 0.05f) || (pointOnScreen.x > Screen.width * 0.95f) ||
           (pointOnScreen.y < Screen.height * 0.05f) || (pointOnScreen.y > Screen.height * 0.95f))
        {
            return false;
        }
        return true;
    }

    protected virtual bool IsInView()
    {
        pointOnScreen = Camera.main.WorldToScreenPoint(rend.bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            return false;
        }

        //Is in FOV
        if (centerFocused)
        {
            int pointsInScreen = 0;
            Vector3 pointA = rend.bounds.min;
            Vector3 pointB = rend.bounds.min + new Vector3(rend.bounds.size.x, 0, 0);
            Vector3 pointC = rend.bounds.min + new Vector3(0, rend.bounds.size.y, 0);
            Vector3 pointD = rend.bounds.min + new Vector3(0, 0, rend.bounds.size.z);
            Vector3 pointE = rend.bounds.max - new Vector3(rend.bounds.size.x, 0, 0);
            Vector3 pointF = rend.bounds.max - new Vector3(0, rend.bounds.size.y, 0);
            Vector3 pointG = rend.bounds.max - new Vector3(0, 0, rend.bounds.size.z);
            Vector3 pointH = rend.bounds.max;


            checkBoundVisible[0] = CheckPointInView(pointA);
            checkBoundVisible[1] = CheckPointInView(pointB);
            checkBoundVisible[2] = CheckPointInView(pointC);
            checkBoundVisible[3] = CheckPointInView(pointD);
            checkBoundVisible[4] = CheckPointInView(pointE);
            checkBoundVisible[5] = CheckPointInView(pointF);
            checkBoundVisible[6] = CheckPointInView(pointG);
            checkBoundVisible[7] = CheckPointInView(pointH);

            for (int i = 0; i < checkBoundVisible.Length; i++)
            {
                if (checkBoundVisible[i])
                    pointsInScreen++;
            }

            if (pointsInScreen < 3)
                return false;

        }
        else
        {
            if ((pointOnScreen.x < Screen.width * 0.2f) || (pointOnScreen.x > Screen.width * 0.8f) ||
               (pointOnScreen.y < Screen.height * 0.2f) || (pointOnScreen.y > Screen.height * 0.8f))
            {
                return false;
            }

        }

        if (!centerFocused)
        {
            if (rend != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position - new Vector3(0, 0.1f, 0), rend.bounds.center - (Camera.main.transform.position - new Vector3(0, 0.1f, 0)), out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.name != gameObject.name && !hit.collider.CompareTag("Player"))
                    {
                        return false;
                    }

                }
            }
        }

        return true;
    }

    public void OnBecameVisible()
    {
        isVisible = true;
    }

    public void OnBecameInvisible()
    {
        isVisible = false;
    }
}
