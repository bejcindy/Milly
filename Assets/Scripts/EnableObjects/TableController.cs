using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public bool atTable;
    public bool isHolding;
    public bool tableControlOn;
    public PickUpObject[] objects;

    public GameObject[] plates;
    PlayerHolding playerHolding;

    public bool firstTableOn;
    public Chopsticks chopsticks;
    // Start is called before the first frame update
    void Start()
    {
        objects = GetComponentsInChildren<PickUpObject>();
        playerHolding = ReferenceTool.playerHolding;

        tableControlOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        atTable = playerHolding.atTable;
        isHolding = !playerHolding.GetLeftHand();
        if (atTable)
        {
            if (tableControlOn)
            {
                if (!firstTableOn)
                    ChooseChopsFirst();
            }
        }
        else
        {
            tableControlOn = false;

        }
        playerHolding.tableControl = tableControlOn;

        if (StartSequence.noControl)
            tableControlOn = false;

    }

    public void ChooseChopsFirst()
    {
        firstTableOn = true;
        playerHolding.AddInteractable(chopsticks.gameObject);
        chopsticks.selected = true;
        playerHolding.selectedObj = chopsticks.gameObject;
    }

    public void DeactivateTable()
    {
        tableControlOn = false;
        foreach (GameObject plate in plates)
        {
            plate.layer = 0;
        }
        playerHolding.ClearPickUp();
        atTable = false;
    }

    
    public void TurnOnTable()
    {
        tableControlOn = true;
    }
}
