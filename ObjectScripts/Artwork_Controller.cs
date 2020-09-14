using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Audio;

namespace Paro
{
    public class Artwork_Controller : MonoBehaviour
    {
        public string title = "UNTITLED";
        public InputField titleField;

        public string year = "UNDATED";
        public InputField yearField;

        public AudioSource audioSource;
        public AudioClip audioClip;
        public GameObject labelCanvas;
        public bool labelIsVisible;

        [System.Serializable]
        public class Artist
        {
            public string name = "UNKNOWN";
            public InputField nameField;

            public RawImage avatarField;
            public Texture defaultAvatar;
            public Texture artistAvatar;
        }

        public Artist artist;

        // Start is called before the first frame update
        void Start()
        {
            SetInitialReferences();
            SetupLabels();
        }

        // Update is called once per frame
        void Update()
        {
            SetInitialReferences();
            PlayAudio();
            PauseAudio();
        }

        void SetInitialReferences()
        {
            if(labelCanvas != null)
            {
                if(labelCanvas.activeSelf == true)
                {
                    labelIsVisible = true;
                }
                else
                {
                    labelIsVisible = false;
                }
            }
        }

        public void SetupLabels()
        {
            //title
            if(titleField != null)
            {
                titleField.text = title;
            }

            //date
            if(yearField != null)
            {
                yearField.text = year;
            }

            //artist name
            if(artist.nameField != null)
            {
                artist.nameField.text = artist.name;
            }

            //avatar
            if(artist.avatarField != null)
            {
                if(artist.artistAvatar != null)
                {
                    artist.avatarField.texture = artist.artistAvatar;
                }
                else if(artist.defaultAvatar != null && artist.artistAvatar == null)
                {
                    artist.avatarField.texture = artist.defaultAvatar;
                }
            }
        }

        public void PlayAudio()
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                if(labelIsVisible == true)
                {
                    if (audioSource != null)
                    {
                        if (audioClip != null)
                        {
                            audioSource.clip = audioClip;
                            audioSource.Play();
                        }
                    }
                }
            }
        }

        public void PauseAudio()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if(audioSource != null)
                {
                    audioSource.Pause();
                }
            }
        }
    }
}
