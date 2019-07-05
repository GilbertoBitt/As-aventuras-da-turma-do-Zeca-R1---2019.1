using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialSystem.Scripts
{
    public class TeacherComponent : MonoBehaviour, ICharacterAnimation
    {
        public Color characterNameColor;
        public string characterName = String.Empty;
        public Image overlayTalkSprite;
        public Image blinkSprite;

        private Sequence _talkingSequence;
        private Sequence _blinkSequence;

        public float talkingSpeed = 0.3f;
        public float blinkSpeed = 0.3f;

        public void SetupTalkingSequence()
        {
            _talkingSequence = DOTween.Sequence();
            _talkingSequence.SetId(000);
            _talkingSequence.AppendCallback(ToggleMouth);
            _talkingSequence.AppendInterval(talkingSpeed);
            _talkingSequence.SetLoops(-1, LoopType.Restart);
        }

        public void SetupBlinkLoopAnimation()
        {
            _blinkSequence = DOTween.Sequence();
            _blinkSequence.SetId(001);
            _blinkSequence.AppendCallback(ToggleBlink);
            _blinkSequence.AppendInterval(blinkSpeed);
            _blinkSequence.SetLoops(-1, LoopType.Restart);

        }

        public void ToggleMouth()
        {
            overlayTalkSprite.enabled = !overlayTalkSprite.isActiveAndEnabled;
        }

        public void ToggleBlink()
        {
            blinkSprite.enabled = !blinkSprite.isActiveAndEnabled;
        }

        public string GetCharacterName() => characterName;
        public Color GetCharacterNameColor() => characterNameColor;

        public void StartTalking()
        {
            SetupTalkingSequence();
            _talkingSequence.Play();
        }

        public void StopTalking()
        {
            _talkingSequence.Kill(true);
            overlayTalkSprite.enabled = false;
        }

        public void StartLoopAnimation()
        {
            SetupBlinkLoopAnimation();
            _blinkSequence.Play();
        }

        public void StopLoopAnimation()
        {
            _blinkSequence.Kill(true);
            blinkSprite.enabled = false;
        }
    }
}
