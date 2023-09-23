using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCFSM
{
    public class BaseState : ScriptableObject
    {
        public virtual void Execute(BaseStateMachine machine) { }
    }
}
