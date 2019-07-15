using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;
using System.IO;
using com.csutil;
using DG.DeAudio;
using DG.Tweening;
using UniRx;

public class SoundManager : OverridableMonoBehaviour {

	public AudioClip backgroundMusic;
    public AudioClip endGameTheme;
    public DeAudioSource musicBack;
	public AudioClip AmbientSFX;
	public bool hasAmbientFX = false;
	public DeAudioSource AmbientSFXComp;
	public List<AudioSource> fxsList = new List<AudioSource>();
    public List<AudioSource> voiceFxs = new List<AudioSource>();
	public int amountOfSoundFxs;
	Dictionary<AudioClip, AudioSource> openWith = new Dictionary<AudioClip, AudioSource>();
	public bool isStarted = false;
	private bool hasReferences = false;
    private bool _pause = false;
    public bool hasBackaudio = true;
    public float OnVoiceEffectVolumeBackground = 0.4f;
    public float OnVoiceEffectVolumeAmbient = 0.4f;
    public bool isVoicePlaying;
    public float hasAmbientFXVolume = 0.4f;
    public float backgroundMusicVolume = 0.8f;
    public PauseManager PauseManager2;
    public AudioClip audio1;

    void SomBT2() {
        if (PauseManager2 == null || audio1 == null) {
            PauseManager2 = FindObjectOfType<PauseManager>();
            if (audio1 && PauseManager2 != null) {
                audio1 = PauseManager2.audio1;
            }
        }
      
        //startVoiceFX(audio1);
    }

    IEnumerator<float> delayStart()
    {   
        yield return Timing.WaitForSeconds(1.5f);

        if (hasAmbientFX && GameConfig.Instance.isAudioFXOn)
        {
	        DeAudioManager.Stop(DeAudioGroupId.Ambient);
	        AmbientSFXComp = DeAudioManager.Play(DeAudioGroupId.Ambient, AmbientSFX, !isVoicePlaying ? hasAmbientFXVolume : OnVoiceEffectVolumeAmbient, loop:true);
//                AmbientSFXComp.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
//                AmbientSFXComp.clip = AmbientSFX;
//                AmbientSFXComp.loop = true;
//                AmbientSFXComp.volume = !isVoicePlaying ? hasAmbientFXVolume : OnVoiceEffectVolumeAmbient;
//
//                AmbientSFXComp.Play();
        } 

        if(!hasAmbientFX && AmbientSFXComp != null && AmbientSFXComp.isPlaying) {
//            AmbientSFXComp.Stop();
            DeAudioManager.Stop(DeAudioGroupId.Ambient);
        }

        if (hasBackaudio && backgroundMusic != null && GameConfig.Instance.isAudioOn)
        {
	        DeAudioManager.Stop(DeAudioGroupId.Music);
	        musicBack = DeAudioManager.Play(DeAudioGroupId.Music, backgroundMusic, !isVoicePlaying ? backgroundMusicVolume : OnVoiceEffectVolumeBackground, loop:true);
//            musicBack.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
//            musicBack.clip = backgroundMusic;
//            musicBack.loop = true;
//            musicBack.volume = !isVoicePlaying ? backgroundMusicVolume : OnVoiceEffectVolumeBackground;
//            musicBack.Play();
        }
        else
        {
	        DeAudioManager.Stop(DeAudioGroupId.Music);
        }

        isStarted = true;
    }

	// Use this for initialization
	public void StartLate () {
        Timing.RunCoroutine(delayStart());

	}


	public void Start(){
        StartLate();       
    }

	public DeAudioSource returnRightSource(AudioClip _clip)
	{

		return DeAudioManager.Play(DeAudioGroupId.FX, _clip);

//		if (openWith.ContainsKey(_clip)) {
//			AudioSource d = openWith[_clip];
//			return d;
//		} else {
//			GameObject effectSound = new GameObject ("EffectSound");
//			effectSound.transform.SetParent (this.transform.GetChild (0));
//			effectSound.AddComponent<AudioSource> ();
//			AudioSource d = effectSound.GetComponent<AudioSource>();
//
//			d.playOnAwake = false;
//			d.clip = _clip;
//			openWith.Add(_clip, d);
//			return d;
//		}

	}

