using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public NPCControl myNPC;
    public GameObject myObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myNPC.gameObject.SetActive(true);
            gameObject.SetActive(false);
            myObj.SetActive(true);
        }
    }
}
