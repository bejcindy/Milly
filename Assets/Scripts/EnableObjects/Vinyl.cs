using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vinyl : PickUpObject
{
    public RecordPlayer recordPlayer;
    public VinylHolder holder;
    public LocationController tattooZone;
    public bool onRecordPlayer;
    public bool onStand;
    public bool standSelect;
    public GameObject mySong;
    public GameObject songInfo;
    public bool listened;
    public bool loyiAtVinyl;
    public float playTime;
    bool listneCountAdded;
    bool hadDialogue;

    bool notInCD;
    float placedCDVal = 2f;
    GameObject vinylDialogue;

    protected override void Awake()
    {
        base.Awake();
        songInfo = transform.GetChild(1).gameObject;
        songInfo.SetActive(false);
        vinylDialogue = transform.GetChild(2).gameObject; 
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        notInCD = true;
        objType = HandObjectType.DOUBLE; 
        recordPlayer = ReferenceTool.recordPlayer;
        mySong = transform.GetChild(0).gameObject;
        if (onStand)
        {
            holder = transform.parent.GetComponent<VinylHolder>();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        loyiAtVinyl = DialogueLua.GetVariable("Vinyl/LoyiInBF").asBool;
        if (!hadDialogue)
        {
            if (loyiAtVinyl && tattooZone.inZone)
            {
                if (playTime > 20f && !MindPalace.tatMenuOn)
                {
                    vinylDialogue.SetActive(true);
                    hadDialogue = true;
                }
            }
        }
        if (listened && !listneCountAdded)
        {
            listneCountAdded = true;
            recordPlayer.vinylListenCount++;
        }
        if (inHand)
        {
            base.Update();
            onStand = false;
            standSelect = false;
            onRecordPlayer = false;
            if (recordPlayer.currentRecord == this)
                recordPlayer.currentRecord = null;
            activated = true;
            if (transform.localScale.x < 2)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2, 2, 2), 0.5f);
            }

        }
        else
        {
            if (transform.localScale.x > 1)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.5f);
            }
        }


        if (onRecordPlayer)
        {
            onStand = false;
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
                if(loyiAtVinyl)
                    playTime += Time.deltaTime;

                if (playerHolding.CheckInteractable(gameObject))
                    playerHolding.RemoveInteractable(gameObject);
                if (playerHolding.selectedObj == this)
                    playerHolding.selectedObj = null;
                selected = false;


                if (!mySong.activeSelf)
                {
                    listened = true;
                    mySong.SetActive(true);
                }

                transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
            }
            else
            {
                playTime = 0;
                if(!recordPlayer.moving && !notInCD)
                    base.Update();
                if (mySong.activeSelf)
                {
                    mySong.SetActive(false);
                    
                    if (loyiAtVinyl && !MindPalace.tatMenuOn)
                    {
                        DialogueLua.SetVariable("Vinyl/LoyiSongChange", Random.Range(0, 2));
                        DialogueManager.StartConversation("Vinyl/CancelSong");
                    }

                }
            }
        }
        else if(!onStand)
        {
            notInCD = true;
            base.Update();
            if (mySong.activeSelf)
            {
                mySong.SetActive(false);
            }

        }
        else if(onStand)
        {
            notInCD = true;
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

    public override void LoadData(GameData data)
    {
        if (data.livableDict.TryGetValue(id, out LivableValues values))
        {
            activated = values.activated;
            transformed = values.transformed;
            if (activated)
            {
                matColorVal = 0;
                if (mat.HasFloat("_WhiteDegree"))
                    mat.SetFloat("_WhiteDegree", matColorVal);
                firstActivated = true;
            }            
        }
        if (data.vinylDict.TryGetValue(id, out bool savedHadDialogue))
        {
            hadDialogue = savedHadDialogue;
        }
    }

    public override void SaveData(ref GameData data)
    {
        if (id == null)
            Debug.LogError(gameObject.name + " ID is null.");
        if (id == "")
            Debug.LogError(gameObject.name + " ID is empty.");

        if (data.livableDict.ContainsKey(id))
            data.livableDict.Remove(id);
        LivableValues values = new LivableValues(activated, transformed);
        data.livableDict.Add(id, values);

        if (data.vinylDict.ContainsKey(id))
            data.vinylDict.Remove(id);
        data.vinylDict.Add(id, hadDialogue);
    }
}
