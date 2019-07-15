using System.Collections;
using System.Collections.Generic;
using DG.DeAudio;
using UnityEngine;

namespace MiniGames.Scripts
{
    public class ControlSomTutor : MonoBehaviour
    {

        public SoundManager soundManager;
        public AudioClip[] audiosTutorial;
        public int numTutor;
        public bool checkSom;
        public DeAudioSource audio;
        bool checkPassou;

        public void SomTutor()
        {

            if (checkSom && !checkPassou)
            {

                checkPassou = true;

                audio = soundManager.startVoiceFXReturn(audiosTutorial[numTutor]);

                //audio.Play();
            }
            else if (checkSom && checkPassou)
            {
                checkPassou = false;
            }
            else if (!checkSom)
            {
                if (soundManager.startVoiceFXReturn(audiosTutorial[numTutor]) != null)
                {
                    audio = soundManager.startVoiceFXReturn(audiosTutorial[numTutor]);
                }


                // audio.Play();
            }


        }

        public void RepetirAudio()
        {

            soundManager.startVoiceFXReturn(audiosTutorial[numTutor]);


        }
    }
}
