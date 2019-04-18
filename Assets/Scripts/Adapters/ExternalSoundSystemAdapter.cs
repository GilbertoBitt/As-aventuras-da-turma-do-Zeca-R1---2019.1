using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalSoundSystemAdapter : OverridableMonoBehaviour {

    public enum SoundClipType {
        Background,
        SoundFXs,
        VoiceSound,
    }

    public SoundClipType ClipType;

    public bool ContinuousPlay = false;

    public SoundManager SoundInstance { get {
            if (_instanceSoundManager == null) {
                _instanceSoundManager = FindObjectOfType<SoundManager>();
            }
            return _instanceSoundManager;
        }
        set {
            _instanceSoundManager = value;
        }
    }

    private SoundManager _instanceSoundManager;

    public AudioSource SoundSource {
        get {
            if(_audioSource == null) {
                _audioSource = GetComponent<AudioSource>();
            }
            return _audioSource;
        }
    }

    private AudioSource _audioSource;

    private bool latestAudioState = false;

    private void Start() {
        latestAudioState = IsAudioTypeOn();
    }

    private void FixedUpdate() {
        if (latestAudioState == IsAudioTypeOn()) return;
        latestAudioState = IsAudioTypeOn();
        var audioIsOn = latestAudioState;
        if (SoundSource.isPlaying && !audioIsOn) {
            if (ContinuousPlay) {
                SoundSource.Pause();
            } else {
                SoundSource.Stop();
            }
        } else if(!SoundSource.isPlaying && audioIsOn) {
            SoundSource.Play();
        } else {
            return;
        }
    }

    private bool IsAudioTypeOn() {
        switch (ClipType) {
            case SoundClipType.Background:
                return SoundInstance.gameConfig.isAudioOn;
            case SoundClipType.SoundFXs:
                return SoundInstance.gameConfig.isAudioFXOn;
            case SoundClipType.VoiceSound:
                return SoundInstance.gameConfig.isAudioVoiceOn;
        }
        return false;
    }

}
