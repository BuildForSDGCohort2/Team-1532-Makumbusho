using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paro
{
    public class Agent_Animations : MonoBehaviour
    {
        private Agent_Master agentMaster;
        private Animator myAnimator;

        private void OnEnable()
        {
            SetInitialReferences();
                

        }

        private void OnDisable()
        {
            
        }

        private void SetInitialReferences()
        {
            if(GetComponent<Agent_Master>() != null)
            {
                agentMaster = GetComponent<Agent_Master>();
            }

            if(GetComponent<Agent_Master>() != null)
            {
                myAnimator = GetComponent<Animator>();
            }
        }

        void ActivateWalkingAnimation()
        {
            if(myAnimator != null)
            {
                if(myAnimator.enabled)
                {
                    myAnimator.SetBool(agentMaster.animationBoolPatroling, true);
                }
            }
        }

        void ActivateIdleAnimation()
        {
            if(myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetBool(agentMaster.animationBoolPatroling, false);
                }
            }
        }

        void ActivateSpeakingAnimation()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetTrigger(agentMaster.animationTriggerSpeak);
                }
            }
        }

        void ActivateListeningAnimation()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetTrigger(agentMaster.animationTriggerListening);
                }
            }
        }


    }
    
}
