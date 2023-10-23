using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public bool atTable;
    public bool isHolding;
    public bool tableControlOn;
    public PickUpObject[] objects;
    PlayerHolding playerHolding;
    // Start is called before the first frame update
    void Start()
    {
        objects = GetComponentsInChildren<PickUpObject>();
        playerHolding = GameObject.Find("Player").GetComponent<PlayerHolding>();
        atTable = playerHolding.atTable;
        tableControlOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        isHolding = !playerHolding.GetLeftHand();
        if (atTable)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !isHolding)
            {
                tableControlOn = !tableControlOn;
                CheckTableControl();
            }
        }
        else
        {
            tableControlOn = false;

        }
        playerHolding.tableControl = tableControlOn;
        

    }

    void CheckTableControl()
    {
        if (tableControlOn)
        {

        }
        else
        {
            playerHolding.ClearPickUp();
        }
    }
}
