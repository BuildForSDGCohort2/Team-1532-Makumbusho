using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Paro
{
    public class Agent_StatePattern : MonoBehaviour
    {
        //used for decision making
        private float checkRate = 0.1f;
        private float nextCheck;
        public float sightRange = 40;
        public float detectBehindRange = 5;
        public float convoRange = 2;
        public float convoRate = 0.4f;
        public float nextConvo;
        public float offset = 0.4f;
        public int requiredDetectionCount = 15;

        public bool hasConvo;
        public bool isInConvo;

        public Transform myFollowTarget;
        [HideInInspector]
        public Transform pursueTarget;
        [HideInInspector]
        public Vector3 locationOfInterest;
        [HideInInspector]
        public Vector3 wanderTarget;
        [HideInInspector]
        public Transform myAudience;

        //Used for sight
        public LayerMask sightLayers;
        public LayerMask myPartnerLayers;
        public LayerMask myAudienceLayers;
        public string[] myAudienceTags;
        public string[] myPartnerTags;

        //references
        public Transform[] waypoints;
        public Transform head;
        public MeshRenderer flag;
        public GameObject voiceBox;
        [HideInInspector]
        public NavMeshAgent myNavMeshAgent;
        public Agent_Master agentMaster;

        //used for state AI
        public AgentState_Interface currentState;
        public AgentState_Interface capturedState;
        public AgentState_Patrol patrolState;
        public AgentState_Listen listenState;
        public AgentState_Speak speakState;
        public AgentState_Follow followState;
        public AgentState_Investigate investigateState;
        public AgentState_Alert alertState;

        private void Awake()
        {
            SetupStateReferences();
            SetInitialReferences();
            
        }

        // Start is called before the first frame update
        void Start()
        {
            SetInitialReferences();
        }

        // Update is called once per frame
        void Update()
        {
            CarryOutUpdateState();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        void SetupStateReferences()
        {
            patrolState = new AgentState_Patrol(this);
            
        }

        void SetInitialReferences()
        {
            if(GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
            ActivatePatrolState();
        }

        void CarryOutUpdateState()
        {
            if(Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                currentState.UpdateState();
            }
        }

        void ActivatePatrolState()
        {
            currentState = patrolState;
        }

    }
}