    public AudioSource VoiceEffectSource(AudioClip _clip) {

        if (_clip != null) {
            if (openWith.ContainsKey(_clip)) {
                AudioSource d = openWith[_clip];
                return d;
            } else {
                GameObject voiceEffect = new GameObject("VoiceEffect");
                voiceEffect.transform.SetParent(this.transform.GetChild(0));
                voiceEffect.AddComponent<AudioSource>();
                AudioSource d = voiceEffect.GetComponent<AudioSource>();
                d.playOnAwake = false;
                d.clip = _clip;
                openWith.Add(_clip, d);
                return d;
            }
        }

        GameObject voiceEffect2 = new GameObject("VoiceEffect");
        voiceEffect2.transform.SetParent(this.transform.GetChild(0));
        voiceEffect2.AddComponent<AudioSource>();
        AudioSource d2 = voiceEffect2.GetComponent<AudioSource>();
        return d2;

    }

    public AudioSource VoiceEffectSource(AudioClip _clip, float _volume) {

        if (openWith.ContainsKey(_clip)) {
            AudioSource d = openWith[_clip];
            d.volume = _volume;
            return d;
        } else {
            GameObject voiceEffect = new GameObject("VoiceEffect");
            voiceEffect.transform.SetParent(this.transform.GetChild(0));
            voiceEffect.AddComponent<AudioSource>();
            AudioSource d = voiceEffect.GetComponent<AudioSource>();
            d.playOnAwake = false;
            d.clip = _clip;
            d.volume = _volume;
            d.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            openWith.Add(_clip, d);
            return d;
        }

    }

    public DeAudioSource returnRightSource(AudioClip _clip, float _volume)
    {
	    return DeAudioManager.Play(_clip, _volume);

//		if (openWith.ContainsKey(_clip)) {
//			AudioSource d = openWith[_clip];
//			d.volume = _volume;
//			return d;
//		} else {
//			GameObject effectSound = new GameObject ("EffectSound");
//			effectSound.transform.SetParent (this.transform.GetChild (0));
//			effectSound.AddComponent<AudioSource> ();
//			AudioSource d = effectSound.GetComponent<AudioSource>();
//			d.playOnAwake = false;
//			d.clip = _clip;
//			d.volume = _volume;
//			d.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
//			openWith.Add(_clip, d);
//			return d;
//		}

    }



	public void stopCurrentFxs(){
		DeAudioManager.Stop(DeAudioGroupId.FX);
//		foreach (var item in fxsList) {
//			item.Stop ();
//		}
	}

	public void playCurrentsFxs(){
		DeAudioManager.Resume(DeAudioGroupId.Dialogue);
//		foreach (var item in fxsList) {
//			item.Play ();
//		}
	}

	public void changeBackgroundMusic(AudioClip song, bool isLoop){
		if (!GameConfig.Instance.isAudioOn) return;
		musicBack.Stop ();
		musicBack.loop = isLoop;
//		fixSound();
	}


	public void changeBackgroundMusic(AudioClip song){
		if (!GameConfig.Instance.isAudioOn) return;
		musicBack.Stop ();
		musicBack.loop = true;
//		fixSound();
	}

    public void VoiceOverHandler() {
	    DeAudioManager.Stop(DeAudioGroupId.Dialogue);
    }

    public void StopVoiceEffects() {
        DeAudioManager.Pause(DeAudioGroupId.Dialogue);
    }

    public void PlayVoiceEffects() {
	    DeAudioManager.Resume(DeAudioGroupId.Dialogue);
    }


