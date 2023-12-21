using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GroceryBoxGame : MonoBehaviour
{
    public bool inGameZone;
    public bool gameStarted;
    public bool questAccepted;
    public bool gameSucceeded;
    public int boxScore;
    public GameObject scorePanel;
    public TextMeshProUGUI score;

    public List<GroceryBox> boxes;
    public BuildingGroupController groceryStore;
    public Felix felix;
    public int maxStack;

    float successTimer;


    void Start()
    {
        maxStack = 0;
        successTimer = 0;
        questAccepted = false;
    }

    public void Update()
    {
        if(boxScore == boxes.Count)
        {
            successTimer += Time.deltaTime;
            if(successTimer >= 2)
            {
                gameSucceeded = true;
            }
        }

        if(gameSucceeded)
        {
            DialogueManager.StartConversation("Prop/GroceryBoxGameComplete");
            groceryStore.activateAll = true;
            scorePanel.SetActive(false);
            felix.ChangeFelixCompleteDialogue();
            string reTriggerName = "NPC/Frank/Main_Talked";
            Debug.Log(DialogueLua.GetVariable(reTriggerName).asBool);
            GetComponent<BoxCollider>().enabled = false;
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameStarted)
        {
            scorePanel.SetActive(true);
            boxScore = FindHighest();
            score.text = "Stacks: "+boxScore +"/"+boxes.Count;

        }
    }

    int FindHighest()
    {
        int i = 0;
        foreach(GroceryBox box in boxes)
        {
            if(box.groundBox && box.boxAbove)
            {
                if(box.stackCount > i)
                {
                    i = box.stackCount;
                }
            }
        }
        return i;
    }

    public void SetBoxQuestStarted()
    {
        questAccepted = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGameZone = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGameZone = false;
            gameStarted = false;
            scorePanel.SetActive(false);
        }
    }
}
