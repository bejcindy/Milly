using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public bool inZone;

    bool turningFinished;
    public LivableObject[] allLivable;
    public GroupMaster[] allGroupMaster;
    // Start is called before the first frame update
    void Start()
    {
        allLivable = GetComponentsInChildren<LivableObject>();
        allGroupMaster = GetComponentsInChildren<GroupMaster>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TurnOnScripts();
            inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TurnOffScripts();
            inZone = false;
        }
    }

    private void TurnOnScripts()
    {
        foreach(LivableObject obj in allLivable)
        {
            obj.enabled = true;
        }
    }

    private void TurnOffScripts()
    {
        foreach(LivableObject obj in allLivable)
        {
            obj.enabled = false;
        }
    }
}
