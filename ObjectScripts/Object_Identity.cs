using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paro
{
    public class Object_Identity : MonoBehaviour
    {
        public string objectName = "object";
        [HideInInspector]
        public enum ObjectGoal
        {
            Ignore,
            Spatial,
            Auditory,
            Visual
        }
        public ObjectGoal goal = ObjectGoal.Ignore;
        public Animator myAnimator;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetInitialReferences()
        {

        }
    }
}
