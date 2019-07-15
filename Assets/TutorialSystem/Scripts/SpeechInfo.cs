using System;
using com.csutil;
using Sirenix.OdinInspector;
using TMPro;
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
        public string eventName;
        public SpeechEventBusDelay eventDelay;

        public void Subscribe(object targetSubscriber, Action callbackAction)
        {
            EventBus.instance.Subscribe(targetSubscriber, eventName, callbackAction);
        }

        public void Unsubscribe(object targetSubscriber, Action callbackAction)
        {
            EventBus.instance.Unsubscribe(targetSubscriber, eventName);
        }

        public void FireEvent()
        {
            EventBus.instance.Publish(eventName);
        }
    }

    [Serializable, Toggle("enabled")]
    public class SpeechEventBusDelay
    {
        public bool enabled;
        public float eventDelay;

    }
}
