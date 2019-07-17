using System;
using com.csutil;
using Sirenix.OdinInspector;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Video;

namespace TutorialSystem.Scripts
{
    [Serializable]
    public class SpeechInfo
    {
        [TextArea(1, 5)]
        public string speechText;
        public SpeechAudioInfo speechAudioClip;
        public SpeechSpriteAsset speechSpriteAsset;
        public SpeechVideoPlayer speechVideoAsset;
        public SpeechEventBus speechEventSystem;
    }

    [Serializable, Toggle("enabled")]
    public class SpeechAudioInfo
    {
        public bool enabled;
        public AudioClip audioClip;
    }

    [Serializable, Toggle("enabled")]
    public class SpeechSpriteAsset
    {
        public bool enabled;
        public Sprite spriteAsset;
    }

    [Serializable, Toggle("enabled")]
    public class SpeechVideoPlayer
    {
        public bool enabled;
        public VideoClip videoAsset;
    }

    [Serializable, Toggle("enabled")]
    public class SpeechEventBus
    {
        public bool enabled;
        public VoidEvent startEvent;
        public VoidEvent stopEvent;
        public SpeechEventBusDelay eventDelay;

    }

    [Serializable, Toggle("enabled")]
    public class SpeechEventBusDelay
    {
        public bool enabled;
        public float eventDelay;

    }
}
