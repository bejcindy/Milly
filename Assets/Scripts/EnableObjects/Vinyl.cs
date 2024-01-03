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
        mySong = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (inHand)
        {
            if (recordPlayer.currentRecord == this)
                recordPlayer.currentRecord = null;
            activated = true;
            onRecordPlayer = false;
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

            if (recordPlayer.isPlaying)
            {
                if (playerHolding.CheckInteractable(gameObject))
                    playerHolding.RemoveInteractable(gameObject);
                selected = false;

                if(activated)
                    gameObject.layer = 17;
                else
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
