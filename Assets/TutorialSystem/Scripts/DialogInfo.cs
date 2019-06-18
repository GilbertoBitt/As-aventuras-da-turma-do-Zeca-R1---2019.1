using System;
using System.Collections.Generic;
using com.csutil;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TutorialSystem.Scripts
{
    [CreateAssetMenu(menuName = "Eduqbrinq/Tutorial/DialogInfo", fileName = "DialogInfo")]
    public class DialogInfo : ScriptableObject
    {
        [ReadOnly]
        public DialogSettings dialogSettings;
        [ListDrawerSettings(ShowIndexLabels = false)]
        public List<SpeechInfo> speeches = new List<SpeechInfo>();

        private void OnValidate()
        {
            if (dialogSettings == null)
            {
                dialogSettings = ResourcesV2.LoadScriptableObjectInstance<DialogSettings>("DialogSettings.asset");
            }
        }
    }
}
