using UnityEngine;

namespace TutorialSystem.Scripts
{
    [CreateAssetMenu(menuName = "Eduqbrinq/Tutorial/DialogSettings", fileName = "DialogSettings")]
    public class DialogSettings : ScriptableObject
    {
        [Range(0.1f, 5f)]
        public float timeBetweenSpeechs = .6f;

        [Range(0.1f, 5f)]
        public float doTextDuration = 1f;

        public float textDefaultDuration = 5f;

    }
}
