using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paro
{
    public class MenuController : MonoBehaviour
    {
        private bool isPaused;
        private bool instructionsIsActive;
        public GameObject menuGO;
        public GameObject startGO;
        public GameObject instructionsGO;
        // Start is called before the first frame update
        void Start()
        {
            TogglePause();
        }

        // Update is called once per frame
        void Update()
        {
            CheckPauseInput();
        }

        public void CheckPauseInput()
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                TogglePause();
            }
        }

        public void ToggleStartMenu()
        {
            if(startGO != null)
            {
                startGO.SetActive(true);
                isPaused = true;
            }
        }

        public void TogglePause()
        {
            if (isPaused)
            {
                if (menuGO != null)
                {
                    menuGO.SetActive(false);
                }
                Time.timeScale = 1;
                isPaused = false;
            }
            else
            {
                if (menuGO != null)
                {
                    menuGO.SetActive(true);
                }
                Time.timeScale = 0;
                isPaused = true;
            }
        }

        public void ToggleInstructionPanel()
        {
            if(instructionsGO != null)
            {
                if(instructionsGO.activeSelf == false)
                {
                    instructionsGO.SetActive(true);
                }
                else
                {
                    instructionsGO.SetActive(false);
                }
            }
        }

        //quit game
        public void ExitSimulation()
        {
            Application.Quit();
        }
    }
}
