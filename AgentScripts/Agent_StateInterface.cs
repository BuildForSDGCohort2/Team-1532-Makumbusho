using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paro
{
    public interface AgentState_Interface
    {
        void UpdateState();
        void ToPatrolState();
        void ToReactionState();
        void ToSpeakingState();
        void ToListeningState();
        void ToAlertState();
    }
}
