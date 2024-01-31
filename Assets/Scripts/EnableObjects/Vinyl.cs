using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vinyl : PickUpObject
{
    public RecordPlayer recordPlayer;
    public bool onRecordPlayer;
    public bool onStand;
    public bool standSelect;
    public GameObject mySong;

    bool notInCD;
    float placedCDVal = 2f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        notInCD = true;
        objType = HandObjectType.DOUBLE; 
        recordPlayer = ReferenceTool.recordPlayer;
        mySong = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (inHand)
        {
            onStand = false;
            standSelect = false;
            onRecordPlayer = false;
            if (recordPlayer.currentRecord == this)
                recordPlayer.currentRecord = null;
            activated = true;
            if(transform.localScale.x < 2)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2, 2, 2), 0.5f);
            }

        }
        else
        {
            if (transform.localScale.x >1)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.5f);
            }
        }

        if (onRecordPlayer)
        {
            if (activated)
                gameObject.layer = 17;
            else
                gameObject.layer = 0;

            if (notInCD)
            {
                if(placedCDVal > 0)
                {
                    placedCDVal -= Time.deltaTime;
                }
                else
                {
                    notInCD = false;
                    placedCDVal = 2f;
                }
            }

            if (recordPlayer.isPlaying)
            {
                if (playerHolding.CheckInteractable(gameObject))
                    playerHolding.RemoveInteractable(gameObject);
                if (playerHolding.selectedObj == this)
                    playerHolding.selectedObj = null;
                selected = false;


                if (!mySong.activeSelf)
                    mySong.SetActive(true);
                transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
            }
            else
            {
                if(!recordPlayer.moving && !notInCD)
                    base.Update();
                if (mySong.activeSelf)
                {
                    mySong.SetActive(false);
                }
            }
        }
        else if (!onStand)
        {
            notInCD = true;
            base.Update();
            if (mySong.activeSelf)
            {
                mySong.SetActive(false);
            }
        }

        //if (onStand && standSelect)
        //{
        //    base.Update();
        //}
        //else if(onStand && !standSelect)
        //{
        //    playerHolding.RemoveInteractable(gameObject);
        //    selected = false;
        //    if (playerHolding.selectedObj == this)
        //        playerHolding.selectedObj = null;
        //    if (activated)
        //        gameObject.layer = 17;
        //    else
        //        gameObject.layer = 0;
        //}

    }

    public void CheckPlaceVinyl()
    {
        if(recordPlayer.readyPlacing)
        {
            onRecordPlayer = true;
            rb.isKinematic = true;
            recordPlayer.PlaceRecord(this);
        }
    }
}
