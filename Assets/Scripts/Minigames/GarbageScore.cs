using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Unity.VisualScripting;

public class GarbageScore : MonoBehaviour
{
    [SerializeField]
    public int score;
    public Material dumpsterMat;
    float matColorVal = 1f;
    float fadeInterval = 10f;
    bool firstActivated,firstTrashIn;
    // Start is called before the first frame update
    void Start()
    {
        dumpsterMat.SetFloat("_WhiteDegree", matColorVal);
    }

    // Update is called once per frame
    void Update()
    {
        if (score > 0)
            firstTrashIn = true;
        if (!firstActivated && firstTrashIn)
            TurnOnColor(dumpsterMat);


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PickUpObject>())
        {
            score++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PickUpObject>())
        {
            score--;
        }

    }
    void TurnOnColor(Material material)
    {
        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            if (material.HasFloat("_WhiteDegree"))
                material.SetFloat("_WhiteDegree", matColorVal);
            
        }
        else
        {
            matColorVal = 0;
            firstActivated = true;
        }
    }


}
