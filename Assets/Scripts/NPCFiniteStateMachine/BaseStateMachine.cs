using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPCFSM
{
    public class BaseStateMachine: MonoBehaviour
    {

        [SerializeField] State currentState;
        public State initialState;
        public FrozenState frozenState= new FrozenState();
        public MoveState moveState= new MoveState();
        public IdleState idleState = new IdleState();
        public TalkState talkState = new TalkState();
        public FinalState finalState = new FinalState();

        public Transform player;


        private Dictionary<Type, Component> _cachedComponents;
        private NPCDestinations destinations;

        private Animator anim;
        private DialogueSystemTrigger dialogue;
        private NPCControl npcControl;
        private NavMeshAgent agent;
        private Coroutine lookCoroutine;

        private void Awake()
        {

            _cachedComponents = new Dictionary<Type, Component>();

        }

        private void Start()
        {
            ChangeState(initialState);
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            destinations = GetComponent<NPCDestinations>();
            npcControl = GetComponent<NPCControl>();
            dialogue = GetComponent<DialogueSystemTrigger>();
            player = GameObject.Find("Player").transform;
        }

        private void Update()
        {
            Debug.Log(currentState);
            if (currentState != null)
                currentState.OnStateUpdate();


        }

        public void ChangeState(State newState)
        {
            if(currentState != null)
            {
                currentState.OnStateExit();
            }
            currentState = newState;
            currentState.OnStateEnter(this);
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
            return destinations;
        }

        #region GeneralFunctionalityRegion


        public void RotateNPC(Transform target)
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            StartCoroutine(RotateTowards(target));  
        }

        public void StopRotatingNPC()
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
        }

        public IEnumerator RotateTowards(Transform target)
        {
            Debug.Log("Rotating towards " + target);
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            float time = 0;
            while (time < 2)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
                time += Time.deltaTime * 0.1f;
                yield return null;
            }
        }
        #endregion


        #region NPCStateCheckRegion
        public void ActivateNPC()
        {
            npcControl.npcActivated = true;
        }

        public bool CheckNPCActivation()
        {
            return npcControl.npcActivated;
        }

        public void BeginIdling()
        {
            npcControl.idling = true;
            npcControl.idleTime = destinations.GetWaitTime();
            npcControl.idleAction = destinations.GetWaitAction();
        }

        public void PauseIdling()
        {
            if(npcControl.idling)
                npcControl.idlePaused = true;
        }

        public void UnPauseIdling()
        {
            npcControl.idlePaused = false;
        }
        public void InvokeIdleFunction()
        {

            npcControl.InvokeIdleFunction();
        }

        public bool CheckIdleFinished()
        {
            return !npcControl.idling;
        }

        public void FinishNpc()
        {
            gameObject.SetActive(false);
        }
        #endregion



        #region NavMeshControlRegion
        public void StopNavigation()
        {
            agent.isStopped = true;
        }

        public void BeginNavigation()
        {
            agent.isStopped = false;
        }

        public void SetNPCDestination()
        {
            if (!destinations.FinalStop())
            {
                agent.SetDestination(destinations.GetNext().position);
                destinations._counter++;
            }

        }

        public bool CheckReachDestination()
        {
            return destinations.HasReached(agent);
        }

        public bool CheckPathFinished()
        {
            return destinations.FinalStop();
        }

        public Transform GetCurrentDestination()
        {
            return destinations.GetCurrentDestination();
        }

        #endregion

        #region AnimationControlRegion
        public void SetAnimatorTrigger(string trigger)
        {

            anim.SetTrigger(trigger);
        }

        public void ResetAnimTrigger(string trigger)
        {
            anim.ResetTrigger(trigger);
        }
        #endregion

        #region DialogueSystemRegion

        public void StartConversation()
        {
            dialogue.enabled = true;
        }

        public void EndConversation()
        {
            dialogue.enabled = false;
        }

        public bool CheckNPCInDialogue()
        {
            return npcControl.inConversation;
        }

        public bool RetriggerConversation()
        {
            return npcControl.reTriggerConversation;
        }
        #endregion
    }
}
