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
        isHolding = !playerHolding.GetLeftHand();
        tableControlOn = false;
    }

    // Update is called once per frame
    void Update()
    {
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
