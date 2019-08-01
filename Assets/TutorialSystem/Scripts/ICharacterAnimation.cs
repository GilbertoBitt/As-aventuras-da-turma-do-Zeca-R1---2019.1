using UnityEngine;

namespace TutorialSystem.Scripts
{
    public interface ICharacterAnimation
    {

        string GetCharacterName();
        Color GetCharacterNameColor();
        void StartDefaultAnimation();
        void StopDefaultAnimation();
        void StartTalkingAnimation();
        void StopTalkingAnimation();
    }
}
