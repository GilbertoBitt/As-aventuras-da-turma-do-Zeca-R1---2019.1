using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

	public bool isOnPause;
	public Toggle[] Toggles;
	public GameConfig config;
	public SoundManager sound;
	public LoadManager load;
	public GameObject pausePanel;

	public GameObject panelLoad;

	public UnityEvent OnPauseMenuOpen;
	public UnityEvent OnPauseMenuClose;
    public SoundManager soundManager;
    public AudioClip audio1;

    [Header("Clear Cache Settings")]
    public Button buttonConfirm;
    public Button buttonDecline;
    public GameObject panelCacheCleaner;
    public TextMeshProUGUI warningTextComponent;

    public void SomBT() {
        if (soundManager == null) {
            soundManager = FindObjectOfType<SoundManager>();
        }
        soundManager.startSoundFX(audio1);
    }

    public void PauseGame(){
        SomBT();
        Pause();
        OpenConfig ();
        soundManager.OnPause();
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
	    Time.timeScale = 0f;
    }
    
    public void OpenConfigPanel()
    {
	    OpenConfig();
	    isOnPause = true;
	    OnPauseMenuOpen.Invoke();
	    pausePanel.SetActive (true);
    }
    public void CloseConfigPanel()
    {
	    OpenConfig();
	    isOnPause = false;
	    SomBT();
	    soundManager.OnUnPause();
	    OnPauseMenuClose.Invoke();
	    pausePanel.SetActive (false);
    }

    public void UnpauseGameWithSound(){
	    UnpausWithSound();;
    }

    public void UnpauseGame(){
        SomBT();
		Unpause();
		pausePanel.SetActive (false);
		GameConfig.Instance.SavePrefs ();
	}

    public void UnpausWithSound()
    {
	    isOnPause = false;
	    Time.timeScale = 1f;
    }

    public void Unpause()
    {
	    isOnPause = false;
	    Time.timeScale = 1f;
	    soundManager.OnUnPause();
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

	public void ExitGame()
	{
		Application.Quit();
	}

	public void ClearCache()
	{
		GameConfig.Instance.OpenDb().CloseConnection();
	}

	public void ShowCacheConfirmWindow()
	{
		
		var ds = GameConfig.Instance.OpenDb();
		
		//Get all offline Logs.
		var _logToSend = ds.GetAllMinigamesLog().FindAll(x => x.online == 0);
		var _rankToSend = ds.GetAllOfflineRanks();
		var _statisticaToSend = ds.GetAllStatisticDidatica().FindAll(x => x.online == 0);
		var _logsGames = ds.GetAllGamesLOG().FindAll(x => x.online == 0);
		var userScoreUpdates = ds.GetallScoresOffline();
		var userInventoryUdpdate = ds.GetAllInventory();

		var informationToSync = _logsGames.Count + _rankToSend.Count + _statisticaToSend.Count + _logsGames.Count +
		                        userScoreUpdates.Count + userInventoryUdpdate.Count;
		
		warningTextComponent.text = $"Existem {informationToSync} informações salvas offline esperando para serem sincronizadas. Aguarde um momento e não desconecte-se da internet.";
		panelCacheCleaner.SetActive(true);
		buttonConfirm.OnClickAsObservable().Subscribe(_ =>
		{
			buttonConfirm.interactable = false;
			buttonDecline.interactable = false;
			EduqbrinqLogger.Instance.SendOfflineData(_logToSend, _rankToSend, _statisticaToSend, _logsGames, userScoreUpdates, userInventoryUdpdate).GetAwaiter().OnCompleted(
				() =>
				{
					PlayerPrefs.SetString("PlayerLastLogin", "");
					PlayerPrefs.DeleteKey("PlayerProfile");
					PlayerPrefs.DeleteAll();
					SceneManager.LoadScene ("LowSceneLoad");
				});
			
		});

		buttonDecline.OnClickAsObservable().TakeUntilDisable(buttonDecline).Subscribe(_ =>
		{
			GameConfig.Instance.OpenDb().CloseConnection();
			panelCacheCleaner.SetActive(false);
		});
	}
	
}
