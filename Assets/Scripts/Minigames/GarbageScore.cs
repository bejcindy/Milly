using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Unity.VisualScripting;
using PixelCrushers.DialogueSystem;

public class GarbageScore : MonoBehaviour,ISaveSystem
{
    public static bool dumpsterAimed;
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
    HandObjectType[] acceptedTypes;

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

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.name == gameObject.name)
            {
                dumpsterAimed = true;
            }
            else
            {
                dumpsterAimed = false;
            }
        }
        else
        {
            dumpsterAimed = false;
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        PickUpObject pickUp;
        if (other.gameObject.tag != "CigButt")
            pickUp = other.GetComponent<PickUpObject>();
        else
            pickUp = other.transform.parent.GetComponent<PickUpObject>();
        if (pickUp)
        {
            if (CheckAcceptableObject(pickUp.objType))
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
                        if (pickUp.GetComponent<Collider>() != null)
                            pickUp.GetComponent<Collider>().material = highFriction;
                    }

                    if (throwValid)
                    {
                        score++;
                        pickUp.dumped = true;
                        if(pickUp.GetComponent<Collider>() != null)
                            pickUp.GetComponent<Collider>().material = highFriction;
                    }

                }
            }


        }
    }

    public static bool CheckAcceptableObject(HandObjectType handObjectType)
    {
        if(handObjectType == HandObjectType.TRASH || handObjectType == HandObjectType.DRINK || handObjectType == HandObjectType.CATFOOD || handObjectType == HandObjectType.CIGARETTE)
        {
            return true;
        }
        return false;
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

    public void TriggerMyTat()
    {
        myTat.triggered = true;
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

    public void LoadData(GameData data)
    {
        firstActivated = data.soccerDumpsterOn;
        score = data.soccerScore;
        if (firstActivated)
        {
            matColorVal = 0;
            dumpsterMat.SetFloat("_WhiteDegree", matColorVal);
        }
        throwValidCheckDiaDone = data.trashThrowDiaDone;
    }

    public void SaveData(ref GameData data)
    {
        data.soccerDumpsterOn = firstActivated;
        data.soccerScore = score;
        data.trashThrowDiaDone = throwValidCheckDiaDone;
    }
}
