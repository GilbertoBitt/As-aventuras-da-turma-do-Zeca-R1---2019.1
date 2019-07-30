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

    public void PauseWithSound()
    {
	    isOnPause = true;
//	    OnPauseMenuOpen.Invoke();
	    Time.timeScale = 0f;
    }

    public void UnpauseGameWithSound(){
//	    SomBT();
//	    btPause.SetActive (true);
	    UnpausWithSound();
//	    pausePanel.SetActive (false);
//	    GameConfig.Instance.SavePrefs ();
    }

    public void UnpauseGame(){
        SomBT();
        btPause.SetActive (true);
		Unpause();
		pausePanel.SetActive (false);
		GameConfig.Instance.SavePrefs ();
	}

    public void UnpausWithSound()
    {
	    isOnPause = false;
	    Time.timeScale = 1f;
//	    OnPauseMenuClose.Invoke();
    }

    public void Unpause()
    {
	    isOnPause = false;
	    Time.timeScale = 1f;
	    OnPauseMenuClose.Invoke();
    }

    public void OpenConfig(){

        SomBT();
        Toggles [0].isOn = !GameConfig.Instance.isAudioOn;
		Toggles [1].isOn = !GameConfig.Instance.isAudioFXOn;
		Toggles [2].isOn = !GameConfig.Instance.isAudioVoiceOn;

	}

	public void backgroundMusicValidate()
	{
		SomBT();
		GameConfig.Instance.isAudioOn = !Toggles [0].isOn;
	}

	public void soundFXValidate()
	{
		SomBT();
		GameConfig.Instance.isAudioFXOn = !Toggles [1].isOn;
	}

	public void VoicesValidate()
	{
		SomBT();
		GameConfig.Instance.isAudioVoiceOn = !Toggles [2].isOn;
	}

	public void backToMenu(){
        SomBT();
        panelLoad.SetActive(true);
		Time.timeScale = 1;
		load.LoadAsync ("selectionMinigames");
	}
}
