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

        State currentState;
        Transform player;
        public State initialState;
        public char initialStateChar;
        public FrozenState frozenState= new FrozenState();
        public MoveState moveState= new MoveState();
        public IdleState idleState = new IdleState();
        public TalkState talkState = new TalkState();
        public FollowState followState= new FollowState();
        public FinalState finalState = new FinalState();

        [SerializeField] private float toPlayerDistance;


        private Dictionary<Type, Component> _cachedComponents;

        private Animator anim;
        private DialogueSystemTrigger dialogue;
        private NPCControl npcControl;
        private NavMeshAgent agent;
        private Coroutine lookCoroutine;
        Vector3 previousPlayerPos = Vector3.zero;

        private void Awake()
        {

            _cachedComponents = new Dictionary<Type, Component>();

        }

        private void Start()
        {
            initialState = ChooseInitialState(initialStateChar);
            //Debug.Log(initialState);
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            npcControl = GetComponent<NPCControl>();
            dialogue = GetComponent<DialogueSystemTrigger>();
            player = GameObject.Find("Player").transform;
            ChangeState(initialState);
        }

        private void Update()
        {
            if (CheckNPCActivation())
                //Debug.Log(currentState);
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

        public State ChooseInitialState(char c)
        {
            switch (c)
            {
                case 'F':
                    return frozenState;
                case 'T':
                    return talkState;
                case 'M':
                    return moveState;
                case 'I':
                    return idleState;
                default:
                    return frozenState;
            }
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
            npcControl.SetWaitTime();
            npcControl.SetWaitAction();
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

        public void StopIdling()
        {
            npcControl.StopIdle();
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
            if (!npcControl.FinalStop())
            {
                agent.SetDestination(npcControl.GetNext().position);
                npcControl._counter++;
            }

        }

        public bool CheckFollowPlayer()
        {
            return npcControl.followingPlayer;
        }

        public void StopFollowPlayer()
        {
            npcControl.followingPlayer = false;
            agent.stoppingDistance = 0;
        }

        public bool CheckPlayerMove()
        {
            return (Vector3.Distance(transform.position, player.position) > toPlayerDistance);
        }

        public void FollowPlayer()
        {
            agent.SetDestination(player.transform.position);
            //if (previousPlayerPos == Vector3.zero)
            //{
            //    Vector3 newPos = player.position;
            //    agent.SetDestination(newPos);
            //    previousPlayerPos = newPos;
            //    if (!anim.GetBool("Move"))
            //    {
            //        ResetAnimTrigger("Idle");
            //        SetAnimatorTrigger("Move");
            //    }
            //}
            //else
            //{
            //    if(Vector3.Distance(previousPlayerPos, player.position) >= 2f)
            //    {
            //        Vector3 newPos = player.position;
            //        agent.SetDestination(newPos);
            //        previousPlayerPos = newPos;
            //        if (!anim.GetBool("Move"))
            //        {
            //            ResetAnimTrigger("Idle");
            //            SetAnimatorTrigger("Move");
            //        }
            //    }
            //    else
            //    {
            //        if (!anim.GetBool("Idle"))
            //        {
            //            ResetAnimTrigger("Move");
            //            SetAnimatorTrigger("Idle");
            //        }
            //    }
                
            //}

        }

        public bool CheckReachDestination()
        {
            return npcControl.HasReached(agent);
        }

        public bool CheckPathFinished()
        {
            return npcControl.FinalStop();
        }

        public Transform GetCurrentDestination()
        {
            return npcControl.GetCurrentDestination();
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
