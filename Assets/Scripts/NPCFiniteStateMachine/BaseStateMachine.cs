using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

namespace NPCFSM
{
    public class BaseStateMachine: MonoBehaviour
    {

        State currentState;
        State previousState;
        Transform player;
        State initialState;
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
        private NavMeshObstacle navObstacle;
        private Coroutine lookCoroutine;

        public CinemachineVirtualCamera charCam;
        public float camXAxis;
        public float camYAxis;
        public CinemachineBrain camBrain;
        Vector3 previousPlayerPos = Vector3.zero;

        public float moveWait = 2f;

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
            player = ReferenceTool.player;
            camBrain = ReferenceTool.playerBrain;
            ChangeState(initialState);
            camXAxis = charCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value;
            camYAxis = charCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value;
        }

        private void Update()
        {
            if (CheckNPCActivation() && gameObject.name == "Charles")
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


        public void SetPreviousState(State newState)
        {
            previousState = newState;
        }

        public State GetPreviousState()
        {
            return previousState;
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

        public bool CheckIfQuestTrigger()
        {
            return npcControl.questTriggered;
        }

        public bool CheckQuestActivated()
        {
            return npcControl.questAccepted;
        }

        public void FakeActivateNPC()
        {
            npcControl.fakeActivated = true;
        }

        public void FakeDeactivateNPC()
        {
            npcControl.fakeActivated = false;
        }

        public void FinishNpc()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region Idle Region
        public void BeginIdling()
        {
            npcControl.idling = true;
            npcControl.SetWaitAction();
            npcControl.SetDialogue();
        }

        public void StartIdling()
        {
            npcControl.idling = true;
        }

        public bool GetIdling()
        {
            return npcControl.idling;
        }
        public void StopIdling()
        {
            npcControl.StopIdle();
        }
        public void InvokeIdleFunction()
        {
            npcControl.InvokeIdleFunction();
        }

        public bool CheckSpecialIdleAnim()
        {
            return npcControl.GetSpecialIdleAnim();
        }

        public void SetIdleRotation()
        {
            npcControl.IdleRotate();
        }

        public void StopRotation()
        {
            npcControl.StopLookRotation();
        }

        public void SetSpecialIdleAnim()
        {
            SetAnimatorTrigger("Special" + npcControl._counter);
        }

        public bool CheckNotIdling()
        {
            return !npcControl.idling;
        }
        #endregion

        #region NavMeshControlRegion
        public bool CheckNavOn()
        {
            return agent.isActiveAndEnabled;
        }
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

        public void IncreaseCounter()
        {
            npcControl._counter++;
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

        public void ResetPath()
        {
            agent.ResetPath();
        }

        public bool CheckPlayerMove()
        {
            return (Vector3.Distance(transform.position, player.position) > toPlayerDistance);
        }

        public void FollowPlayer()
        {
            agent.SetDestination(player.transform.position);
        }

        public bool CheckPlayerInVincinity()
        {
            return (Vector3.Distance(transform.position, player.position) < npcControl.npcVincinity);
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
        public bool CheckInConversation()
        {
            return npcControl.inConversation;
        }

        public bool CheckTalkable()
        {
            return npcControl.CheckTalkable();
        }

        public bool CheckFirstTalked()
        {
            return npcControl.CheckFirstTalked();
        }

        public void SetMainTalkTrue()
        {
            npcControl.SetMainTalkeTrue();
        }

        public void SetMainTalkFalse()
        {
            npcControl.SetMainTalkFalse();
        }

        public bool GetMainTalkTrue()
        {
            return npcControl.GetMainTalked();
        }

        public bool CheckNoMoveAfterTalk()
        {
            return npcControl.CheckNoMoveAfterTalk();
        }

        public bool CheckNoLookInTalk()
        {
            return npcControl.noLookInConvo;
        }

        public void TurnOnCam()
        {
            charCam.m_Priority = 11;
            ReferenceTool.playerMovement.enabled = false;
        }

        public void TurnOffCam()
        {
            charCam.m_Priority = 9;
        }

        public bool CheckBrainBlending()
        {
            return camBrain.IsBlending;
        }

        public void ResetCam()
        {
            charCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = camYAxis;
            charCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = camXAxis;
        }

        public void DelayMove()
        {
            if (moveWait > 0)
                moveWait -= Time.deltaTime;
            else
            {
                ChangeState(moveState);
                moveWait = 2f;
            }
        }
        #endregion
    }
}
