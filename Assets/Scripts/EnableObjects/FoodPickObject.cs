using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class FoodPickObject : MonoBehaviour
{
    public enum FoodType
    {
        Takosan,
        RoastedGarlic,
        CucumberTofu,
        OcraTofu,
        TomatoTofu
    }

    public static bool takosanPicked;
    public static bool roastedGarlicPicked;
    public static bool cucumberTofuPicked;
    public static bool ocraTofuPicked;
    public static bool tomatoTofuPicked;



    public static bool takosanAte;
    public static bool roastedGarlicAte;
    public static bool cucumberTofuAte;
    public static bool ocraTofuAte;
    public static bool tomatoTofuAte;


    public FoodType foodType;
    public bool selected;
    public TableController myTable;
    PlayerLeftHand leftHand;
    PlayerHolding playerHolding;

    public Vector3 inChopRot;
    public bool picked;

    void Start()
    {
        leftHand = ReferenceTool.playerLeftHand;
        playerHolding = ReferenceTool.playerHolding;
    }

    void Update()
    {
        if (leftHand.chopAiming)
        {
            DetectChopPick();
        }
        else
        {
            selected = false;
        }

        if (selected && !picked)
        {
            gameObject.layer = 9;
            leftHand.selectedFood = this.transform;
        }
        else if(myTable.tableControlOn && !picked)
        {
            gameObject.layer = 0;
            if (leftHand.selectedFood == this.transform)
                leftHand.selectedFood = null;
        }
        else if(picked)
        {
            gameObject.layer = 7;
            if(leftHand.selectedFood == this.transform)
            {
                if(!playerHolding.inDialogue && (PlayerLeftHand.foodAte < 3 || PlayerLeftHand.foodAte >3))
                    CheckFirstPicked();
                RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Chopsticks_PickFood", transform.position);
                leftHand.selectedFood = null;
            }
        }
        else if (!MainQuestState.firstGloriaTalk)
        {
            gameObject.layer = 0;
        }

    }

    void CheckFirstPicked()
    {
        switch (foodType)
        {
            case FoodType.Takosan:
                if (!takosanPicked)
                {
                    takosanPicked = true;
                    DialogueManager.StartConversation("Food/" + foodType + "/Picked");
                }
                break;

            case FoodType.RoastedGarlic:
                if (!roastedGarlicPicked)
                {
                    DialogueManager.StartConversation("Food/" + foodType + "/Picked");
                }
                roastedGarlicPicked = true; break;
            case FoodType.CucumberTofu:
                if (!cucumberTofuPicked)
                {
                    DialogueManager.StartConversation("Food/" + foodType + "/Picked");
                }
                cucumberTofuPicked = true; break;
            case FoodType.OcraTofu:
                if (!ocraTofuPicked)
                {
                    DialogueManager.StartConversation("Food/" + foodType + "/Picked");
                }
                ocraTofuPicked = true; break;
            case FoodType.TomatoTofu:
                if (!tomatoTofuPicked)
                {
                    DialogueManager.StartConversation("Food/" + foodType + "/Picked");
                }
                tomatoTofuPicked = true; break;

            default: break;
        }
    }

    public void CheckFirstAte()
    {
        switch (foodType)
        {
            case FoodType.Takosan:
                if (!takosanAte)
                {
                    takosanAte = true;
                    DialogueManager.StartConversation("Food/" + foodType + "/Ate");
                }
                break;

            case FoodType.RoastedGarlic:
                if (!roastedGarlicAte)
                {
                    DialogueManager.StartConversation("Food/" + foodType + "/Ate");
                }
                roastedGarlicAte = true; break;
            case FoodType.CucumberTofu:
                if (!cucumberTofuAte)
                {
                    DialogueManager.StartConversation("Food/" + foodType + "/Ate");
                }
                cucumberTofuAte = true; break;
            case FoodType.OcraTofu:
                if (!ocraTofuAte)
                {
                    DialogueManager.StartConversation("Food/" + foodType + "/Ate");
                }
                ocraTofuAte = true; break;
            case FoodType.TomatoTofu:
                if (!tomatoTofuAte)
                {
                    DialogueManager.StartConversation("Food/" + foodType + "/Ate");
                }
                tomatoTofuAte = true; break;

            default: break;
        }
    }


    public void DetectChopPick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == this.name)
            {
                selected = true;
            }
            else
            {
                selected = false;
            }
        }
    }

    public IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;

    }

    public IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = endValue;
    }


}
