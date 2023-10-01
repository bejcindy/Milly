using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CigaretteContainer : MonoBehaviour
{
    public int cigCounts;
    public Transform player;
    PlayerHolding playerHolding;
    
    public GameObject fullCig;

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
            if(cigCounts > 0 && !playerHolding.smoking && !playerHolding.fullHand)
            {
                cigCounts--;
                GameObject newCig = GameObject.Instantiate(fullCig);
                newCig.GetComponent<LivableObject>().activated = true;
                newCig.GetComponent<Rigidbody>().isKinematic = true;
                newCig.GetComponent<Cigarette>().inHand = true;
                if (playerHolding.GetLeftHandSmoking())
                {
                    playerHolding.OccupyLeft(newCig.transform);
                }

                else if (playerHolding.GetRightHandSmoking())
                {
                    playerHolding.OccupyRight(newCig.transform);
                }
            }
        }
    }
}
