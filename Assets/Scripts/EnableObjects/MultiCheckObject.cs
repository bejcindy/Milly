using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCheckObject : LivableObject
{
    public List<LivableObject> checks;


    protected override void Update()
    {
        base.Update();
        activated = CheckAllChecks();
    }

    bool CheckAllChecks()
    {
        foreach(LivableObject obj in checks)
        {
            if (!obj.activated)
            {
                return false;
            }
        }
        return true;
    }
}
