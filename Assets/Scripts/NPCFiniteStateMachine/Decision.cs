using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCFSM
{
    public abstract class Decision: ScriptableObject
    {
        public abstract bool Decide(BaseStateMachine machine);
    }
}
