using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vinyl : PickUpObject
{
    public RecordPlayer recordPlayer;
    public bool onRecordPlayer;
    public GameObject mySong;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.DOUBLE; 
        recordPlayer = ReferenceTool.recordPlayer;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (inHand)
        {
            activated = true;
            onRecordPlayer = false;
            if(recordPlayer.currentRecord == this)
                recordPlayer.currentRecord = null;
        }

        if (onRecordPlayer)
        {

            if (recordPlayer.isPlaying)
            {
                if (playerHolding.CheckInteractable(gameObject))
                    playerHolding.RemoveInteractable(gameObject);
                selected = false;
                gameObject.layer = 0;
                if (!mySong.activeSelf)
                    mySong.SetActive(true);
                transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
            }
            else
            {
                if(!recordPlayer.moving)
                    base.Update();
                if (mySong.activeSelf)
                {
                    mySong.SetActive(false);
                }
            }
        }
        else
        {
            base.Update();
            if (mySong.activeSelf)
            {
                mySong.SetActive(false);
            }
        }

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
