using System;
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

    private void OnEnable()
    {
        if (SoundSource.enabled == GameConfig.Instance.isAudioFXOn) return;
        SoundSource.enabled = GameConfig.Instance.isAudioFXOn;
    }

    public void Update()
    {
        if (SoundSource.enabled == GameConfig.Instance.isAudioFXOn) return;
        SoundSource.enabled = GameConfig.Instance.isAudioFXOn;
    }


    private bool IsAudioTypeOn() {
        switch (ClipType) {
            case SoundClipType.Background:
                return GameConfig.Instance.isAudioOn;
            case SoundClipType.SoundFXs:
                return GameConfig.Instance.isAudioFXOn;
            case SoundClipType.VoiceSound:
                return GameConfig.Instance.isAudioVoiceOn;
        }
        return false;
    }

}
