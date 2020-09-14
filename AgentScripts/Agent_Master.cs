using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paro
{
    public class Agent_Master : MonoBehaviour
    {
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventAgentPatrolAnim;
        public event GeneralEventHandler EventAgentSpeakAnim;
        public event GeneralEventHandler EventAgentListenAnim;
        public event GeneralEventHandler EventAgentIdleAnim;
        public event GeneralEventHandler EventAgentReactionAnim;

        public delegate void AgentRelationsChangeEventHandler();
        public event AgentRelationsChangeEventHandler EventRelationsChange;

        //animations variables
        public string animationBoolPatroling = "isPatroling";
        public string animationTriggerSpeak = "Speak";
        public string animationTriggerListening = "Listen";
        public string animationTriggerReaction = "React";

       public void CallEventPatrol()
        {
            if(EventAgentPatrolAnim != null)
            {
                EventAgentPatrolAnim();
            }
        }

        public void CallEventSpeak()
        {
            if (EventAgentSpeakAnim != null)
            {
                EventAgentSpeakAnim();
            }
        }

        public void CallEventListen()
        {
            if (EventAgentListenAnim != null)
            {
                EventAgentListenAnim();
            }
        }

        public void CallEventReact()
        {
            if (EventAgentReactionAnim != null)
            {
                EventAgentReactionAnim();
            }
        }

        public void CallEventIdle()
        {
            if (EventAgentIdleAnim != null)
            {
                EventAgentIdleAnim();
            }
        }
    }
}
