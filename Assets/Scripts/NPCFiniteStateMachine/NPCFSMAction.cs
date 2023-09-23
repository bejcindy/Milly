using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCFSM
{
    public abstract class NPCFSMAction: ScriptableObject
    {
        public abstract void Execute(BaseStateMachine machine);
    }
}
