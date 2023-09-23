using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPCFSM
{
    public class BaseStateMachine: MonoBehaviour
    {
        [SerializeField] private BaseState _initialState;
        private Dictionary<Type, Component> _cachedComponents;
        private NPCDestinations _destinations;

        private void Awake()
        {
            CurrentState = _initialState;
            _cachedComponents = new Dictionary<Type, Component>();
            _destinations = GetComponent<NPCDestinations>();
        }

        [SerializeField] public BaseState CurrentState { get; set; }

        private void Update()
        {
            CurrentState.Execute(this);
            Debug.Log(CurrentState);
            Debug.Log("REACHED DEST: " + _destinations.HasReached(GetComponent<NavMeshAgent>()));
        }

        public new T GetComponent<T>() where T : Component
        {
            if (_cachedComponents.ContainsKey(typeof(T)))
                return _cachedComponents[typeof(T)] as T;

            var component = base.GetComponent<T>();
            if (component != null)
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component;
        }

        public NPCDestinations GetDestinations()
        {
            return _destinations;
        }
    }
}
