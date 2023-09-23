using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCFSM
{
    [CreateAssetMenu(menuName = "NPCFSM/Transition")]
    public sealed class NPCTransition: ScriptableObject
    {
        public Decision _decision;
        public BaseState _trueState;
        public BaseState _falseState;

        public void Execute(BaseStateMachine machine)
        {
            if(_decision.Decide(machine) && (_trueState is not RemainInState))
            {
                machine.CurrentState = _trueState;
            }
            else if (_falseState is not RemainInState)
            {
                machine.CurrentState= _falseState;
            }
        }
    }
}
