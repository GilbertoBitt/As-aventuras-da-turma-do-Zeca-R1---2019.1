using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSomTutor : MonoBehaviour {

    public SoundManager soundManager;
    public AudioClip[] audiosTutorial;
    public int numTutor;
    public bool checkSom;
    public AudioSource audio;
    bool checkPassou;

    public void SomTutor() {

        if (checkSom && !checkPassou) {
          
            checkPassou = true;          
            
            audio = soundManager.startVoiceFXReturn(audiosTutorial[numTutor]);
            if (audio != null) {
                audio.ignoreListenerPause = false;
            }
            //audio.Play();
        } 
        else if(checkSom && checkPassou) {
            checkPassou = false;
        }
        else if (!checkSom) {
           
            audio = soundManager.startVoiceFXReturn(audiosTutorial[numTutor]);
          if(audio!=null)
            audio.ignoreListenerPause = true;
           // audio.Play();
        }


    }
    public void RepetirAudio() {

        soundManager.startVoiceFXReturn(audiosTutorial[numTutor]);


    }
}
