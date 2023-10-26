using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public bool inZone;
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
        if (inZone)
            TurnOnScripts();
        else
            TurnOffScripts();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
