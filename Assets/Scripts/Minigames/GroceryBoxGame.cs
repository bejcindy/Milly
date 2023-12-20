using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GroceryBoxGame : MonoBehaviour
{
    public bool inGameZone;
    public bool gameStarted;
    public int boxScore;
    public GameObject scorePanel;
    public TextMeshProUGUI score;

    public List<GroceryBox> boxes;
    public int maxStack;


    void Start()
    {
        maxStack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            boxScore = FindHighest();
            score.text = "Box: "+boxScore +"/"+boxes.Count;

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGameZone = true;
            scorePanel.SetActive(true);
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
