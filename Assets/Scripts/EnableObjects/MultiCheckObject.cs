using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCheckObject : LivableObject
{
    public List<LivableObject> checks;
    public List<BuildingGroupController> bgcs;


    protected override void Update()
    {
        base.Update();
        activated = CheckAllChecks();
    }

    bool CheckAllChecks()
    {
        if(checks.Count > 0)
        {
            foreach (LivableObject obj in checks)
            {
                if (!obj.activated)
                {
                    return false;
                }
            }
            return true;
        }
        if(bgcs.Count > 0)
        {
            foreach(BuildingGroupController bgc in bgcs)
            {
                if (!bgc.activateAll)
                {
                    return false;
                }
            }
            return true;
        }
        return false;

    }
}
