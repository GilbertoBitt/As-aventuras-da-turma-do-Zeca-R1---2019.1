using UnityEngine;

namespace TutorialSystem.Scripts
{
    public interface ICharacterAnimation
    {

        string GetCharacterName();
        Color GetCharacterNameColor();
        void StartTalking();
        void StopTalking();
        void StartLoopAnimation();
        void StopLoopAnimation();
    }
}
