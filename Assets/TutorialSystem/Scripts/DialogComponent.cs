using System;
using com.csutil;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace TutorialSystem.Scripts
{
    public class DialogComponent : MonoBehaviour
    {
        [ReadOnly]
        public DialogSettings dialogSetting;
        public DoTextDuration doTextDuration;
        public bool autoStart;
        public DialogInfo currentDialogInfo;
        public string subscribeEventName = "tutorialLudica";
        public string stopEventName => $"{subscribeEventName}Stopped";
        public string startEventName => $"{subscribeEventName}Started";
        public TextMeshProUGUI textMeshComponent;
        [ReadOnly]
        public Sequence dialogSequenceAnimation;

        [ReadOnly] public SoundManager soundManager;
        [ReadOnly] public EventBus eventBus;
        public void Start()
        {

            eventBus = (EventBus) EventBus.instance;
            dialogSetting = IoC.inject.Get<DialogSettings>(this);
            if (dialogSetting == null)
            {
                dialogSetting = ResourcesV2.LoadScriptableObjectInstance<DialogSettings>("DialogSettings.asset");
                IoC.inject.SetSingleton(dialogSetting);
            }

            if (soundManager == null)
            {
                soundManager = FindObjectOfType<SoundManager>();
            }

            if (autoStart && currentDialogInfo != null)
            {
                //TODO iniciar automaticamente no start
                eventBus.Publish(startEventName);
            }
        }

        public void StartDialog()
        {
            if (currentDialogInfo == null) return;

            StartDialog(currentDialogInfo);
        }

        public void StartDialog(DialogInfo dialogInfo)
        {

            //TODO adicionar animação do personagem piscando e falando.    
            dialogSequenceAnimation = DOTween.Sequence();
            dialogSequenceAnimation.SetId("tutorialSystem");
            dialogSequenceAnimation.AppendInterval(.3f);
            foreach (var speech in currentDialogInfo.speeches)
            {
                if (speech.speechAudioClip.enabled && speech.speechAudioClip != null)
                {
                    dialogSequenceAnimation.AppendCallback(() =>
                        soundManager.startVoiceFX(speech.speechAudioClip.audioClip));
                }

                dialogSequenceAnimation.Append(textMeshComponent.DOText(speech.speechText,
                    doTextDuration.enabled ? doTextDuration.timeBetweenSpeechs : dialogSetting.doTextDuration));

                dialogSequenceAnimation.AppendInterval(dialogSetting.timeBetweenSpeechs);
            }
        }

        public void StopTutorial()
        {
            DOTween.Kill("tutorialSystem");
            eventBus.Publish(stopEventName);
        }


    }

    [Serializable, Toggle("enabled")]
    public class DoTextDuration
    {
        public bool enabled;
        public float timeBetweenSpeechs;
    }

}
