using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCFSM
{
    [CreateAssetMenu(menuName = "NPCFSM/State")]
    public sealed class State: BaseState
    {
        public List<NPCFSMAction> Action = new();
        public List<NPCTransition> Transitions = new();

        public override void Execute(BaseStateMachine machine)
        {
            foreach (var action in Action)
            {
                action.Execute(machine);
            }
            foreach(var transition in Transitions)
            {
                transition.Execute(machine);
            }
        }
    }

}
