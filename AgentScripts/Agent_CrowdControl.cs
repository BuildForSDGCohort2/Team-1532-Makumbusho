using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Paro
{
    public class Agent_CrowdControl : MonoBehaviour
    {
        //used for decision making
        private float checkRate = 0.1f;
        private float nextCheck;
        public float sightRange = 40;

        public string agentTag = "Agent";
        public string goalTag = "Goal";
        public List<GameObject> goalLocations;

        
        public List<GameObject> agentIdentities;
        // Start is called before the first frame update
        void Start()
        {
            SetInitialReferences();
            BayoSimulation();
        }

        // Update is called once per frame
        void Update()
        {
            SpatialStimulusReaction();
            
        }

        void SetInitialReferences()
        {
            //add agents to list
            foreach(GameObject agent in GameObject.FindGameObjectsWithTag(agentTag))
            {
                agentIdentities.Add(agent);
            }

            //add goals to list
            foreach(GameObject goal in GameObject.FindGameObjectsWithTag(goalTag))
            {
                goalLocations.Add(goal);
            }
        }

        public void BayoSimulation()
        {
            if(goalLocations.Count > 0)
            {
                if(agentIdentities.Count > 0)
                {
                    foreach(GameObject agent in agentIdentities)
                    {
                        //set destination
                        if(agent.GetComponent<NavMeshAgent>() != null)
                        {
                            GameObject goal = goalLocations[Random.Range(0, goalLocations.Count)];
                            agent.GetComponent<NavMeshAgent>().SetDestination(goal.transform.position);

                            //set walking animation
                            if(agent.GetComponent<Agent_Identity>() != null)
                            {
                                agent.GetComponent<Agent_Identity>().KeepWalking();
                            }
                            
                        }
                    }
                }
            }
        }

        public void SpatialStimulusReaction()
        {
            if (goalLocations.Count > 0)
            {
                if (agentIdentities.Count > 0)
                {
                    foreach (GameObject agent in agentIdentities)
                    {
                        //check remaining distance
                        if (agent.GetComponent<NavMeshAgent>() != null)
                        {
                            if(agent.GetComponent<NavMeshAgent>().remainingDistance < agent.GetComponent<Agent_Identity>().spatialData.highPriorityDist)
                            {
                                if (agent.GetComponent<Agent_Identity>().CheckSpatialStimulus() == true)
                                {
                                    agent.GetComponent<NavMeshAgent>().SetDestination(goalLocations[Random.Range(0, goalLocations.Count)].transform.position);

                                    //set walking animation
                                    if (agent.GetComponent<Agent_Identity>() != null)
                                    {
                                        agent.GetComponent<Agent_Identity>().KeepWalking();
                                    }
                                }
                                
                            }
                            
                        }
                    }
                }
            }
        }

        public void AuditoryStimulusReaction()
        {
            if(goalLocations.Count > 0)
            {
                if(agentIdentities.Count > 0)
                {
                    foreach(GameObject agent in agentIdentities)
                    {
                        //check remaining distance
                        if(agent.GetComponent<NavMeshAgent>() != null)
                        {
                            if(agent.GetComponent<NavMeshAgent>().remainingDistance < agent.GetComponent<Agent_Identity>().auditoryData.highPriorityDist)
                            {
                                if(agent.GetComponent<Agent_Identity>().CheckAuditoryStimulus() == true)
                                {
                                    //auditory reactions
                                }
                            }
                        }
                    }
                }
            }
        }

        public void VisualStimulusReaction()
        {
            if (goalLocations.Count > 0)
            {
                if (agentIdentities.Count > 0)
                {
                    foreach (GameObject agent in agentIdentities)
                    {
                        //check remaining distance
                        if (agent.GetComponent<NavMeshAgent>() != null)
                        {
                            if (agent.GetComponent<NavMeshAgent>().remainingDistance < agent.GetComponent<Agent_Identity>().visualData.highPriorityDist)
                            {
                                if (agent.GetComponent<Agent_Identity>().CheckVisualStimulus() == true)
                                {
                                    //visual reactions
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetDefaultBehavior()
        {

        }
    }
}
