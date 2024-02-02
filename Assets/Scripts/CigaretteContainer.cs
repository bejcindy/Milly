using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMODUnity;

public class CigaretteContainer : MonoBehaviour
{
    public bool takingOutCig;
    public float cigWait = 0.5f;
    public int cigCounts;
    public Transform player;
    public Transform handContainer;
    public Transform cigContainer;
    public Cigarette selectedCig;
    public List<Cigarette> cigs;
    public GameObject cigBox;

    PlayerHolding playerHolding;
    PlayerLeftHand leftHand;

    string cigLightSound = "event:/Sound Effects/ObjectInteraction/Cigarette/Cigarette_Light";


    public GameObject fullCig;

    bool beforeGloriaThought;
    bool afterGloriaThought;
    bool afterParentsThought;

    // Start is called before the first frame update
    void Start()
    {
        player = ReferenceTool.player;
        playerHolding = player.GetComponent<PlayerHolding>();
        leftHand = ReferenceTool.playerLeftHand;
        cigs.AddRange(cigContainer.GetComponentsInChildren<Cigarette>());
    }
     
    // Update is called once per frame
    void Update()
    {
        GetCig();

        if (takingOutCig)
        {
            if (cigWait > 0)
            {
                cigWait -= Time.deltaTime;
                selectedCig.transform.position += 0.5f * Time.deltaTime * cigContainer.up;
            }
            else
            {
                cigWait = 0.5f;
                takingOutCig = false;
                RuntimeManager.PlayOneShot(cigLightSound, transform.position);
                playerHolding.OccupyLeft(selectedCig.transform);
                RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Cigarette/CigBox_Close");
                leftHand.gettingCig = false;
                Invoke(nameof(TurnOffBox), 0.5f);
            }

        }

        if(cigs.Count<=0 && cigBox.activeSelf)
        {
            if (cigBox.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                cigBox.SetActive(false);
                leftHand.gettingCig = false;
                this.enabled = false;
            }
            else
            {
                leftHand.gettingCig = true;
            }
        }

    }

    public void ShowCigHint()
    {
        DataHolder.ShowHint(DataHolder.hints.cigHint);
    }

    public void GetCig()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            if(cigs.Count>0 && !leftHand.isHolding && !playerHolding.atInterior && !takingOutCig)
            {
                TakeOutBox();

                if (!MainQuestState.firstGloriaTalk)
                {
                    if (!beforeGloriaThought)
                    {
                        DialogueManager.StartConversation("Cigarette/BeforeGloria");
                        beforeGloriaThought = true;
                    }

                }
                else if(MainQuestState.firstGloriaTalk)
                {
                    if (!afterGloriaThought)
                    {
                        //DialogueManager.StartConversation("Cigarette/AfterGloria");
                        afterGloriaThought = true;
                    }

                }


            }
            else if (cigs.Count <= 0) {
                DialogueManager.StartConversation("Cigarette/Empty");
                cigBox.SetActive(true);
            }
        }
    }

    void TakeOutBox()
    {
        DataHolder.HideHint(DataHolder.hints.cigHint);
        RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Cigarette/CigBox_Open");
        leftHand.gettingCig = true;
        cigBox.SetActive(true);
        int index = Random.Range(0, cigs.Count-1);
        selectedCig = cigs[index];
      
        Invoke(nameof(DelayTakeCig), 0.5f);

    }

    void DelayTakeCig()
    {
        takingOutCig = true;
        selectedCig.transform.SetParent(handContainer);
        cigs.Remove(selectedCig);
        selectedCig.activated = true;
        selectedCig.GetComponent<Rigidbody>().isKinematic = true;
    }


    void TurnOffBox()
    {
        cigBox.gameObject.SetActive(false);
        selectedCig.inBox = false;
        selectedCig.inHand = true;
    }
}
