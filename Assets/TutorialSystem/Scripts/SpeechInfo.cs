using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace TutorialSystem.Scripts
{
    [Serializable]
    public class SpeechInfo
    {
        [TextArea(1, 5)]
        public string speechText;
        public SpeechAudioInfo speechAudioClip;
        public SpeechSpriteAsset speechSpriteAsset;
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
        public TMP_SpriteAsset spriteAsset;
    }
}