    public void startSoundFX(AudioClip effectSong){
	    if (!GameConfig.Instance.isAudioFXOn) return;
	    //AudioSource voic =
		DeAudioManager.Play(effectSong);
		StopVoiceEffects();
		//voiceFxs.Add(voic);
		//oic.PlayOneShot(effectSong);
		//StartCoroutine (playAndDelete (effectSound));
//		fixSound();
	}

    public void startSoundFXDelay(AudioClip effectSong){
	    if (!GameConfig.Instance.isAudioFXOn) return;
	    //AudioSource voic =
	    this.ExecuteDelayed(() => { DeAudioManager.Play(DeAudioGroupId.FX, effectSong); }, .3f);

//		    var source = returnRightSource(effectSong);
//
//		    source.clip = effectSong;
//		    source.PlayDelayed(.3f);
	    StopVoiceEffects();
	    //voiceFxs.Add(voic);
	    //oic.PlayOneShot(effectSong);
	    //StartCoroutine (playAndDelete (effectSound));
//	    fixSound();
    }

    public void startSoundFXDelay(AudioClip effectSong, float delay){
	    if (!GameConfig.Instance.isAudioFXOn) return;
	    this.ExecuteDelayed(() => { DeAudioManager.Play(DeAudioGroupId.FX, effectSong); }, delay);
	    //AudioSource voic =
//	    var source = returnRightSource(effectSong);
//	    source.clip = effectSong;
//	    source.PlayDelayed(delay);
//	    StopVoiceEffects();
	    //voiceFxs.Add(voic);
	    //oic.PlayOneShot(effectSong);
	    //StartCoroutine (playAndDelete (effectSound));
//	    fixSound();
    }

	public DeAudioSource startSoundFXWithReturn(AudioClip effectSong)
	{
		return GameConfig.Instance.isAudioFXOn ? DeAudioManager.Play(DeAudioGroupId.FX, effectSong) : null;
	}

	public void startSoundFX(AudioClip effectSong, float volume){
		if (GameConfig.Instance.isAudioFXOn)
		{
			DeAudioManager.Play(DeAudioGroupId.FX, effectSong, volume);
		}
	}

	public void startSoundFX(AudioClip effectSong, float volume, float delay){
		Timing.RunCoroutine (startSoundFXdelayed (effectSong, volume, delay));
//		fixSound();
	}

	IEnumerator<float> startSoundFXdelayed(AudioClip effectSong, float volume, float delay){
		yield return Timing.WaitForSeconds (delay);
		if (GameConfig.Instance.isAudioFXOn) {
			DeAudioManager.Play(DeAudioGroupId.FX, effectSong, volume);
		}
	}

	public void startVoiceFX(AudioClip VoiceSound){
        SomBT2();
        if (VoiceSound != null && GameConfig.Instance.isAudioVoiceOn)
        {
	        DeAudioManager.Play(DeAudioGroupId.Dialogue, VoiceSound);
        }
    }

    IEnumerator<float> BackVolumeOverTime(float time) {
        if (hasBackaudio && musicBack != null) {
//            musicBack.volume = OnVoiceEffectVolumeBackground;
            DeAudioManager.FadeSourcesTo(DeAudioGroupId.Music, OnVoiceEffectVolumeBackground);
        }
        if (hasAmbientFX) {
//            AmbientSFXComp.volume = OnVoiceEffectVolumeAmbient;
            DeAudioManager.FadeSourcesTo(DeAudioGroupId.Ambient, OnVoiceEffectVolumeAmbient);
        }
        yield return Timing.WaitForSeconds(time);
        if (hasBackaudio) {
//            musicBack.volume = backgroundMusicVolume;/
            DeAudioManager.FadeSourcesTo(DeAudioGroupId.Music, backgroundMusicVolume);
        }
        if (hasAmbientFX) {
//            AmbientSFXComp.volume = hasAmbientFXVolume;
            DeAudioManager.FadeSourcesTo(DeAudioGroupId.Ambient, hasAmbientFXVolume);
        }
        isVoicePlaying = false;
    }

