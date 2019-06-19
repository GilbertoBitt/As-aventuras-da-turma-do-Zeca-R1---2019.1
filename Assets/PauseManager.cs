using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour {

	public bool isOnPause;
	public Toggle[] Toggles;
	public GameConfig config;
	public SoundManager sound;
	public LoadManager load;
	public GameObject pausePanel;

	public GameObject panelLoad;
	public GameObject btPause;

	public UnityEvent OnPauseMenuOpen;
	public UnityEvent OnPauseMenuClose;
    public SoundManager soundManager;
    public AudioClip audio1;

    public void SomBT() {
        if (soundManager == null) {
            soundManager = FindObjectOfType<SoundManager>();
        }
        soundManager.startVoiceFX(audio1);
    }

    public void PauseGame(){
        SomBT();
        Pause();
        OpenConfig ();
		pausePanel.SetActive (true);
	}

    public void Pause()
    {
	    isOnPause = true;
	    OnPauseMenuOpen.Invoke();
	    Time.timeScale = 0f;
    }

    public void UnpauseGame(){
        SomBT();
        btPause.SetActive (true);
		Unpause();
		pausePanel.SetActive (false);
		config.SavePrefs ();
	}

    public void Unpause()
    {
	    isOnPause = false;
	    Time.timeScale = 1f;
	    OnPauseMenuClose.Invoke();
    }

    public void OpenConfig(){

        SomBT();
        Toggles [0].isOn = !config.isAudioOn;
		Toggles [1].isOn = !config.isAudioFXOn;
		Toggles [2].isOn = !config.isAudioVoiceOn;

	}

	public void backgroundMusicValidate()
	{
		SomBT();
		config.isAudioOn = !Toggles [0].isOn;
	}

	public void soundFXValidate()
	{
		SomBT();
		config.isAudioFXOn = !Toggles [1].isOn;
	}

	public void VoicesValidate()
	{
		SomBT();
		config.isAudioVoiceOn = !Toggles [2].isOn;
	}

	public void backToMenu(){
        SomBT();
        panelLoad.SetActive(true);
		Time.timeScale = 1;
		load.LoadAsync ("selectionMinigames");
	}
}
