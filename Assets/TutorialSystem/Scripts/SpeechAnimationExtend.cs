using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TutorialSystem.Scripts
{
    public class SpeechAnimationExtend : SerializedScriptableObject
    {
        public Sequence sequenceAnimation;

        public Sequence animation => sequenceAnimation;
    }
}