    public DeAudioSource startVoiceFXReturn(AudioClip VoiceSound)
    {
	    return GameConfig.Instance.isAudioVoiceOn ? DeAudioManager.Play(DeAudioGroupId.Dialogue, VoiceSound) : null;
    }

    public void startVoiceFX(AudioClip VoiceSound, float volume){
	    if (!GameConfig.Instance.isAudioVoiceOn) return;
	    DeAudioManager.Stop(DeAudioGroupId.Dialogue);
	    var audio = DeAudioManager.Play(DeAudioGroupId.Dialogue, VoiceSound, volume);
//	    VoiceEffectSource(VoiceSound, volume).PlayOneShot(VoiceSound);
//		VoiceOverHandler();
		Timing.KillCoroutines("soundVoice");
		Timing.RunCoroutine(BackVolumeOverTime(VoiceSound.length), "soundVoice");
    }

	public void startVoiceFX(AudioClip VoiceSound, float volume, float delay){
		
		Timing.RunCoroutine (startVoiceFXDekat (VoiceSound, volume, delay));
	}


	IEnumerator<float> startVoiceFXDekat(AudioClip VoiceSound, float volume, float delay){
		yield return Timing.WaitForSeconds (delay);
		if (GameConfig.Instance.isAudioVoiceOn) {
            VoiceEffectSource(VoiceSound, volume).PlayOneShot(VoiceSound);
		}
	}

	IEnumerator<float> playAndDelete(GameObject soundFX){
		soundFX.GetComponent<AudioSource> ().Play();
		yield return Timing.WaitForSeconds (soundFX.GetComponent<AudioSource> ().clip.length);

		if (fxsList.Contains (soundFX.GetComponent<AudioSource> ())) {
			fxsList.Remove (soundFX.GetComponent<AudioSource> ());
		}

		Destroy (soundFX);
//		fixSound();
	}

	public void OnPause(){
        _pause = true;
        DeAudioManager.Stop(DeAudioGroupId.Dialogue);
        DeAudioManager.Stop(DeAudioGroupId.FX);
//        stopCurrentFxs();
//        StopVoiceEffects();
//        if (musicBack == null) {
//            musicBack = GetComponent<AudioSource>();
//        }
		DeAudioManager.Stop(DeAudioGroupId.Music);
		DeAudioManager.Pause(DeAudioGroupId.Ambient);

//        musicBack.Pause();
//		AmbientSFXComp.Pause();
        

	}

	public void OnUnPause(){
        _pause = false;

//        fixSound();


		if(GameConfig.Instance.isAudioOn){
			DeAudioManager.Stop(DeAudioGroupId.Music);
			musicBack = DeAudioManager.Play(DeAudioGroupId.Music, backgroundMusic, !isVoicePlaying ? backgroundMusicVolume : OnVoiceEffectVolumeBackground);
		}
		else
		{
			DeAudioManager.Stop(DeAudioGroupId.Music);
		}

		if(GameConfig.Instance.isAudioFXOn && hasAmbientFX){
			DeAudioManager.Stop(DeAudioGroupId.Ambient);
			AmbientSFXComp = DeAudioManager.Play(DeAudioGroupId.Ambient, AmbientSFX, !isVoicePlaying ? hasAmbientFXVolume : OnVoiceEffectVolumeAmbient);
        }
		else
		{
			DeAudioManager.Stop(DeAudioGroupId.Ambient);
		}

//        if (!hasAmbientFX && AmbientSFXComp != null && AmbientSFXComp.isPlaying) {
//            AmbientSFXComp.Stop();
//        }
//
//        if (gameConfig.isAudioVoiceOn) {
//            //PlayVoiceEffects();
//        }

        isStarted = true;

    }

    public void PlayEndGameTheme() {
    }

	public void EndAllCoroutines(){
        Timing.KillCoroutines();
        StopAllCoroutines();
	}

    

}
