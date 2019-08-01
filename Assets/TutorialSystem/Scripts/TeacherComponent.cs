using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace TutorialSystem.Scripts
{
    public class TeacherComponent : MonoBehaviour, ICharacterAnimation
    {
        public Color characterNameColor;
        public string characterName = String.Empty;
        public Image characterHeadImageComponent;

        public PlayableDirector directorComponent;

        public PlayableAsset defaultAnimationTimelineAsset;
        public PlayableAsset talkingAnimationTimelineAsset;

        public float talkAnimationSpeed;
        public float defaultAnimationSpeed;


        public string GetCharacterName() => characterName;
        public Color GetCharacterNameColor() => characterNameColor;

        private void Start()
        {
            directorComponent = GetComponent(typeof(PlayableDirector)) as PlayableDirector;
        }

        public void StartDefaultAnimation()
        {
            directorComponent.Play(defaultAnimationTimelineAsset);
        }

        public void StopDefaultAnimation()
        {
            directorComponent.Stop();
        }

        public void StartTalkingAnimation()
        {
            directorComponent.Play(talkingAnimationTimelineAsset);
        }

        public void StopTalkingAnimation()
        {
            directorComponent.Stop();
            directorComponent.Play(defaultAnimationTimelineAsset);
        }
    }
}
