using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Paro
{
    public class AgentState_Patrol : AgentState_Interface
    {
        private readonly Agent_StatePattern agent;
        private int nextWayPoint;
        private Collider[] colliders;
        private Vector3 lookAtPoint;
        private Vector3 heading;
        private float dotProd;

        public AgentState_Patrol(Agent_StatePattern agentStatePattern)
        {
            agent = agentStatePattern;
        }

        public void UpdateState()
        {
            Look();
            Patrol();
        }

        public void ToPatrolState() { }
        public void ToReactionState() { }
        public void ToAlertState()
        {
            agent.currentState = agent.alertState;
        }

        public void ToSpeakingState() { }

        public void ToListeningState() { }

        void Look()
        {
            //check medium range
            colliders = Physics.OverlapSphere(agent.transform.position, agent.sightRange / 3, agent.myAudienceLayers);

            foreach(Collider col in colliders)
            {
                RaycastHit hit;

                VisibilityCalculations(col.transform);

                if(Physics.Linecast(agent.head.position, lookAtPoint, out hit, agent.sightLayers))
                {
                    foreach(string tags in agent.myAudienceTags)
                    {
                        if(hit.transform.CompareTag(tags))
                        {
                            if(dotProd > 0)
                            {
                                AlertStateActions(col.transform);
                                return;
                            }
                        }
                    }
                }
            }
        }

        void Patrol()
        {
            if(agent.flag != null)
            {
                agent.flag.material.color = Color.green;
            }

            if(agent.myFollowTarget != null)
            {
                //agent.currentState = agent.followState;
            }

            if(!agent.myNavMeshAgent.enabled)
            {
                return;
            }

            if(agent.waypoints.Length > 0)
            {
                MoveTo(agent.waypoints[nextWayPoint].position);

                if(HaveIreachedDestination())
                {
                    nextWayPoint = (nextWayPoint + 1) % agent.waypoints.Length;
                }
            }
            else//wander about if there are no waypoints
            {
                if(HaveIreachedDestination())
                {
                    StopWalking();
                    if(RandomWanderTarget(agent.transform.position, agent.sightRange, out agent.wanderTarget))
                    {
                        MoveTo(agent.wanderTarget);
                    }
                }
            }
        }

        void AlertStateActions(Transform target)
        {
            agent.locationOfInterest = target.position;//check for state
            ToAlertState();
        }

        void VisibilityCalculations(Transform target)
        {
            lookAtPoint = new Vector3(target.position.x, target.position.y + agent.offset, target.position.z);
            heading = lookAtPoint - agent.transform.position;
            dotProd = Vector3.Dot(heading, agent.transform.forward);
        }

        bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            NavMeshHit navHit;
            Vector3 randomPoint = center + Random.insideUnitSphere * agent.sightRange;

            if(NavMesh.SamplePosition(randomPoint, out navHit, 3.0f, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }
            else
            {
                result = center;
                return false;
            }
        }

        bool HaveIreachedDestination()
        {
            if(agent.myNavMeshAgent.remainingDistance <= agent.myNavMeshAgent.stoppingDistance && !agent.myNavMeshAgent.pathPending)
            {
                StopWalking();
                return true;
            }
            else
            {
                KeepWalking();
                return false;
            }
        }

        void MoveTo(Vector3 targetPos)
        {
            if(Vector3.Distance(agent.transform.position, targetPos) > agent.myNavMeshAgent.stoppingDistance + 1)
            {
                agent.myNavMeshAgent.SetDestination(targetPos);
                KeepWalking();
            }
        }

        void KeepWalking()
        {
            if(agent != null)
            {
                agent.myNavMeshAgent.isStopped = false;
                agent.agentMaster.CallEventPatrol();
            }
        }

        void StopWalking()
        {
            agent.myNavMeshAgent.isStopped = true;
            agent.agentMaster.CallEventIdle();
        }
    }
}
