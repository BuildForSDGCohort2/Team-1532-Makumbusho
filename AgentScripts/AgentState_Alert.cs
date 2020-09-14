using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paro
{
    public class AgentState_Alert : AgentState_Interface
    {
        private readonly Agent_StatePattern agent;
        private float informRate = 3;
        private float nextInform;
        private float offset = 0.3f;
        private Vector3 targetPosition;
        private RaycastHit hit;
        private Collider[] colliders;
        private Collider[] partnerColliders;
        private Vector3 lookAtTarget;
        private int detectionCount;
        private int lastDetectionCount;
        private Transform possibleTarget;

        public AgentState_Alert(Agent_StatePattern agentStatePattern)
        {
            agent = agentStatePattern;
        }

        public void UpdateState()
        {
            Look();
        }

        public void ToPatrolState()
        {
            agent.currentState = agent.patrolState;
        }

        public void ToAlertState() { }

        public void ToSpeakingState() { }
        public void ToListeningState() { }
        public void ToReactionState() { }

        void Look()
        {
            colliders = Physics.OverlapSphere(agent.transform.position, agent.sightRange, agent.myAudienceLayers);

            lastDetectionCount = detectionCount;

            foreach(Collider col in colliders)
            {
                lookAtTarget = new Vector3(col.transform.position.x, col.transform.position.y + offset, col.transform.position.z);
                if(Physics.Linecast(agent.head.position, lookAtTarget, out hit, agent.sightLayers))
                {
                    foreach(string tags in agent.myAudienceTags)
                    {
                        if(hit.transform.CompareTag(tags))
                        {
                            detectionCount++;
                            possibleTarget = col.transform;
                            break;
                        }
                    }
                }
            }

            //check if detection count has changed and if not then set it back to 0
            if(detectionCount == lastDetectionCount)
            {
                detectionCount = 0;
            }

            //check if detection count is greater than the required and if so pursue
            if(detectionCount >= agent.requiredDetectionCount)
            {
                detectionCount = 0;
                agent.locationOfInterest = possibleTarget.position;
                agent.pursueTarget = possibleTarget.root;
                InformNearbyPartners();
                
            }
            GoToLocationOfInterest();
        }

        void GoToLocationOfInterest()
        {
            if(agent.flag != null)
            {
                agent.flag.material.color = Color.yellow;

                if(agent.myNavMeshAgent.enabled && agent.locationOfInterest != Vector3.zero)
                {
                    agent.myNavMeshAgent.SetDestination(agent.locationOfInterest);
                    agent.myNavMeshAgent.isStopped = false;
                    agent.agentMaster.CallEventPatrol();

                    if(agent.myNavMeshAgent.remainingDistance <= agent.myNavMeshAgent.stoppingDistance && !agent.myNavMeshAgent.pathPending)
                    {
                        agent.agentMaster.CallEventIdle();
                        agent.locationOfInterest = Vector3.zero;
                        ToPatrolState();
                    }
                }
            }
        }

        void InformNearbyPartners()
        {
            if(Time.time > nextInform)
            {
                nextInform = Time.time + informRate;

                partnerColliders = Physics.OverlapSphere(agent.transform.position, agent.sightRange, agent.myPartnerLayers);

                if(partnerColliders.Length == 0)
                {
                    return;
                }

                foreach(Collider partner in partnerColliders)
                {
                    if(partner.transform.root.GetComponent<Agent_StatePattern>() != null)
                    {
                        Agent_StatePattern partnerPattern = partner.transform.root.GetComponent<Agent_StatePattern>();

                        if(partnerPattern.currentState == partnerPattern.patrolState)
                        {
                            partnerPattern.pursueTarget = agent.pursueTarget;
                            partnerPattern.locationOfInterest = agent.pursueTarget.position;
                            partnerPattern.currentState = partnerPattern.alertState;
                            partnerPattern.agentMaster.CallEventPatrol();
                        }
                    }
                }
            }
        }
    }
}
