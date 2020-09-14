using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Paro
{
    public class Agent_Identity : MonoBehaviour
    {
        public string agentName = "Adam";
        public string gender = "male";
        public Transform head;
        public NavMeshAgent myNavmesh;
        public Animator myAnimator;

        //animations
        public string walkBool = "isWalking";
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

        //Used for sight
        public LayerMask sightLayers;
        public LayerMask myPartnerLayers;
        public LayerMask myAudienceLayers;
        public LayerMask myGoalLayers;
        public string[] myAudienceTags;
        public string[] myPartnerTags;
        public string[] myGoalTags;

        //references
        public Transform[] waypoints;


        public Transform myFollowTarget;
        [HideInInspector]
        public Transform pursueTarget;
        [HideInInspector]
        public Vector3 locationOfInterest;
        [HideInInspector]
        public Vector3 wanderTarget;
        [HideInInspector]
        public Transform myAudience;

        [HideInInspector]
        public enum myTribe
        {
            Luo,
            Swahili
        };

        [HideInInspector]
        public enum StimulusLevel
        {
            Zero,
            Low,
            Mid,
            High
        };
        public myTribe dhok = myTribe.Luo;

        [System.Serializable]
        public class SpatialStimulusData
        {
            public float lowPriorityDist = 10.0f;
            public float midPriorityDist = 5.0f;
            public float highPriorityDist = 1.0f;

            public List<AudioClip> lowPrioritySounds;
            public List<AudioClip> midPrioritySounds;
            public List<AudioClip> highPrioritySounds;

            public float cooldownTimer = 1.0f;

            public string lowPrioritySpatialReactionTrigger = "lowSpatialReactionTrigger";
            public string mediumPrioritySpatialReactionTrigger = "midSpatialReactionTrigger";
            public string highPrioritySpatialReactionTrigger = "highSpatialReactionTrigger";

            public StimulusLevel stimulusLevel = StimulusLevel.Zero;

            public bool isTriggered = false;
        }

        [System.Serializable]
        public class AuditoryStimulusData
        {
            public float lowPriorityDist = 10.0f;
            public float midPriorityDist = 5.0f;
            public float highPriorityDist = 1.0f;

            public List<AudioClip> lowPrioritySounds;
            public List<AudioClip> midPrioritySounds;
            public List<AudioClip> highPrioritySounds;

            public float cooldownTimer = 1.0f;

            public string lowPriorityAuditoryReactionTrigger = "lowSpatialReactionTrigger";
            public string mediumPriorityAuditoryReactionTrigger = "midSpatialReactionTrigger";
            public string highPriorityAuditoryReactionTrigger = "highSpatialReactionTrigger";

            public StimulusLevel stimulusLevel = StimulusLevel.Zero;

            public bool isTriggered = false;
        }

        [System.Serializable]
        public class VisualStimulusData
        {
            public float lowPriorityDist = 10.0f;
            public float midPriorityDist = 5.0f;
            public float highPriorityDist = 1.0f;

            public List<AudioClip> lowPrioritySounds;
            public List<AudioClip> midPrioritySounds;
            public List<AudioClip> highPrioritySounds;

            public float cooldownTimer = 1.0f;

            public string lowPriorityVisualReactionTrigger = "lowSpatialReactionTrigger";
            public string mediumPriorityVisualReactionTrigger = "midSpatialReactionTrigger";
            public string highPriorityVisualReactionTrigger = "highSpatialReactionTrigger";

            public StimulusLevel stimulusLevel = StimulusLevel.Zero;

            public bool isTriggered = false;
        }

        public SpatialStimulusData spatialData;
        public AuditoryStimulusData auditoryData;
        public VisualStimulusData visualData;

        private Collider[] partnerColliders;
        private Collider[] audienceColliders;
        private Collider[] objectColliders;
        private Transform myTransform;
        private Vector3 lookAtPoint;
        private Vector3 heading;
        private float dotProd;

        // Start is called before the first frame update
        void Start()
        {
            SetInitialReferences();
        }

        // Update is called once per frame
        void Update()
        {
            SetInitialReferences();
        }

        void SetInitialReferences()
        {
            if(GetComponent<Transform>() != null)
            {
                myTransform = GetComponent<Transform>();
            }

            partnerColliders = Physics.OverlapSphere(myTransform.position, spatialData.highPriorityDist, myPartnerLayers);
            audienceColliders = Physics.OverlapSphere(myTransform.position, spatialData.highPriorityDist, myAudienceLayers);
            objectColliders = Physics.OverlapSphere(myTransform.position, spatialData.highPriorityDist, myGoalLayers);
        }

        public bool CheckSpatialStimulus()
        {
            
            if(objectColliders.Length > 0)
            {
                if(objectColliders[0].GetComponent<Object_Identity>() != null)
                {
                    Object_Identity id = objectColliders[0].GetComponent<Object_Identity>();

                    if(id.goal == Object_Identity.ObjectGoal.Spatial)
                    {
                        return true;
                        
                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        public void CheckConversationStimulus()
        {

        }

        public bool CheckVisualStimulus()
        {
            if (objectColliders.Length > 0)
            {
                if (objectColliders[0].GetComponent<Object_Identity>() != null)
                {
                    Object_Identity id = objectColliders[0].GetComponent<Object_Identity>();

                    if (id.goal == Object_Identity.ObjectGoal.Visual)
                    {
                        return true;

                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        public bool CheckAuditoryStimulus()
        {
            if (objectColliders.Length > 0)
            {
                if (objectColliders[0].GetComponent<Object_Identity>() != null)
                {
                    Object_Identity id = objectColliders[0].GetComponent<Object_Identity>();

                    if (id.goal == Object_Identity.ObjectGoal.Auditory)
                    {
                        return true;

                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        public void Look()
        {
            //check audience medium range
            audienceColliders = Physics.OverlapSphere(myTransform.position, sightRange / 3, myAudienceLayers);

            if(audienceColliders.Length > 0)
            {
                VisibilityCalculations(audienceColliders[0].transform);

                if(dotProd > 0)
                {
                    //state actions
                }
            }
            objectColliders = Physics.OverlapSphere(myTransform.position, sightRange / 3, myGoalLayers);
        }

        void VisibilityCalculations(Transform target)
        {
            lookAtPoint = new Vector3(target.position.x, target.position.y + offset, target.position.z);
            heading = lookAtPoint - myTransform.position;
            dotProd = Vector3.Dot(heading, myTransform.forward);
        }

        public void SetOrientation()
        {

        }

        public void AnimateReaction()
        {

        }

        public void MoveTo()
        {

        }

        public void KeepWalking()
        {
            if(myAnimator != null)
            {
                myAnimator.SetBool(walkBool, true);
            }
        }

        public void StopWalking()
        {
            if (myAnimator != null)
            {
                myAnimator.SetBool(walkBool, false);
            }
        }

        public void PlayReactionSound()
        {

        }

        public void SetCoolDownTimer()
        {

        }
    }
}
