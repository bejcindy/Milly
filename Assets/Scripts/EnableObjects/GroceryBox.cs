using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class GroceryBox : PickUpObject
{
    Collider groBoxCollider;

    [Foldout("GroceryBox")]
    public int stackCount;
    public float upDetectDist;
    public bool boxAbove;
    public bool boxBelow;
    public bool groundBox;
    public LayerMask flatGround;
    [SerializeField] GroceryBoxGame game;
    [SerializeField] GroceryBox upperBox, lowerBox;
    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.DOUBLE;
        groBoxCollider = GetComponent<Collider>();
    }

    protected override void Update()
    {
        if(game.questAccepted)
            base.Update();
        if (inHand)
        {
            groBoxCollider.enabled = false;
            boxAbove = false;
            boxBelow = false;
            groundBox = false;
        }
        else
        {
            groBoxCollider.enabled = true;
        }
    }

    public void FixedUpdate()
    {
        if (game.inGameZone && !rb.isKinematic)
        {
            BoxCollisionDetection();
            groundBox = Physics.Raycast(transform.position, Vector3.down, upDetectDist, flatGround);
            
            if(game.gameStarted) 
                stackCount = CalculateBoxChain(1, this);
        }
        else
        {
            boxAbove = false;
            boxBelow = false;
            groundBox = false;
        }
    }


    void BoxCollisionDetection()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, upDetectDist))
        {
            if (hit.collider.gameObject.CompareTag("GroceryBox"))
            {
                boxAbove = true;
                upperBox = hit.collider.gameObject.GetComponent<GroceryBox>();
            }

            else
            {
                boxAbove = false;
                upperBox = null;
            }


        }
        else
        {
            boxAbove = false;
            upperBox = null;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out hit, upDetectDist))
        {
            if (hit.collider.gameObject.CompareTag("GroceryBox"))
            {
                if (!boxAbove && !boxBelow && !groundBox)
                    game.gameStarted = true;
                boxBelow = true;
                lowerBox = hit.collider.gameObject.GetComponent<GroceryBox>();
            }
            else
            {
                boxBelow = false;
                lowerBox = null;
            }

        }
        else
        {
            boxBelow = false;
            lowerBox = null;
        }
    }

    public int CalculateBoxChain(int count, GroceryBox box)
    {
        if (!box.boxAbove)
        {
            return count;
        }
        else
        {
            return (CalculateBoxChain(1+count, box.upperBox));
        }
    }
}
