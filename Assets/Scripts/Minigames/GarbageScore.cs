using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Unity.VisualScripting;
using PixelCrushers.DialogueSystem;

public class GarbageScore : MonoBehaviour
{
    [SerializeField]
    public int score;
    public bool throwValid;
    public Material dumpsterMat;
    public GameObject dumpsterBody;
    public PhysicMaterial highFriction;
    public CharacterTattoo myTat;

    float matColorVal = 1f;
    float fadeInterval = 10f;
    bool firstActivated,firstTrashIn;
    bool throwValidCheckDiaDone;

    public List<PickUpObject> trashList = new List<PickUpObject>();
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
        {
            dumpsterBody.gameObject.layer = 17;
            TurnOnColor(dumpsterMat);
        }



    }


    private void OnTriggerEnter(Collider other)
    {
        PickUpObject pickUp = other.GetComponent<PickUpObject>();
        if (pickUp)
        {
            if (pickUp.kicked)
            {
                score++;
                pickUp.dumped = true;
                pickUp.GetComponent<Collider>().material = highFriction;
            }
            else
            {
                if (!throwValidCheckDiaDone)
                {
                    throwValidCheckDiaDone = true;
                    DialogueManager.StartConversation("SoccerGame/ThrownObject");
                    pickUp.dumped = true;
                    pickUp.GetComponent<Collider>().material = highFriction;
                }

                if (throwValid)
                {
                    score++;
                    pickUp.dumped = true;
                    pickUp.GetComponent<Collider>().material = highFriction;
                }

            }

        }
    }

    public void ValidateThrow()
    {
        throwValid = true;
    }

    public void InvalidateThrow()
    {
        throwValid = false;
    }

    public void AddScore()
    {
        score++;
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
            myTat.triggered = true;
        }
    }


}
