using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMODUnity;

public class CigaretteContainer : MonoBehaviour
{
    public int cigCounts;
    public Transform player;
    public Transform handContainer;
    PlayerHolding playerHolding;
    string cigLightSound = "event:/Sound Effects/ObjectInteraction/Cigarette/Cigarette_Light";


    public GameObject fullCig;

    bool beforeGloriaThought;
    bool afterGloriaThought;
    bool afterParentsThought;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerHolding = player.GetComponent<PlayerHolding>();
    }
     
    // Update is called once per frame
    void Update()
    {
        GetCig();
    }

    public void GetCig()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(cigCounts > 0 && !playerHolding.smoking && !playerHolding.fullHand && !playerHolding.atInterior)
            {
                RuntimeManager.PlayOneShot(cigLightSound, transform.position);
                cigCounts--;
                GameObject newCig = GameObject.Instantiate(fullCig, handContainer.position, handContainer.rotation, handContainer);
                newCig.GetComponent<LivableObject>().activated = true;
                newCig.GetComponent<Rigidbody>().isKinematic = true;
                newCig.GetComponent<Cigarette>().inHand = true;
                if (playerHolding.GetLeftHandSmoking())
                {
                    playerHolding.OccupyLeft(newCig.transform);
                }

                if (!MainQuestState.firstGloriaTalk)
                {
                    if (!beforeGloriaThought)
                    {
                        DialogueManager.StartConversation("Cigarette/BeforeGloria");
                        beforeGloriaThought = true;
                    }

                }
                if(MainQuestState.firstGloriaTalk && !MainQuestState.parentsCalled)
                {
                    if (!afterGloriaThought)
                    {
                        DialogueManager.StartConversation("Cigarette/AfterGloria");
                        afterGloriaThought = true;
                    }

                }
                if(MainQuestState.firstGloriaTalk && MainQuestState.parentsCalled)
                {
                    if (!afterParentsThought)
                    {
                        DialogueManager.StartConversation("Cigarette/AfterCallingParents");
                        afterParentsThought = true;
                    }

                }


            }
        }
    }
}
