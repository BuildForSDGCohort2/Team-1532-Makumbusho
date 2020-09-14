using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paro
{
    public class Agent_HeadLook : MonoBehaviour
    {
        private Animator myAnimator;
        public Transform lookAtPosition;

        // Start is called before the first frame update
        void Start()
        {
            SetInitialReferences();   
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetInitialReferences()
        {
            myAnimator = GetComponent<Animator>();
        }

        private void OnAnimatorIK()
        {
            if(myAnimator.enabled)
            {
                if(lookAtPosition != null)
                {
                    myAnimator.SetLookAtWeight(1, 0, 0.5f, 0.5f, 0.7f);
                    myAnimator.SetLookAtPosition(lookAtPosition.position);
                }
                else
                {
                    myAnimator.SetLookAtWeight(0);
                }
            }
        }
    }
}
